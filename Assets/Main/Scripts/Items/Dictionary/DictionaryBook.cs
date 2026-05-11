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
    [SerializeField] private Transform _attachPoint;
    [field: SerializeField] public Transform ScaleWrapper {  get; private set; }
    [field: SerializeField] public Transform Camera { get; private set; }
    [SerializeField] private float _minDistanceToOpen = 1;
    [SerializeField] private Renderer _renderer;

    public Vector3 DefaultScale { get; private set; }
    private Coroutine _returning;
    private bool _added = false;
    private float _returingMoveSpeed = 10;
    private float _returingRotationSpeed = 700;

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
        DefaultScale = ScaleWrapper.localScale;

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

    public void Close(SelectExitEventArgs args)
    {
        if (_added && _stateMachine.Current != _stateMachine.OpenState)
            StartReturing();

        _stateMachine.ChangeState(_stateMachine.CloseState);
    }

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

    public void StartReturing()
    {
        _returning = StartCoroutine(ReturnToAttach());
    }

    public void StopReturing()
    {
        if(_returning != null)
        {
            StopCoroutine(_returning);
            FinishTransform();
        }    
    }

    private void FinishTransform()
    {
        transform.localRotation = Quaternion.identity;
        //transform.rotation = _attachPoint.rotation;
    }

    public void Open(SelectEnterEventArgs args)
    {
        if (_added && args.interactorObject.GetType() == typeof(NearFarInteractor))
            _stateMachine.ChangeState(_stateMachine.GrabState);
        else if(args.interactorObject.GetType() == typeof(XRSocketInteractor))
        {
            _stateMachine.ChangeState(_stateMachine.CloseState);
            transform.SetParent(_attachPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            args.interactorObject.transform.gameObject.SetActive(false);
            _added = true;
        }
    }

    private void OnDisable()
    {
        DictionaryManager.OnWordWrite -= AddWord;
    }

    public bool IsInFrontOfCamera() => 
        (Camera.position - transform.position).magnitude >= _minDistanceToOpen && _renderer.isVisible;
}
