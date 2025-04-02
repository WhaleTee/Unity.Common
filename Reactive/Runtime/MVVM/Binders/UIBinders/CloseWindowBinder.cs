using R3;
using UnityEngine;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.UIBinders
{
    public class CloseWindowBinder : ObservableBinder<Unit>
    {
        [SerializeField] private GameObject _destroyingGameObject;
        
        protected override void OnPropertyChanged(Unit newValue)
        {
            Destroy(_destroyingGameObject);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            if (!_destroyingGameObject)
            {
                _destroyingGameObject = gameObject;
            }
        }
#endif
    }
}
