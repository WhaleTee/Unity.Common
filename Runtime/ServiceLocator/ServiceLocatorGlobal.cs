using UnityEngine;

namespace WhaleTee.Runtime.ServiceLocator {
  [AddComponentMenu("ServiceLocator/ServiceLocator Global")]
  public class ServiceLocatorGlobal : Bootstrapper {
    [SerializeField] private bool destroyOnLoad;

    protected override void Bootstrap() {
      Container.ConfigureAsGlobal(destroyOnLoad);
    }
  }
}