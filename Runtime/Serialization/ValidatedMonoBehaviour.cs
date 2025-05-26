using UnityEngine;

namespace WhaleTee.Runtime.Serialization {
  public class ValidatedMonoBehaviour : MonoBehaviour {
    #if UNITY_EDITOR
    protected virtual void OnValidate() {
      this.ValidateRefs();
    }
    #else
        protected virtual void OnValidate() { }
    #endif
  }
}