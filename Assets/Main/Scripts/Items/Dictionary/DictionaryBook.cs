using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DictionaryBook : MonoBehaviour
{
    [SerializeField] private WordData[] _words;
    [field: SerializeField] public Transform FrontBinding { get; private set; }
    private DictionaryStateMachine _stateMachine;

    [Header("Transform")]
    [SerializeField] private Transform _openedAttach;
    [SerializeField] private Transform _scaleWrapper;
    [field: SerializeField] public Transform Camera { get; private set; }
    public Vector3 DefaultScale { get; private set; }
    private bool _isOpenable = true;

    [Header("Book UI")]
    [SerializeField] private Canvas _pagesCanvas;
    [SerializeField] private Transform _pagesGroup;
    [SerializeField] private DictionaryPage _page;
    private List<DictionaryPage> _pages;
    private int _currentPage = 0; //current page is the page on the right side of book

    [SerializeField] private Button _leftButton, _rightButton;

    private void Awake()
    {
        _pages = new List<DictionaryPage>();
        _stateMachine = new DictionaryStateMachine(this);
    }

    private void OnEnable()
    {
        DictionaryManager.OnWordWrite += AddWord;
    }

    void Start()
    {
        DefaultScale = _scaleWrapper.localScale;

        var pagesCount = (int)Mathf.Ceil(_words.Length / 2f);
        for (int i = 0; i < pagesCount; i++)
        {
            var page = Instantiate(_page, _pagesGroup);
            page.gameObject.SetActive(true);
            _pages.Add(page);
        }

        _pages.Reverse();

        _leftButton.gameObject.SetActive(_currentPage != 0);
        _rightButton.gameObject.SetActive(_pages.Count > 0);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void AddWord(WordData word, int count)
    {
        if ((count + 1) <= _pages.Count * 2)
        {
            var page = _pages[(count - 1) / 2];
            var side = count % 2 == 0 ? page.Back : page.Front;
            var wasActive = side.gameObject.activeSelf;
            side.SetWord(word);
        }
    }

    public void TurnThePage(bool left = false)
    {
        var pageToTurn = _currentPage + (left ? -1 : 0);
        if (pageToTurn >= 0 && pageToTurn < _pages.Count)
        {
            _pages[pageToTurn].Turn();
            _currentPage = Mathf.Clamp(_currentPage + (left ? -1 : 1), 0, _pages.Count);

            _leftButton.gameObject.SetActive(_currentPage != 0);
            _rightButton.gameObject.SetActive(_currentPage != _pages.Count);
        }
    }

    public void EnableCanvas(bool enable)
    {
        if(!enable)
            for (int i = _pages.Count - 1; i >= 0; i--)
                _pages[i].Close();

        _pagesCanvas.gameObject.SetActive(enable);
    }

    public void Resize(Vector3 scale)
    {
        _scaleWrapper.localScale = scale;
    }

    public void LockOpening(HoverEnterEventArgs args)
    {
        _isOpenable = false;
    }

    public void UnlockOpening(HoverExitEventArgs args)
    {
        _isOpenable = true;
    }

    public void Close(SelectEnterEventArgs args)
    {
        if(args.interactorObject.GetType() != typeof(XRSocketInteractor))
        {
            Debug.Log("Closed");
            _stateMachine.ChangeState(_stateMachine.CloseState, _openedAttach);
        } else
        {
            Debug.Log("Attached");
            transform.SetParent(_openedAttach);
        }
    }

    public void Open(SelectExitEventArgs args)
    {
        if(args.interactorObject.GetType() != typeof(XRSocketInteractor) && _isOpenable)
        {
            Debug.Log("Opened");
            _stateMachine.ChangeState(_stateMachine.OpenState, _openedAttach);
        }
    }

    private void OnDisable()
    {
        DictionaryManager.OnWordWrite -= AddWord;
    }
}
