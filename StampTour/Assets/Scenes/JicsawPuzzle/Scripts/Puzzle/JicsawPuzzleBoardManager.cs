using System.Collections;
using UnityEngine;

namespace JicsawPuzzle
{
    public class JicsawPuzzleBoardManager : BaseInitializeObject
    {
        public GameObject Board;

        protected override void Start() {
            IsInitialized = true;
        }

        public override IEnumerator Play()
        {
            yield return null;
        }

        public void BoardGenerate()
        {
            Board.GetComponent<BoardGen>().Generate(this);
            JicsawPuzzleManager.Instance.PuzzleStart();
        }

        public void PuzzleClear()
        {
            JicsawPuzzleManager.Instance.PuzzleClear();
        }
    }
}
