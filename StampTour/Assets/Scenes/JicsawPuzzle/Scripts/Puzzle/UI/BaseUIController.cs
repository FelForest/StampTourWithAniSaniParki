using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JicsawPuzzle
{
    public abstract class BaseUIController : MonoBehaviour, IPlay
    {
        protected bool _isInitalized = false;
        public bool IsInitialized { get {return _isInitalized;} protected set { _isInitalized = value;}}

        protected virtual void Start() {
            IsInitialized = true;
        }

        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public virtual IEnumerator Play()
        {
            throw new System.NotImplementedException();
        }
    }
}

