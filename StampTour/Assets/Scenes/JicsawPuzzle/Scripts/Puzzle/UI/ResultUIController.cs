using System.Collections;

namespace JicsawPuzzle
{
    public class ResultUIController : BaseInitializeObject
    {
        public ResultInfoComponent ResultInfoComponent;
        public NextMissionButton NextMissionButton;

        public override IEnumerator Play()
        {
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

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
