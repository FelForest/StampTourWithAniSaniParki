using System.Collections;
using System.Collections.Generic;
using JicsawPuzzle;
using UnityEngine;

public class JicsawPuzzleManager : BaseInitializeObject
{
#region Singleton
    protected static JicsawPuzzleManager _instance;
    public static JicsawPuzzleManager Instance
    {
        get 
        { 
            if (_instance == null)
            {
                _instance = FindObjectOfType<JicsawPuzzleManager>();
                
                if (_instance == null)
                {
                    GameObject obj = new GameObject("JicsawPuzzleManager");
                    _instance = obj.AddComponent<JicsawPuzzleManager>();

                    // DontDestroyOnLoad(obj);
                }
            } 

            return _instance;
        }
    }

    private void SingletonAwake() 
    {
        if (_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

            return;
        }
    }
#endregion

    public JicsawPuzzleBoardManager BoardManager;
    public JicsawPuzzleUIManager UIManager;

    private void Awake() 
    {
        SingletonAwake();
    }

    protected override void Start()
    {
        if (BoardManager == null)
        {
            BoardManager = GetComponentInChildren<JicsawPuzzleBoardManager>();
        }

        if (UIManager == null)
        {
            UIManager = GetComponentInChildren<JicsawPuzzleUIManager>();
        }

        StartCoroutine(InitInternal());
        StartCoroutine(StartInternal());
    }

    protected override IEnumerator ActiveObject(BaseInitializeObject targetObj)
    {
        targetObj.gameObject.SetActive(true);
        yield return new WaitUntil(()=> targetObj.IsInitialized);
    }

    protected IEnumerator InitInternal()
    {
        yield return ActiveObject(UIManager);

        IsInitialized = true;
    }

    public IEnumerator StartInternal()
    {
        yield return new WaitUntil(()=>IsInitialized);
        yield return UIManager.Play();
    }

    public void SetActiveARCamera(bool isActive)
    {
        Debug.Log($"ARCamera Active : {isActive}");
    }

    public void CheckMarker()
    {
        StartCoroutine(CheckMarkerIntenal());
    }

    protected IEnumerator CheckMarkerIntenal()
    {
        // 마커 실패 시, while 반복
            yield return UIManager.Sequence4();
        // 마커 성공 시,
            //마커 끄기
            yield return BoardManager.Play();
            yield return UIManager.Sequence5();
    }

    public void PuzzleStart()
    {
        StartCoroutine(PuzzleStartIntenal());
    }
    protected IEnumerator PuzzleStartIntenal()
    {
        yield return UIManager.Sequence6();
    }

    public void PuzzleClear()
    {
        StartCoroutine(PuzzleClearInternal());
    }

    protected IEnumerator PuzzleClearInternal()
    {
        yield return UIManager.Sequence7();
    }
}
