using UnityEngine;
using UnityEngine.Events;
using WhaleTee.Reactive.Runtime.MVVM.Binders.Utils;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.UnityEventBinders
{
    public class IntToBoolUnityEventBinder : ObservableBinder<int>
    {
        [SerializeField] private CompareOperation _compareOperation;
        [SerializeField] private int _comparingValue;
        
        [SerializeField] private UnityEvent<bool> _event;
		
        protected override void OnPropertyChanged(int newValue)
        {
            var result = _compareOperation.Compare(newValue, _comparingValue);

            _event.Invoke(result);
        }
    }
}