using UnityEngine;
using UnityEngine.Events;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.UnityEventBinders
{
    public class DoubleToTextUnityEventBinder : ObservableBinder<double>
    {
        [SerializeField] private string _format = "0.00";
        [SerializeField] private UnityEvent<string> _event;
        
        protected override void OnPropertyChanged(double newValue)
        {
            _event.Invoke(newValue.ToString(_format));
        }
    }
}