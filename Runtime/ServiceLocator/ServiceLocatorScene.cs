using UnityEngine;

namespace WhaleTee.Runtime.ServiceLocator {
  [AddComponentMenu("ServiceLocator/ServiceLocator Scene")]
  public class ServiceLocatorScene : Bootstrapper {
    protected override void Bootstrap() {
      container.ConfigureForScene();
    }
  }
}