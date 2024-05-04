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

        protected override void Start() {
            gameObject.SetActive(false);
            Board.GetComponent<Image>().sprite = puzzleSprite;
            Board.SetImageFilePath("JicsawPuzzle/" + puzzleSprite.name);
            IsInitialized = true;
        }

        public override IEnumerator Play()
        {
            gameObject.SetActive(true);
            yield return null;
        }

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
        }
    }
}
