using System.Collections;
using DG.Tweening;

namespace JicsawPuzzle
{
    public class ResultUIController : BaseInitializeObject
    {
        public ResultInfoComponent ResultInfoComponent;
        public NextMissionButton NextMissionButton;

        public override IEnumerator Play()
        {
            if (ResultInfoComponent == null)
            {
                ResultInfoComponent = GetComponentInChildren<ResultInfoComponent>();
            }
            if (NextMissionButton == null)
            {
                NextMissionButton = GetComponentInChildren<NextMissionButton>();
            }
            gameObject.SetActive(true);
            // Debug.Log(gameObject.activeSelf);
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

        private void OnEnable() {
            transform.DOScale(1.0f, 2.0f);
        }

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
