using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JicsawPuzzle
{
    public class ChoiceUIController : BaseUIController
    {
        public ChoiceElementList ChoiceElementList;

        protected override void Start() {
            if (ChoiceElementList == null)
            {
                ChoiceElementList = GetComponentInChildren<ChoiceElementList>();
            }

            IsInitialized = true;
        }

        public override IEnumerator Play()
        {
            SetActive(true);
            yield return ChoiceSequence0();
            yield return new WaitUntil(()=> gameObject.activeSelf == false);
        }

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void CallSequence(IEnumerator sequenceIE)
        {
            if (sequenceIE != null)
            {
                StartCoroutine(sequenceIE);
            }
        }

        public void GenerateChoice(string text, UnityAction action)
        {
            var choiceElement = ChoiceElementList.GenerateElement();
            choiceElement.SetButton(text, action);
        }

        public void CrearChoice()
        {
            ChoiceElementList.CrearElements();
        }

        #region Choice Sequence
        public IEnumerator ChoiceSequence0()
        {
            CrearChoice();
            GenerateChoice("응", ()=>CallSequence(ChoiceSequence2()));
            GenerateChoice("아니", ()=>CallSequence(ChoiceSequence1()));
            yield return null;
        }

        public IEnumerator ChoiceSequence1()
        {
            CrearChoice();
            GenerateChoice("도착했어", ()=>CallSequence(ChoiceSequence2()));
            yield return null;
        }

        public IEnumerator ChoiceSequence2()
        {
            SetActive(false);
            yield return null;
        }

        #endregion
    }
}

