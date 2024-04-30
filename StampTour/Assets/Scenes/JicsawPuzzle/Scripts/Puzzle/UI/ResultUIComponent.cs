using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JicsawPuzzle
{
    public class ResultUIComponent : MonoBehaviour, IPlay
    {
        public ResultInfoComponent ResultInfoComponent;
        public NextMissionButton NextMissionButton;

        public IEnumerator Play()
        {
            gameObject.SetActive(true);
            yield return ResultInfoComponent.Play();
            yield return NextMissionButton.Play();
        }

        private void Start() {
            if (ResultInfoComponent == null)
            {
                ResultInfoComponent = GetComponentInChildren<ResultInfoComponent>();
            }
            if (NextMissionButton == null)
            {
                NextMissionButton = GetComponentInChildren<NextMissionButton>();
            }

            gameObject.SetActive(false);
        }
    }
}
