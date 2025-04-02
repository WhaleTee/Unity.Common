using UnityEngine;
using UnityEngine.Events;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.UnityEventBinders
{
    public abstract class UnityEventBinder<T> : ObservableBinder<T>
    {
        [SerializeField] private UnityEvent<T> _event;
        
        protected override void OnPropertyChanged(T newValue)
        {
            _event.Invoke(newValue);
        }
    }
}
