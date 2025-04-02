using System;

namespace WhaleTee.Runtime.EventBus {
  public interface IEventBinding<T> {
    public Action<T> onEvent { get; set; }
    public Action onEventNoArgs { get; set; }
  }

  public class EventBinding<T> : IEventBinding<T> where T : Event {
    private Action<T> onEvent = _ => { };
    private Action onEventNoArgs = () => { };

    Action<T> IEventBinding<T>.onEvent {
      get => onEvent;
      set => onEvent = value;
    }

    Action IEventBinding<T>.onEventNoArgs {
      get => onEventNoArgs;
      set => onEventNoArgs = value;
    }

    public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
    public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

    public void Add(Action onEvent) => onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

    public void Add(Action<T> onEvent) => this.onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
  }
}