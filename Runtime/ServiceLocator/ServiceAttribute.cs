using System;

namespace WhaleTee.Runtime.ServiceLocator {
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class ServiceAttribute : Attribute {}
}