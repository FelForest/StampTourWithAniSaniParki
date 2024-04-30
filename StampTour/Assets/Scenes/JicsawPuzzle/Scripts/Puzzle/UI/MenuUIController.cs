using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class MenuUIController : BaseUIController
    {
        public Button BackButton;

        protected override void Start() {
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
