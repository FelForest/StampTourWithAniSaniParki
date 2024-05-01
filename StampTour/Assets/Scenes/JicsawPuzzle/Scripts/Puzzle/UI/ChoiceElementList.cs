using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class ChoiceElementList : MonoBehaviour
    {
        [SerializeField]
        protected GameObject prefab_ChoiceElement;
        
        public readonly List<ChoiceElement> Elements = new List<ChoiceElement>();
        
        protected int _count;
        public int Count { get { return _count; } set { _count = value; } }

        protected RectTransform m_RectTransform;
        protected VerticalLayoutGroup m_VerticalLayoutGroup;

        private void Start() {
            m_RectTransform = GetComponent<RectTransform>();
        }

        public ChoiceElement GenerateElement()
        {
            try
            {
                if (Count >= Elements.Count)
                {
                    GameObject elementObj = Instantiate(prefab_ChoiceElement, transform);
                    elementObj.TryGetComponent(out ChoiceElement choiceElement);
                    choiceElement.Init();
                    Elements.Add(choiceElement);
                }
                else
                {
                    Elements[Count].gameObject.SetActive(true);
                }

                Count++;

                float height = Count * Elements[Count - 1].RectTransform.rect.height;

                m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return Elements[Count - 1];
        }

        public void CrearElements(bool isDeep = false)
        {
            if (isDeep)
            {
                Elements.Clear();
            }

            foreach (ChoiceElement element in Elements)
            {
                element.ResetButton();
                element.gameObject.SetActive(false);
            }

            Count = 0;
        }
    }
}