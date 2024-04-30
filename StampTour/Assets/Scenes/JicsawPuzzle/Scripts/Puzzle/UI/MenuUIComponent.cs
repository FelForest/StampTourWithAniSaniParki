using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class MenuUIComponent : MonoBehaviour
    {
        public Button BackButton;

        private void Start() {
            if (BackButton != null)
            {
                BackButton.onClick.RemoveListener(ClickBackButton);
                BackButton.onClick.AddListener(ClickBackButton);
            }
        }
        public void ClickBackButton()
        {
        }
    }
}
