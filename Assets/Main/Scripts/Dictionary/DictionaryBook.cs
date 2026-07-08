using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Manages dictionary as a physical book. Its pages display on UI canvas.
/// </summary>
public class DictionaryBook : MonoBehaviour
{
    [Header("Book")]
    [SerializeField] private WordData[] _words;

    /// <value>
    /// Transform of the front binding of book.
    /// </value>
    [field: SerializeField] public Transform FrontBinding { get; private set; }
    private DictionaryStateMachine _stateMachine;

    [Header("Transform")]
    [SerializeField] private Transform _attachPoint;

    /// <value>
    /// Parent for all inner book elements to scale.
    /// </value>
    [field: SerializeField] public Transform ScaleWrapper {  get; private set; }

    /// <value>
    /// Scale multiplier when book is opened.
    /// </value>
    [field: SerializeField] public float OpenedScale { get; private set; } = 1f;

    /// <value>
    /// Camera transform to track book visibility.
    /// </value>
    [field: SerializeField] public Transform Camera { get; private set; }
    [SerializeField] private float _minDistanceToOpen = 1;
    [SerializeField] private Renderer _renderer;

    /// <value>
    /// Start scale of the book wrapper.
    /// </value>
    public Vector3 DefaultScale { get; private set; }
    private Coroutine _returning;
    private float _returingMoveSpeed = 10;
    private float _returingRotationSpeed = 700;

    [Header("Book UI")]
    [SerializeField] private Canvas _pagesCanvas;
    [SerializeField] private Transform _pagesGroup;
    [SerializeField] private DictionaryPage _page;
    private List<DictionaryPage> _pages;
    private Dictionary<WordData, DictionaryPageSide> _pagesSides;
    private delegate float Round(float number);
    private int _currentPage = 0; //current page is the page on the right side of book

    public event Action<DictionaryPage> onPageTurn;

    [SerializeField] private Button _leftButton, _rightButton;

    [Header("Audio")]
    [SerializeField] private AudioClip _pageTurningSound;
    [SerializeField] private AudioClip _openingSound, _writingSound;
    private RandomizedAudioSource _audioSource;

    private void Awake()
    {
        _pages = new List<DictionaryPage>();
        _pagesSides = new Dictionary<WordData, DictionaryPageSide>();
        _stateMachine = new DictionaryStateMachine(this);
        _audioSource = GetComponent<RandomizedAudioSource>();
    }

    private void OnEnable()
    {
        DictionaryManager.OnWordWrite += AddWord;
    }

    /// <summary>
    /// Fills dictionary book with pages with words.
    /// Each page has two sides with only one word and description on each one.
    /// </summary>
    void Start()
    {
        DefaultScale = ScaleWrapper.localScale;
        Round round = _words.Length % 2 == 0 ? Mathf.Floor : Mathf.Ceil;

        for (int i = _words.Length - 1; i >= 0; i--)
        {
            var pageIndex = (int)round((_words.Length - i - 1) / 2f);
            var page = pageIndex >= _pages.Count ? CreatePage() : _pages[pageIndex];
            page.gameObject.SetActive(true);

            var side = i % 2 == 0 ? page.Front : page.Back;
            side.SetDefinition(_words[i]);

            if (DictionaryManager.IsWordStored(_words[i]))
                side.SetTitle(_words[i]);

            _pagesSides.Add(_words[i], side);
        }

        _pages.Reverse();

        _leftButton.gameObject.SetActive(_currentPage != 0);
        _rightButton.gameObject.SetActive(_pages.Count > 0);
        _pagesCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    /// <summary>
    /// Instantiates new page as the last child.
    /// </summary>
    /// <returns>Instantiated empty page of dictionary book.</returns>
    private DictionaryPage CreatePage()
    {
        var newPage = Instantiate(_page, _pagesGroup);
        _pages.Add(newPage);
        return newPage;
    }

    /// <summary>
    /// Fills word title on its own page and side.
    /// </summary>
    /// <param name="word">Word to fill the page with.</param>
    public void AddWord(WordData word)
    {
        if (_pagesSides.ContainsKey(word))
        {
            _pagesSides[word].SetTitle(word);
            PlaySound(_writingSound);
        } 
    }

    /// <summary>
    /// Turns one of the opened pages.
    /// </summary>
    /// <param name="left">If true, turns the left page.</param>
    public void TurnThePage(bool left = false)
    {
        var pageToTurn = _currentPage + (left ? -1 : 0);
        if (pageToTurn >= 0 && pageToTurn < _pages.Count)
        {
            _pages[pageToTurn].Turn();
            _currentPage = Mathf.Clamp(_currentPage + (left ? -1 : 1), 0, _pages.Count);

            _leftButton.gameObject.SetActive(_currentPage != 0);
            _rightButton.gameObject.SetActive(_currentPage != _pages.Count);

            PlaySound(_pageTurningSound);
            onPageTurn?.Invoke(_pages[pageToTurn]);
        }
    }

    /// <summary>
    /// Enables or disables book canvas. Turns all pages to the first one.
    /// </summary>
    /// <param name="enable">Whether to enable or disable canvas.</param>
    public void EnableCanvas(bool enable)
    {
        _pagesCanvas.gameObject.SetActive(enable);
        if (!enable)
        {
            for (int i = _pages.Count - 1; i >= 0; i--)
                _pages[i].Close();

            _currentPage = 0;
            _leftButton.gameObject.SetActive(false);
        }    
    }

    /// <summary>
    /// Closes book on release.
    /// </summary>
    /// <param name="args"></param>
    public void Close(SelectExitEventArgs args)
    {
        Close();
    }

    /// <summary>
    /// Closes book and returns it to attach position.
    /// </summary>
    public void Close()
    {
        if (_stateMachine.Current != _stateMachine.OpenState)
            StartReturing();

        _stateMachine.ChangeState(_stateMachine.CloseState);
    }

    /// <summary>
    /// Smoothly moves and rotates book to attach point.
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator ReturnToAttach()
    {
        var distance = 1f;
        while (distance > 0.01f
            || Quaternion.Angle(transform.rotation, _attachPoint.rotation) > 1f)
        {
            distance = Mathf.Clamp((transform.position - _attachPoint.position).magnitude, 0, 5);

            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, Vector3.zero, 
                _returingMoveSpeed * Time.deltaTime);

            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation, Quaternion.identity,
                _returingRotationSpeed * Time.deltaTime / distance);

            yield return null;
        }

        transform.localPosition = Vector3.zero;
        FinishTransform();
    }

    /// <summary>
    /// Returns book to attach position.
    /// </summary>
    public void StartReturing()
    {
        transform.SetParent(_attachPoint);
        _returning = StartCoroutine(ReturnToAttach());
    }

    /// <summary>
    /// Stops book returning.
    /// </summary>
    public void StopReturing()
    {
        if(_returning != null)
        {
            StopCoroutine(_returning);
            FinishTransform();
        }    
    }

    /// <summary>
    /// Instantly sets book transform to initial values.
    /// </summary>
    private void FinishTransform()
    {
        transform.localRotation = Quaternion.identity;
        _returning = null;
    }

    /// <summary>
    /// Sets book state to grabbed on grab. Book opens only when it's grabbed by the player.
    /// </summary>
    /// <param name="args"></param>
    public void Open(SelectEnterEventArgs args)
    {
        if (args.interactorObject.GetType() == typeof(NearFarInteractor))
            _stateMachine.ChangeState(_stateMachine.GrabState);
    }

    /// <summary>
    /// Plays given sound.
    /// </summary>
    /// <param name="clip">Sound to play.</param>
    public void PlaySound(AudioClip clip)
    {
        _audioSource.Play(clip);
    }

    /// <summary>
    /// Plays book opening/closing sound.
    /// </summary>
    public void PlayOpeningSound()
    {
        PlaySound(_openingSound);
    }

    private void OnDisable()
    {
        DictionaryManager.OnWordWrite -= AddWord;
    }

    /// <summary>
    /// Checks if the book is in camera visibility zone and is further than defined distance to open.
    /// </summary>
    /// <returns></returns>
    public bool IsInFrontOfCamera() => 
        (Camera.position - transform.position).magnitude >= _minDistanceToOpen && _renderer.isVisible;
}
