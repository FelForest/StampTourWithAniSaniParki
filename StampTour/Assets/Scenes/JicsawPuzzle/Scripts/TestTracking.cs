using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JicsawPuzzle
{
    public class TestTracking : MonoBehaviour
    {
        public TMP_Text LogTMP;
        public GameObject TargetObject;
        public Transform ScaleObject;

        private void Update()
        {
            if (TargetObject)
            LogTMP.text = $"Position : {TargetObject.transform.position}\n"
                + $"Rotation : {TargetObject.transform.eulerAngles}\n"
                + $"Size : {TargetObject.transform.localScale}\n"
                + $"CanvasPosition : {ScaleObject.position}\n"
                + $"CanvasRotation : {ScaleObject.eulerAngles}\n"
                + $"Canvas Size : {ScaleObject.transform.localScale}";
        }

        public void ChangeScale(float inputValue)
        {
            Vector3 targetScale = ScaleObject.transform.localScale;
            targetScale.x += inputValue;
            targetScale.y += inputValue;
            targetScale.z += inputValue;
            ScaleObject.transform.localScale = targetScale;
        }

        public void TranslateZ(float inputValue)
        {
            var t = ScaleObject.transform.position;
            t.z += inputValue;
            ScaleObject.transform.position = t;
        }
    }
}