using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JicsawPuzzle
{
    public class ResultUIController : BaseUIController
    {
        public ResultInfoComponent ResultInfoComponent;
        public NextMissionButton NextMissionButton;

        public override IEnumerator Play()
        {
            gameObject.SetActive(true);
            Debug.Log(gameObject.activeSelf);
            yield return ResultInfoComponent.Play();
            yield return NextMissionButton.Play();
        }

        protected override void Start() {
            if (ResultInfoComponent == null)
            {
                ResultInfoComponent = GetComponentInChildren<ResultInfoComponent>();
            }
            if (NextMissionButton == null)
            {
                NextMissionButton = GetComponentInChildren<NextMissionButton>();
            }

            IsInitialized = true;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
