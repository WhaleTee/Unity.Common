using UnityEngine;
using UnityEngine.Events;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.UnityEventBinders
{
    public class BoolToSpriteUnityEventBinder : ObservableBinder<bool>
    {
        [SerializeField] private Sprite _spriteTrue;
        [SerializeField] private Sprite _spriteFalse;

        [SerializeField] private UnityEvent<Sprite> _event;

        protected override void OnPropertyChanged(bool newValue)
        {
            var sprite = newValue ? _spriteTrue : _spriteFalse;
            
            _event.Invoke(sprite);
        }
    }
}