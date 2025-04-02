using System;
using ObservableCollections;
using R3;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders
{
    public abstract class ObservableCollectionBinder : Binder
    {
        public abstract Type ArgumentType { get; }
    }

    public abstract class ObservableCollectionBinder<T> : ObservableCollectionBinder where T : IViewModel
    {
        public override Type ArgumentType => typeof(T);

        protected sealed override IDisposable BindInternal(IViewModel viewModel)
        {
            var propertyInfo = viewModel.GetType().GetProperty(PropertyName);
            var reactiveCollection = (IObservableCollection<T>)propertyInfo?.GetValue(viewModel);
            var subscriptions = Disposable.CreateBuilder();
            reactiveCollection?.ObserveAdd().Subscribe(OnItemAdded).AddTo(ref subscriptions);
            reactiveCollection?.ObserveRemove().Subscribe(OnItemRemoved).AddTo(ref subscriptions);
            return subscriptions.Build();
        }
        
        protected abstract void OnItemAdded(CollectionAddEvent<T> ctx);
        protected abstract void OnItemRemoved(CollectionRemoveEvent<T> ctx);
    }
}