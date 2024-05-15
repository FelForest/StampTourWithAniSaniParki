using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class MarkerUIController : BaseInitializeObject
    {
        [Header("Preview")]
        public GameObject PreviewObject;
        public Image PreviewImage;
        public TMP_Text PreviewGuidText;

        [Header("Goal")]
        public GameObject GoalObject;
        public Image GoalImage;
        public TMP_Text GoalText;

        [Header("Failed")]
        public GameObject FailedObject;
        public Image FailedImage;

        protected override void Start() {
            FailedObject.SetActive(false);  // 실패 오브젝트 비활성화
            IsInitialized = true;
        }

        public override IEnumerator Play()
        {
            SetActive(true);
            SetActivePreviewPanel(true);
            yield return new WaitUntil(()=> !PreviewActiveSelf());
        }

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetActivePreviewPanel(bool isVisible)
        {
            PreviewObject.SetActive(isVisible);
        }
        public bool PreviewActiveSelf()
        {
            return PreviewObject.activeSelf;
        }

        public void SetActiveGoalPanel(bool isActive)
        {
            GoalObject.SetActive(isActive);
        }

        public void SetActiveFailedPanel(bool isActive)
        {
            FailedObject.SetActive(isActive);
        }

        public void ChangePreviewSprite(Sprite sprite)
        {
            ChangeSprite(PreviewImage, sprite);
        }

        public void ChangeGoalSprite(Sprite sprite)
        {
            ChangeSprite(GoalImage, sprite);
        }

        protected void ChangeSprite(Image targetImage, Sprite sprite)
        {
            targetImage.sprite = sprite;
        }
    }
}