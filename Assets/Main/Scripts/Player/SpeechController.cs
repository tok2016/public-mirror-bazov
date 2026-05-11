using DotNetEnv;
using eToile;
using FuzzySharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

class TranscribeData
{
    public string result;
}

public class SpeechController : MonoBehaviour
{
    [SerializeField] private InputActionReference _recordAction;
    [SerializeField] private List<CollectableItem> _items;
    [SerializeField] private int _scoreTreshold = 33;
    [SerializeField] private int _recordingTime = 5;
    [SerializeField] Transform _itemAttachPoint;

    private int _frequency = 48000;

    private float _recordingTreshold = 0.5f;
    private float _recordingTimer;

    private string _deviceName;
    private bool _isPressing = false;

    private AudioClip _audioClip;
    private HttpClient _httpClient;

    private void Awake()
    {
        Env.Load();
        var key = Environment.GetEnvironmentVariable("YANDEX_AI_STUDIO_API_KEY") ?? throw new Exception("No API key");

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Api-Key {key}");
    }

    void Start()
    {
        _deviceName = Microphone.devices[0];
        StartCoroutine(RequestPermission());
    }

    public void CheckItemSpeech()
    {
        if (_recordAction.action.IsPressed() && !_isPressing && !Microphone.IsRecording(_deviceName))
        {
            _audioClip = Microphone.Start(_deviceName, false, _recordingTime, _frequency);
            Debug.Log("Start");
            _recordingTimer = _recordingTime;
            _isPressing = true;
        }

        if (_recordAction.action.WasCompletedThisFrame())
        {
            if (Microphone.IsRecording(_deviceName))
            {
                Microphone.End(_deviceName);
                Debug.Log("End");
            }

            if (_recordingTimer >= 0 && _recordingTimer <= _recordingTime - _recordingTreshold)
                TranscribeRecord();

            _isPressing = false;
        }

        if (Microphone.IsRecording(_deviceName))
        {
            if (_recordingTimer <= 0)
            {
                Microphone.End(_deviceName);
                Debug.Log("End");
            }
            else
                _recordingTimer -= Time.deltaTime;
        }
    }

    private System.Collections.IEnumerator RequestPermission()
    {
        if(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);

        while(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            yield return 0;
    }

    private async void TranscribeRecord()
    {
        if(_audioClip && _audioClip.loadState == AudioDataLoadState.Loaded)
        {
            Debug.Log("Transcribing");

            var wav = OpenWavParser.AudioClipToByteArray(_audioClip, OpenWavParser.Resolution._16bit);

            var originUri = Environment.GetEnvironmentVariable("YANDEX_AI_STUDIO_URI") ?? throw new Exception("No URI");
            var folder = Environment.GetEnvironmentVariable("YANDEX_FOLDER_ID") ?? throw new Exception("No folder ID");

            var content = new ByteArrayContent(wav);
            var uri = new UriBuilder("https://stt.api.cloud.yandex.net/speech/v1/stt:recognize");

            var query = HttpUtility.ParseQueryString(uri.Query);
            query["lang"] = "ru-RU";
            query["topic"] = "general";
            query["profanityFilter"] = "false";
            query["rawResults"] = "true";
            query["format"] = "lpcm";
            query["sampleRateHertz"] = _frequency.ToString();
            query["folderId"] = folder;
            uri.Query = query.ToString();

            var response = await _httpClient.PostAsync(uri.ToString(), content);
            var strData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TranscribeData>(strData);

            if (data != null && data.result != null)
                MatchWord(data.result);
            else
                throw new Exception("No results");
        }
    }

    private void MatchWord(string transcribed)
    {
        var words = _items.Select(item => item.Data.Word.Title.ToLower().Replace('ё', 'е')).ToArray();
        var result = Process.ExtractOne(transcribed, words, (s) => s);

        Debug.Log(transcribed);
        Debug.Log($"{result.Value}: {result.Score}");

        if (result.Score >= _scoreTreshold && result.Index >= 0 && result.Index < _items.Count)
            _items[result.Index].RestoreSocketedItem(_itemAttachPoint);
        else
            Debug.Log("Ничего нет. Может, повторишь снова?");
    }

    public void RemoveItem(CollectableItem item)
    {
        _items.Remove(item);
    }
}
