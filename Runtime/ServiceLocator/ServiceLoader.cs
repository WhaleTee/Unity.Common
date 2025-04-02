using System;
using UnityEngine;
using WhaleTee.Runtime.Assembly;

namespace WhaleTee.Runtime.ServiceLocator {
  public static class ServiceRuntimeUtils {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize() {
      PredefinedAssemblyTypeFinder.GetTypesWithAttributes(typeof(ServiceAttribute))
      .ForEach(
        type => {
          var service = Activator.CreateInstance(type);
          ServiceLocator.Global.Register(type, service);
        }
      );
    }
  }
}