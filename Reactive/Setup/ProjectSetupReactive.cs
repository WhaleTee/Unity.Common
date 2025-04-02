using NugetForUnity;
using NugetForUnity.Models;
using UnityEditor;
using WhaleTee.Setup;

namespace WhaleTee.Reactive.Setup {
  public static class ProjectSetupReactive {
    [MenuItem("Tools/Setup/Install Reactive Packages")]
    public static void InstallEssentialPackages() {
      if (NugetPackageInstaller.InstallIdentifier(new NugetPackageIdentifier("R3", null))) {
        UnityPackages.InstallPackages(new[] { "git+https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity" });
      }
      NugetPackageInstaller.InstallIdentifier(new NugetPackageIdentifier("ObservableCollections", null));
      NugetPackageInstaller.InstallIdentifier(new NugetPackageIdentifier("ObservableCollections.R3", null));
      UnityPackages.InstallPackages(
        new[] {
          "git+https://github.com/WhaleTee/Unity.Common.git?path=/Tools",
          "git+https://github.com/WhaleTee/Unity.Common.git?path=/Reactive/Runtime",
        }
      );
    }
  }
}