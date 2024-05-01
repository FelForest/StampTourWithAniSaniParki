using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JicsawPuzzle
{
    public abstract class BaseInitializeObject : MonoBehaviour, IPlay
    {
        protected bool _isInitalized = false;
        public virtual bool IsInitialized { get {return _isInitalized;} protected set { _isInitalized = value;}}

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

        protected virtual IEnumerator ActiveObject(BaseInitializeObject targetObj)
        {
            targetObj.gameObject.SetActive(true);
            yield return new WaitUntil(()=> targetObj.IsInitialized);
            targetObj.gameObject.SetActive(false);
        }
    }
}

