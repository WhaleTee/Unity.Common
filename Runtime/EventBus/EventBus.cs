using System.Collections.Generic;
using UnityEngine;

namespace WhaleTee.Runtime.EventBus {
  public static class EventBus<T> where T : Event {
    private static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

    public static void Register(IEventBinding<T> binding) => bindings.Add(binding);
    public static void Deregister(IEventBinding<T> binding) => bindings.Remove(binding);

    public static void Raise(T @event) {
      foreach (var binding in bindings) {
        binding.onEvent.Invoke(@event);
        binding.onEventNoArgs.Invoke();
      }
    }

    private static void Clear() {
      Debug.Log($"Clearing {typeof(T).Name} bindings");
      bindings.Clear();
    }
  }
}