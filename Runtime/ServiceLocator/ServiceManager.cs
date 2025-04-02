using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WhaleTee.Runtime.ServiceLocator {
  public class ServiceManager {
    private readonly HashSet<Service> services = new HashSet<Service>();
    public IEnumerable<object> RegisteredServices => services.Select(s => s.Instance);

    public bool TryGet<T>(out T service, string name = "") where T : class {
      var type = typeof(T);

      if (services.TryGetValue(new Service(type, name: name), out var obj)) {
        service = (T) obj.Instance;
        return true;
      }

      service = null;
      return false;
    }

    public T Get<T>() where T : class {
      var type = typeof(T);

      if (services.TryGetValue(new Service(type), out var obj)) {
        return (T) obj.Instance;
      }

      throw new ArgumentException($"ServiceManager.Get: Service of type {type.FullName} not registered");
    }

    public T Get<T>(string name) where T : class {
      var type = typeof(T);

      if (services.TryGetValue(new Service(type, name: name), out var obj)) {
        return (T) obj.Instance;
      }

      throw new ArgumentException($"ServiceManager.Get: Service of type {type.FullName} not registered");
    }

    public ServiceManager Register<T>(T service) {
      var type = typeof(T);

      if (!services.Add(new Service(type, service))) {
        Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} already registered");
      }

      return this;
    }

    public ServiceManager Register<T>(T service, string name) {
      var type = typeof(T);

      if (!services.Add(new Service(type, service, name))) {
        Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} already registered");
      }

      return this;
    }

    public ServiceManager Register(Type type, object service) {
      if (!type.IsInstanceOfType(service)) {
        throw new ArgumentException("Type of service does not match type of service interface", nameof(service));
      }

      if (!services.Add(new Service(type, service))) {
        Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} already registered");
      }

      return this;
    }

    public ServiceManager Register(Type type, object service, string name) {
      if (!type.IsInstanceOfType(service)) {
        throw new ArgumentException("Type of service does not match type of service interface", nameof(service));
      }

      if (!services.Add(new Service(type, service, name))) {
        Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} already registered");
      }

      return this;
    }
  }
}