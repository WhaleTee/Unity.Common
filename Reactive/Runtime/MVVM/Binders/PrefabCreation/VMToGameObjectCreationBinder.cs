using UnityEngine;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.PrefabCreation
{
    public class VMToGameObjectCreationBinder : ObservableBinder<IViewModel>
    {
        [SerializeField] private View _prefabView;
        
        protected override void OnPropertyChanged(IViewModel newValue)
        {
            var createdView = Instantiate(_prefabView, transform);
                
            createdView.Bind(newValue);
        }
    }
}