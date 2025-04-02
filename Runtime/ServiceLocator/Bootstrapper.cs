using UnityEngine;
using WhaleTee.Runtime.Extensions;

namespace WhaleTee.Runtime.ServiceLocator {
  [DisallowMultipleComponent]
  [RequireComponent(typeof(ServiceLocator))]
  public abstract class Bootstrapper : MonoBehaviour {
    private ServiceLocator locator;
    private bool bootstrapped;
    
    internal ServiceLocator container => locator.OrNull() ?? (locator = GetComponent<ServiceLocator>());

    private void Awake() => BootstrapOnDemand();

    public void BootstrapOnDemand() {
      if (bootstrapped) return;

      bootstrapped = true;
      Bootstrap();
    }

    protected abstract void Bootstrap();
  }
}