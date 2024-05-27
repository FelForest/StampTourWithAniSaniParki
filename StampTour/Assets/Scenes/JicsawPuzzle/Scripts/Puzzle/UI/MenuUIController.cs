using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class MenuUIController : BaseInitializeObject
    {
        public string MainSceneName = "MainScene";
        public Button BackButton;

        protected override void Start() 
        {
            if (BackButton != null)
            {
                BackButton.onClick.RemoveListener(ClickBackButton);
                BackButton.onClick.AddListener(ClickBackButton);
            }
        }

        public void ClickBackButton()
        {
            GameManager.LoadScene(MainSceneName);
        }
    }
}
