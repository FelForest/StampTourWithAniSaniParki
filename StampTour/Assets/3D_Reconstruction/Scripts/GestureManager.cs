namespace RapidFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //두 손가락 제스처
    public class GestureManager : MonoBehaviour
    {
        //제스처 인식 최소 거리
        private readonly float m_MinPinchDistance = 10f;
        private readonly float m_MinSwipeDistance = 10f;

        [System.Serializable] public class DragEvnet : UnityEngine.Events.UnityEvent<Vector2> { }
        [System.Serializable] public class PinchEvnet : UnityEngine.Events.UnityEvent<float> { }
        [System.Serializable] public class SwipeEvnet : UnityEngine.Events.UnityEvent<Vector2>{}

        public DragEvnet m_OnDragEvnet;
        public PinchEvnet m_OnPinchEvent;
        public SwipeEvnet m_OnSwipeEvnet;

        protected void Update()
        {
            // 터치 1개 -> 드래그
            if (Input.touchCount == 1)
            {
                Touch touch1 = Input.touches[0];

                if (touch1.phase == TouchPhase.Moved)
                {
                    var dragDistanceDelta = touch1.deltaPosition;
                    var normalizedDragDelta = dragDistanceDelta / Screen.width;
                    m_OnDragEvnet.Invoke(normalizedDragDelta);
                }
            }
            // 터치 2개 -> 핀치와 스와이프
            else if (Input.touchCount == 2)
            {
                //pitch 
                Touch touch1 = Input.touches[0];
                Touch touch2 = Input.touches[1];


                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    float pinchDistanceDelta;
                    if (CheckPinch(touch1, touch2, out pinchDistanceDelta))
                    {
                        var normalizedPinchDelta = pinchDistanceDelta / Screen.width;
                        m_OnPinchEvent.Invoke(normalizedPinchDelta);
                    }

                    Vector2 swipeDelta;
                    if (CheckSwipe(touch1, touch2, out swipeDelta))
                    {
                        var normalizedSwipeDelta = swipeDelta / Screen.width;
                        m_OnSwipeEvnet.Invoke(normalizedSwipeDelta);
                    }
                }
            }

#if UNITY_EDITOR
            Vector2 testRotationValue = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
                testRotationValue.x -= 1 * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                testRotationValue.x += 1 * Time.deltaTime;
            if (Input.GetKey(KeyCode.W))
                testRotationValue.y += 1 * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                testRotationValue.y -= 1 * Time.deltaTime;
            m_OnDragEvnet.Invoke(testRotationValue);

            if (Input.GetKey(KeyCode.KeypadMinus))
                m_OnPinchEvent.Invoke(-1 * Time.deltaTime);
            if (Input.GetKey(KeyCode.KeypadPlus))
                m_OnPinchEvent.Invoke(+1 * Time.deltaTime);

            Vector2 testSwipeValue = Vector2.zero;
            if (Input.GetKey(KeyCode.RightArrow))
                testSwipeValue.x += 1 * Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow))
                testSwipeValue.x -= 1 * Time.deltaTime;
            if (Input.GetKey(KeyCode.UpArrow))
                testSwipeValue.y += 1 * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow))
                testSwipeValue.y -= 1 * Time.deltaTime;

            if(testSwipeValue != Vector2.zero)
                m_OnSwipeEvnet.Invoke(testSwipeValue);
#endif
        }

        // 핀치
        private bool CheckPinch(Touch touch1, Touch touch2, out float pinchDelta)
        {
            pinchDelta = 0;

            
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                //Pinch 체크
                float pinchDistance = Vector2.Distance(touch1.position, touch2.position);
                float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
                                                      touch2.position - touch2.deltaPosition);
                pinchDelta = pinchDistance - prevDistance;

                //Pinch가 최소보간수치를 넘으면
                if (Mathf.Abs(pinchDelta) > m_MinPinchDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        //스와이프
        private bool CheckSwipe(Touch touch1, Touch touch2, out Vector2 swipeDelta)
        {
            swipeDelta = Vector2.zero;

            // ... if at least one of them moved ...
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                //Pinch 체크
                float pinchDistance = Vector2.Distance(touch1.position, touch2.position);
                float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
                                                      touch2.position - touch2.deltaPosition);
                float pinchDistanceDelta = pinchDistance - prevDistance;

                //Swipe 체크
                Vector2 oldTouchPoisition1 = touch1.position - touch1.deltaPosition;
                Vector2 oldTouchPoisition2 = touch2.position - touch2.deltaPosition;

                Vector2 oldTouchCenterPosition = (oldTouchPoisition1 + oldTouchPoisition2) / 2;
                Vector2 touchCenterPosition = (touch1.position + touch2.position) / 2;

                swipeDelta = touchCenterPosition - oldTouchCenterPosition;
                float swipeDeltaDistance = swipeDelta.magnitude;

                //Pinch가 최소보간수치를 넘지 못하고, Swipe가 최소보간수치를 넘으면
                if (Mathf.Abs(pinchDistanceDelta) <= m_MinPinchDistance && swipeDeltaDistance > m_MinSwipeDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}