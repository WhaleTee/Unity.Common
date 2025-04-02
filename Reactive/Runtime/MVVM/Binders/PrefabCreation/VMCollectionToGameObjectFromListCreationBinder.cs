using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.PrefabCreation {
  public class VMCollectionToGameObjectFromListCreationBinder : ObservableCollectionBinder<IViewModel> {
    [SerializeField] private ViewModelToViewMapper _mapper;

    private readonly Dictionary<IViewModel, View> _createdViews = new();

    protected override void OnItemAdded(CollectionAddEvent<IViewModel> ctx) {
      if (_createdViews.ContainsKey(ctx.Value)) {
        return;
      }

      var prefab = _mapper.GetPrefab(ctx.GetType().FullName);
      var createdView = Instantiate(prefab, transform);

      _createdViews.Add(ctx.Value, createdView);
      createdView.Bind(ctx.Value);
    }

    protected override void OnItemRemoved(CollectionRemoveEvent<IViewModel> ctx) {
      if (_createdViews.TryGetValue(ctx.Value, out var view)) {
        view.Destroy();
        _createdViews.Remove(ctx.Value);
      }
    }
  }
}