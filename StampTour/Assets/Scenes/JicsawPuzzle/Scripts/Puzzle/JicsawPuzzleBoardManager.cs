using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class JicsawPuzzleBoardManager : BaseInitializeObject
    {
        public BoardGen Board;
        public GameObject PuzzleSuccedUI;
        public Sprite puzzleSprite;
        public CanvasGroup WorldCanvasGroup;
        public GameObject Blocker;

        [SerializeField]
        private string m_boardName;
        private bool isPlay = false;

        protected override void Start() {
            gameObject.SetActive(false);
            Board.GetComponent<Image>().sprite = puzzleSprite;
            m_boardName = puzzleSprite.name;
            Board.SetImageFilePath("JicsawPuzzle/" + m_boardName);
            Blocker.SetActive(false);

            IsInitialized = true;
        }

        public override IEnumerator Play()
        {
            isPlay = true;
            gameObject.SetActive(true);
            yield return null;
        }

        [ContextMenu("BoardGenerate")]
        public void BoardGenerate()
        {
            Board.Generate(this);
            JicsawPuzzleManager.Instance.PuzzleStart();
        }

        public void PuzzleClear()
        {
            PuzzleSuccedUI.GetComponentInChildren<TMP_Text>().text = "성공";
            PuzzleSuccedUI.GetComponent<Button>().enabled=false;
            PuzzleSuccedUI.SetActive(true);
            JicsawPuzzleManager.Instance.PuzzleClear();
            isPlay = false;
        }

        public void PausePuzzlePlay(bool isPause)
        {
            if (isPause)
            {
                // WorldCanvasGroup.alpha = 0.3f;
                // WorldCanvasGroup.interactable = false;
                Blocker.SetActive(true);
            }
            else
            {
                // WorldCanvasGroup.alpha = 1.0f;
                // WorldCanvasGroup.interactable = true;
                Blocker.SetActive(false);
            }
        }

        public bool CheckBoardName(string inputName)
        {
            // return m_boardName == inputName;
            return true;
        }

        public bool IsPlaying()
        {
            return isPlay;
        }
    }
}
