using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class MarkerUIComponent : MonoBehaviour
    {
        // Preview
        public Image PreviewImage;
        public TMP_Text PreviewGuidText;

        // Goal
        public Image GoalImage;
        public TMP_Text GoalText;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetActivePreviewPanel(bool isVisible)
        {
            PreviewImage.gameObject.SetActive(isVisible);
            PreviewGuidText.gameObject.SetActive(isVisible);
        }

        public void SetActiveGoalPanel(bool isActive)
        {
            GoalImage.gameObject.SetActive(isActive);
            GoalText.gameObject.SetActive(isActive);
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