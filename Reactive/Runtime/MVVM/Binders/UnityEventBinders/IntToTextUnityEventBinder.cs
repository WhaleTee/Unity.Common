using UnityEngine;
using UnityEngine.Events;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.UnityEventBinders
{
    public class IntToTextUnityEventBinder : ObservableBinder<int>
    {
        [SerializeField] private UnityEvent<string> _event;
        
        protected override void OnPropertyChanged(int newValue)
        {
            _event.Invoke(newValue.ToString());
        }
    }
}