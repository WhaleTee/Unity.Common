using UnityEngine;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.PrefabCreation
{
    public class VMToGameObjectCreationFromListBinder : ObservableBinder<IViewModel>
    {
        [SerializeField] private ViewModelToViewMapper _mapper;

        private View _createdView;

        protected override void OnPropertyChanged(IViewModel newValue)
        {
            var prefabView = _mapper.GetPrefab(newValue.GetType().FullName);
            _createdView = Instantiate(prefabView, transform);
                
            _createdView.Bind(newValue);
        }
    }
}