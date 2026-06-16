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

[RequireComponent(typeof(SpeecControllerhHint))]
public class SpeechController : MonoBehaviour
{
    [Header("Recording")]
    [SerializeField] private InputActionReference _recordAction;
    [SerializeField] private int _scoreTreshold = 33;
    [SerializeField] private int _recordingTime = 5;
    public event Action onRecordingStart, onRecordingStop;
    public event Action<IGrabbable> onItemFound;

    [Header("Items Controll")]
    [SerializeField] private List<GrabbableObject> _items;
    [SerializeField] Transform _itemAttachPoint;

    private int _frequency = 48000;

    private float _recordingTreshold = 0.5f;
    private float _recordingTimer;

    private string _deviceName;
    private bool _isPressing = false;
    private bool _isLoading = false;

    private AudioClip _audioClip;
    private HttpClient _httpClient;

    private SpeecControllerhHint _controllerHint;

    private void Awake()
    {
        Env.Load();
        var key = Environment.GetEnvironmentVariable("YANDEX_AI_STUDIO_API_KEY") ?? throw new Exception("No API key");

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Api-Key {key}");

        _controllerHint = GetComponent<SpeecControllerhHint>();
    }

    void Start()
    {
        _deviceName = Microphone.devices[0];
        StartCoroutine(RequestPermission());
    }

    public void CheckItemSpeech()
    {
        if (_isLoading || !Permission.HasUserAuthorizedPermission(Permission.Microphone) 
            || _deviceName == null || _deviceName == "") return;

        if (_recordAction.action.IsPressed() && !_isPressing && !Microphone.IsRecording(_deviceName))
        {
            _audioClip = Microphone.Start(_deviceName, false, _recordingTime, _frequency);
            Debug.Log("Start");
            _controllerHint.ShowRecording();
            _recordingTimer = _recordingTime;
            _isPressing = true;

            onRecordingStart?.Invoke();
        }

        if (_recordAction.action.WasCompletedThisFrame())
        {
            if (Microphone.IsRecording(_deviceName))
            {
                Microphone.End(_deviceName);
                Debug.Log("End");
                onRecordingStop?.Invoke();
            }

            if (_recordingTimer >= 0 && _recordingTimer <= _recordingTime - _recordingTreshold)
            {
                TranscribeRecord();
                //StartCoroutine(TestTranscribe()); 
                _controllerHint.HideRecording();
            } else
                _controllerHint.CancelRecording();

            _isPressing = false;
        }

        if (Microphone.IsRecording(_deviceName))
        {
            if (_recordingTimer <= 0)
            {
                Microphone.End(_deviceName);
                Debug.Log("End");
                onRecordingStop?.Invoke();
            }
            else
            {
                _recordingTimer -= Time.deltaTime;
                _controllerHint.WarnRecording(_recordingTimer);
            }
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
        if (!_audioClip || _audioClip.loadState != AudioDataLoadState.Loaded)
            return;
        
        Debug.Log("Transcribing");

        _isLoading = true;

        var originUri = Environment.GetEnvironmentVariable("YANDEX_AI_STUDIO_URI") ?? throw new Exception("No URI");
        var folder = Environment.GetEnvironmentVariable("YANDEX_FOLDER_ID") ?? throw new Exception("No folder ID");

        var wav = OpenWavParser.AudioClipToByteArray(_audioClip, OpenWavParser.Resolution._16bit);
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

        try
        {
            var response = await _httpClient.PostAsync(uri.ToString(), content);
            var strData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TranscribeData>(strData);

            if (data != null && data.result != null)
                MatchWord(data.result);
            else
                ThrowError("No results");
        }
        catch
        {
            ThrowError("Request error");
        }

        _isLoading = false;
    }

    private IEnumerator<WaitForSeconds> TestTranscribe()
    {
        _isLoading = true;
        Debug.Log("Transcribing");
        yield return new WaitForSeconds(2);

        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
            _controllerHint.ShowError();
        else
        {
            _controllerHint.ShowResponse();
            _items[0].RestoreSocketedItem(_itemAttachPoint);
        }

        _isLoading = false;
    }

    private void ThrowError(string message)
    {
        _controllerHint.ShowError();
        throw new Exception(message);
    }

    private void MatchWord(string transcribed)
    {
        var words = _items
            .Where(item => item.Data.DialogueLine && item.Data.DialogueLine.Word)
            .Select(item => item.Data.DialogueLine.Word.Title.ToLower().Replace('ё', 'е'))
            .ToArray();

        var result = Process.ExtractOne(transcribed, words, (s) => s);

        Debug.Log(transcribed);
        Debug.Log($"{result.Value}: {result.Score}");

        if (result.Score >= _scoreTreshold && result.Index >= 0 && result.Index < _items.Count)
        {
            _items[result.Index].RestoreSocketedItem(_itemAttachPoint);
            _controllerHint.ShowResponse();
            onItemFound?.Invoke(_items[result.Index]);
        }  
        else
            Debug.Log("Ничего нет. Может, повторишь снова?");
    }

    public void AddItem(GrabbableObject item)
    {
        _items.Add(item);
    }

    public void RemoveItem(GrabbableObject item)
    {
        _items.Remove(item);
    }
}
