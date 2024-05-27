using System;
using System.Collections;
using System.Xml.Schema;
using JicsawPuzzle;
using UnityEngine;
using UnityEngine.Android;

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
    public ARTrackingManager TrackingManager;
    public GameObject TrackingObject;

    private Coroutine curCoroutine;

    private void Awake() 
    {
        SingletonAwake();
    }

    protected override void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

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
        TrackingManager.enabled = false;
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
        TrackingManager.enabled = true;
    }

    protected IEnumerator FailedMarkerIntenal()
    {
        yield return UIManager.Sequence4();
    }

    protected IEnumerator PassMarkerIntenal()
    {
        yield return BoardManager.Play();
        BoardManager.PausePuzzlePlay(true);
        yield return UIManager.Sequence5();
        BoardManager.PausePuzzlePlay(false);
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
        TrackingManager.enabled = false;
        StartCoroutine(PuzzleClearInternal());
    }

    protected IEnumerator PuzzleClearInternal()
    {
        yield return UIManager.Sequence7();
    }

#region Tracking Event Trasmitter
    public void OnTrackingAddedEvent(string trackedId, Transform trakedTrasform)
    {
        if (trackedId == String.Empty) return;
        
        // If Target Find, Pass Marker Sequence
        if (BoardManager.CheckBoardName(trackedId))
        {
            if (curCoroutine != null) StopCoroutine(curCoroutine);
             
            curCoroutine = StartCoroutine(PassMarkerIntenal());
            TrackingObject.transform.position = trakedTrasform.position;
            // Debug.LogWarning("Find Traking Image");
        }
        else
        {
            if (curCoroutine != null) StopCoroutine(curCoroutine);

            curCoroutine = StartCoroutine(FailedMarkerIntenal());
            // Debug.LogWarning("Failed Traking Image");
        }
    }
    public void OnTrackingUpdateEvent(string trackedId, Transform trakedTrasform)
    {
        if (BoardManager.CheckBoardName(trackedId))
        {
            if (BoardManager.IsPlaying())
            {
                BoardManager.PausePuzzlePlay(false);
                var pos = trakedTrasform.position;
                // TrackingObject.transform.position = pos;
                var rot = trakedTrasform.rotation;

                // rot.x += 90;
                // TrackingObject.transform.rotation = rot;
                // Debug.LogWarning(" Traking Image");
            }
            else
            {
                OnTrackingAddedEvent(trackedId, trakedTrasform);
            }

        }
    }

    public void OnTrackingRemovedEvent(string trackedId)
    {
        // if (BoardManager.CheckBoardName(trackedId))
        // {
        //     BoardManager.PausePuzzlePlay(true);
        //     TrackingObject.transform.position = Vector3.zero;
        //     TrackingObject.transform.rotation = Quaternion.identity;
        //     Debug.LogWarning("Missing Traking Image");
        // }
        // Debug.LogWarning("Missing Traking Image");
    }
#endregion
}