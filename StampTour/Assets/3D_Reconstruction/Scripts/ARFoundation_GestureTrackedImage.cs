namespace RapidFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public partial class ARFoundation_GestureTrackedImage : MonoBehaviour //Data 
    {
        [SerializeField] private GestureManager gestureManager;
        [SerializeField] private ObjectManipulation objectManipulation;
        [SerializeField] private UnityEvent activeEvent;
        [SerializeField] private UnityEvent deactiveEvent;
    }
    public partial class ARFoundation_GestureTrackedImage : MonoBehaviour //Enable Disable 
    {
        private void OnEnable()
        {
            objectManipulation.Reset();
            gestureManager.m_OnPinchEvent.AddListener(objectManipulation.ScaleObjectBasedPinch);
            gestureManager.m_OnSwipeEvnet.AddListener(objectManipulation.MoveObjectBasedSwipe);
            gestureManager.m_OnDragEvnet.AddListener(objectManipulation.RotateObjectBasedDrag);
        }
        private void OnDisable()
        {
            gestureManager.m_OnPinchEvent.RemoveListener(objectManipulation.ScaleObjectBasedPinch);
            gestureManager.m_OnSwipeEvnet.RemoveListener(objectManipulation.MoveObjectBasedSwipe);
            gestureManager.m_OnDragEvnet.RemoveListener(objectManipulation.RotateObjectBasedDrag);
        }
    }
    public partial class ARFoundation_GestureTrackedImage : MonoBehaviour //Property Active Deactive
    {
        public void Active()
        {
            objectManipulation.gameObject.SetActive(true);
            activeEvent?.Invoke();
        }
        public void Deactive()
        {
            objectManipulation.gameObject.SetActive(false);
            deactiveEvent?.Invoke();
        }
    }
}
