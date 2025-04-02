using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace WhaleTee.Setup {
  public static class UnityPackages {
    private static AddRequest request;
    private static readonly Queue<string> packagesToInstall = new Queue<string>();

    public static void InstallPackages(IEnumerable<string> packages) {
      foreach (var package in packages) {
        packagesToInstall.Enqueue(package);
      }

      if (packagesToInstall.Count > 0) {
        StartNextPackageInstallation();
      }
    }

    private static async void StartNextPackageInstallation() {
      request = Client.Add(packagesToInstall.Dequeue());

      while (!request.IsCompleted) await Task.Delay(10);

      switch (request.Status) {
        case StatusCode.Success: Debug.Log("Installed: " + request.Result.packageId); break;
        case >= StatusCode.Failure: Debug.LogError(request.Error.message); break;
      }

      if (packagesToInstall.Count > 0) {
        await Task.Delay(1000);
        StartNextPackageInstallation();
      }
    }
  }
}