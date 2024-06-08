namespace RapidFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    public partial class TrackedImageInformation : MonoBehaviour //Inner
    {
        public class ARTrackedImageTimerData
        {
            public ARTrackedImage ARTrackedImage;
            public float LimitTimer = 0;
        }
    }
    public partial class TrackedImageInformation : MonoBehaviour //Data
    {
        
        //Key는 ARTrackedImage 체킹용, Value는 타이머 값
        private Dictionary<ARTrackedImage, ARTrackedImageTimerData> arTrackedImageTimerDataDictionary = default; //arTrackedImage Dictionary
        private List<ARTrackedImage> removeARTrackedImageTimerList = default;
        //Main
        private Dictionary<string, GameObject> ARF_GestureDictionary = default; // 저장 딕셔너리
        [SerializeField] private Camera ARF_Camera; //AR Fondation Camera
        [SerializeField] ARTrackedImageManager m_TrackedImageManager; //트래킹 매니저
        [SerializeField] private List<GameObject> ARF_GestureObjectList; // 제스쳐 오브젝트 리스트
        [Header("Property")]
        [SerializeField] private float limitTimer = 1f; //제한시간
        [SerializeField] private float limitDistance = 100; //카메라 거리 제한
    }
    public partial class TrackedImageInformation : MonoBehaviour //초기 설정
    {
        private void Allocate()
        {
            //할당
            arTrackedImageTimerDataDictionary = new Dictionary<ARTrackedImage, ARTrackedImageTimerData>();
            removeARTrackedImageTimerList = new List<ARTrackedImage>();
            ARF_GestureDictionary = new Dictionary<string, GameObject>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            for(int index = 0; index < ARF_GestureObjectList.Count; index++)
            {
                ARF_GestureDictionary.Add(ARF_GestureObjectList[index].name, ARF_GestureObjectList[index]);
            }
            
        }
    }

    public partial class TrackedImageInformation : MonoBehaviour // 메인
    {
        private void Start()
        {
            Initialize();
        }
        private void Update()
        {
            Progress();
        }
        private void Progress() //트래킹 이미지 타이머 계산
        {
            if (arTrackedImageTimerDataDictionary.Count != 0)
            {
                //리스트 변환
                List<ARTrackedImageTimerData> arTrackedImageTimerDataList = arTrackedImageTimerDataDictionary.Values.ToList();

                for (int index = 0; index < arTrackedImageTimerDataList.Count; index++)
                {
                    //시간으로 제거
                    Progress_LimitTimer(arTrackedImageTimerDataList[index]);

                    //거리에 따른 제거
                    Progress_LimitDistance(arTrackedImageTimerDataList[index]);
                }

                if (removeARTrackedImageTimerList.Count != 0) //제거된 객체가 있는지
                {
                    for (int index = 0; index < removeARTrackedImageTimerList.Count; index++)
                    {
                        //값이 있는지 체크
                        if (arTrackedImageTimerDataDictionary.ContainsKey(removeARTrackedImageTimerList[index]) == true)
                        {
                            //있으면 제거
                            arTrackedImageTimerDataDictionary.Remove(removeARTrackedImageTimerList[index]);
                        }
                    }
                    //이후 리스트 비우기
                    removeARTrackedImageTimerList.Clear();
                }
            }
        }
        void OnCompleteButtonClick()
        {
            if (GameManager.Instance.GetIsSceneFinished("3D_Reconstruction"))
            {
                GameManager.RollbackMainScene();
            }
        }
        private void Progress_LimitTimer(ARTrackedImageTimerData arTrackedImageTimerData)
        {

            if (arTrackedImageTimerData.ARTrackedImage.trackingState == TrackingState.Limited)
            {
                //시간에 대한 부분 
                if (arTrackedImageTimerData.LimitTimer >= limitTimer) //시간이 리밋시간을 초과했는지
                {
                    string trackedImageName = arTrackedImageTimerData.ARTrackedImage.referenceImage.name;

                    if (trackedImageName != null && ARF_GestureDictionary.TryGetValue(trackedImageName, out GameObject ARF_GestureObject) == true)
                    {
                        ARF_GestureObject.SetActive(false);
                    }

                    removeARTrackedImageTimerList.Add(arTrackedImageTimerData.ARTrackedImage);
                    GameManager.Instance.SetIsSceneFinished("3D_Reconstruction", true);
                    //OnCompleteButtonClick();


                }
                else //리밋 시간이 안되었으면 시간 증가
                {
                    arTrackedImageTimerData.LimitTimer += Time.deltaTime;
                }
            }
        }
        private void Progress_LimitDistance(ARTrackedImageTimerData arTrackedImageTimerData)
        {

            if (arTrackedImageTimerData.ARTrackedImage.trackingState == TrackingState.Tracking)
            {
                string trackedImageName = arTrackedImageTimerData.ARTrackedImage.referenceImage.name;

                if (trackedImageName != null && ARF_GestureDictionary.TryGetValue(trackedImageName, out GameObject ARF_GestureObject) == true)
                {
                    //카메라와 거리를 구함.
                    float distance = Vector3.Distance(ARF_Camera.gameObject.transform.position, ARF_GestureObject.transform.position);

                    if (distance > limitDistance) //거리를 벗어났다면
                    {
                        ARF_GestureObject.SetActive(false);

                        //제거 목록 추가
                        removeARTrackedImageTimerList.Add(arTrackedImageTimerData.ARTrackedImage);
                        GameManager.Instance.SetIsSceneFinished("3D_Reconstruction",true);
                        //OnCompleteButtonClick();
                    }
                }
            }
        }
    }
    public partial class TrackedImageInformation : MonoBehaviour // OnEnable || Disable 
    {
        private void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

        private void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    }
    public partial class TrackedImageInformation : MonoBehaviour //속성
    {
        private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                NewImage(newImage);
            }

            foreach (var updatedImage in eventArgs.updated)
            {
                UpdateImage(updatedImage);
            }

            foreach (var removedImage in eventArgs.removed)
            {
            }
        }
        private void NewImage(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState != TrackingState.None)
            {
                // 이미지 크기에 맞게 사이즈 재조절
                trackedImage.transform.localScale = new Vector3(trackedImage.size.x, trackedImage.size.x, trackedImage.size.y);
            }

            //딕셔너리 데이터 적재 유무
            if (arTrackedImageTimerDataDictionary.ContainsKey(trackedImage) == false) //포함되어 있지 않다면
            {
                ARTrackedImageTimerData arTrackedImageTimerData = new ARTrackedImageTimerData();
                arTrackedImageTimerData.ARTrackedImage = trackedImage;
                arTrackedImageTimerData.LimitTimer = 0;

                arTrackedImageTimerDataDictionary.Add(arTrackedImageTimerData.ARTrackedImage, arTrackedImageTimerData);      
            }
        }
        private void UpdateImage(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState != TrackingState.None)
            {
                // 이미지 크기에 맞게 사이즈 재조절
                trackedImage.transform.localScale = new Vector3(trackedImage.size.x, trackedImage.size.x, trackedImage.size.y);
            }

            //딕셔너리 데이터 적재 유무
            if (arTrackedImageTimerDataDictionary.ContainsKey(trackedImage) == false) //포함되어 있지 않다면
            {
                ARTrackedImageTimerData arTrackedImageTimerData = new ARTrackedImageTimerData();
                arTrackedImageTimerData.ARTrackedImage = trackedImage;
                arTrackedImageTimerData.LimitTimer = 0;

                arTrackedImageTimerDataDictionary.Add(arTrackedImageTimerData.ARTrackedImage, arTrackedImageTimerData);
            }
            else
            {
                if (arTrackedImageTimerDataDictionary[trackedImage].ARTrackedImage.trackingState != TrackingState.Limited) //트래킹일 경우
                {
                    arTrackedImageTimerDataDictionary[trackedImage].LimitTimer = 0;

                    //트래킹일 경우만 동기화
                    string trackedImageName = trackedImage.referenceImage.name;

                    if (trackedImageName != null && ARF_GestureDictionary.TryGetValue(trackedImageName, out GameObject ARF_GestureObject) == true)
                    {
                        ARF_GestureObject.transform.position = trackedImage.transform.position;
                        ARF_GestureObject.transform.rotation = trackedImage.transform.rotation;
                        ARF_GestureObject.transform.localScale = trackedImage.transform.localScale;
                        ARF_GestureObject.SetActive(true);
                        GameManager.Instance.SetIsSceneFinished("3D_Reconstruction", true);
                    }

                }
            }
        }
    }
}
