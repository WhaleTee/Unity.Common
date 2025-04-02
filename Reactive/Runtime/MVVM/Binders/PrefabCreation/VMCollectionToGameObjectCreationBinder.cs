using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.PrefabCreation
{
    public class VMCollectionToGameObjectCreationBinder : ObservableCollectionBinder<IViewModel>
    {
        [SerializeField] private View _prefabView;

        private readonly Dictionary<IViewModel, View> _createdViews = new();
        
        protected override void OnItemAdded(CollectionAddEvent<IViewModel> ctx)
        {
            if (_createdViews.ContainsKey(ctx.Value))
            {
                return;
            }

            var createdView = Instantiate(_prefabView, transform);
            
            _createdViews.Add(ctx.Value, createdView);
            createdView.Bind(ctx.Value);
        }

        protected override void OnItemRemoved(CollectionRemoveEvent<IViewModel> ctx)
        {
            if (_createdViews.TryGetValue(ctx.Value, out var view))
            {
                view.Destroy();
                _createdViews.Remove(ctx.Value);
            }
        }
    }
}