namespace RapidFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ObjectManipulation : MonoBehaviour
    {
        [System.Flags]
        public enum RotationAxis

        {
            //각 축별 회전
            None = 0,
            X = 1,
            Y = 1 << 1,
            Z = 1 << 2,
            XY = X | Y,
            XZ = X | Z,
            YZ = Y | Z,
            XYZ = X | Y | Z
        };

        //회전 타입
        public enum AxisRotationType
        {
            World,
            Local
        }
        //회전, 스케일링, 평행이동
        public Transform m_RotationPivot;
        public Transform m_ScalePivot;
        public Transform m_MovementPivot;

        public Transform m_RotationRoot;
        public Transform m_ScaleRoot;
        public Transform m_MovementRoot;

        public Transform m_Content;

        public RotationAxis m_RotationAxis = RotationAxis.XYZ;

        public AxisRotationType m_PitchRotationType = AxisRotationType.World;
        public AxisRotationType m_YawRotationType = AxisRotationType.Local;
        public AxisRotationType m_RollRotationType = AxisRotationType.World;


        public float m_LockAngle = 360;


        public bool m_IsActivity = false;


        //Custom
        public void OnEnable()
        {
            Reset();
        }

        public void SetActivity(bool value)
        {
            m_IsActivity = value;
        }

        public bool GetActivity()
        {
            return m_IsActivity;
        }

        //객체 생성 시 초기 설정
        public void Awake()
        {
            m_Content.SetParent(m_ScaleRoot);
            m_Content.localPosition = Vector3.zero;
            m_Content.localRotation = Quaternion.identity;
            m_Content.localScale = Vector3.one;
        }

        //초기 상태 리셋
        public void Reset()
        {
            m_RotationRoot.localRotation = Quaternion.identity;
            m_ScaleRoot.localScale = Vector3.one;
            m_MovementRoot.localPosition = Vector3.zero;
        }

        //핀치에 따른 스케일링
        public void ScaleObjectBasedPinch(float pinchValue)
        {
            var localScale = m_ScaleRoot.localScale.x;
            localScale += pinchValue;

            ScaleObject(localScale);
        }

        //스케일링
        public void ScaleObject(float value)
        {
            if (!m_IsActivity)
                return;

            value = Mathf.Clamp(value, 0.5f, 2f);

            m_ScaleRoot.localScale = value * Vector3.one;
        }

        //스와이프에 따른 평행이동
        public void MoveObjectBasedSwipe(Vector2 swipeValue)
        {
            //스크린 절반이동하면 1유닛 이동.
            MoveObject(swipeValue * 2);
        }

        //평행이동
        public void MoveObject(Vector2 movementValue)
        {
            if (!m_IsActivity)
                return;

            var forwardMovement = m_MovementPivot.forward * movementValue.y;
            var rightMovement = m_MovementPivot.right * movementValue.x;
            var movement = forwardMovement + rightMovement;

            m_MovementRoot.position += movement;
        }


        public void RotateObjectBasedEventTrigger(BaseEventData baseEventData)
        {
            var pointerEventData = baseEventData as PointerEventData;

            RotateObject(pointerEventData.delta / Screen.width * 180);
        }

        //드래그에 따른 회전
        public void RotateObjectBasedDrag(Vector2 dragValue)
        {
            RotateObject(dragValue * 180);
        }

        //회전
        public void RotateObject(Vector2 rotationValue)
        {
            var oldRotation = m_RotationRoot.rotation;

            RotateYaw(m_RotationRoot, m_RotationPivot, rotationValue.x);
            //RotatePitch(m_RotationRoot, m_RotationPivot, rotationValue.y); // pitch(x)축 고정 24.05.24 => object 밑면 보이지 않기 위해서
            //RotateRoll(m_RotationRoot, m_RotationPivot, rotationValue.y); // roll(z)축 고정 24.05.24 => object 밑면 보이지 않기 위해서
            if (!CheckRotationInLockAngle(m_RotationPivot.up, m_RotationRoot.up, m_LockAngle))
            {
                var angle = Vector3.Angle(m_RotationPivot.up, m_RotationRoot.up);

                float pivotAngle, rootAngle;
                Vector3 pivotUp, rootUp;
                m_RotationPivot.rotation.ToAngleAxis(out pivotAngle, out pivotUp);
                m_RotationRoot.rotation.ToAngleAxis(out rootAngle, out rootUp);

                var targetUp = Vector3.Lerp(pivotUp, rootUp, m_LockAngle / angle);
                var targetAngle = Mathf.Lerp(pivotAngle, rootAngle, m_LockAngle / angle);

                var targetRot = Quaternion.Lerp(m_RotationPivot.rotation, m_RotationRoot.rotation, m_LockAngle / angle).normalized;

                m_RotationRoot.rotation = targetRot;
            }
        }

        //회전에서의 yaw, pitch, roll 회전각
        //yaw -> y축 회전
        private void RotateYaw(Transform root, Transform pivot, float yaw)
        {
            if (!m_IsActivity)
                return;

            if ((m_RotationAxis & RotationAxis.Y) != RotationAxis.Y)
                return;

            var oldRotation = root.rotation;


            if (m_YawRotationType == AxisRotationType.World)
                root.RotateAround(root.position, pivot.up, -yaw);
            else
                root.RotateAround(root.position, root.up, -yaw);
        }

        //pitch -> x축 회전
        private void RotatePitch(Transform root, Transform pivot, float pitch)
        {
            if (!m_IsActivity)
                return;

            if ((m_RotationAxis & RotationAxis.X) != RotationAxis.X)
                return;

            var oldRotation = root.rotation;

            if (m_PitchRotationType == AxisRotationType.World)
                root.RotateAround(root.position, pivot.right, pitch);
            else
                root.RotateAround(root.position, root.right, pitch);
        }

        //roll -> z축 회전
        private void RotateRoll(Transform root, Transform pivot, float roll)
        {
            if (!m_IsActivity)
                return;

            if ((m_RotationAxis & RotationAxis.Z) != RotationAxis.Z)
                return;


            var oldRotation = root.rotation;

            if (m_RollRotationType == AxisRotationType.World)
                root.RotateAround(root.position, pivot.forward, roll);
            else
                root.RotateAround(root.position, root.forward, roll);
        }

        //회전 각도 제한
        private bool CheckRotationInLockAngle(Vector3 pivotUp, Vector3 rootUp, float lockAngle)
        {
            var angle = Vector3.Angle(pivotUp, rootUp);

            if (angle > lockAngle)
                return false;
            else
                return true;
        }

        // yaw, pitch, roll 범위 제한
        private void RotationClamp(Transform root, Transform pivot, Vector2 yawRange, Vector2 pitchRagne, Vector3 rollRange)
        {
            var localPivotEuler = (Quaternion.Inverse(pivot.rotation) * root.rotation).eulerAngles;


        }
    }

}
