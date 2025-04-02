using System.IO;
using UnityEditor;
using UnityEngine;

namespace WhaleTee.Setup {
  public static class UnityFolders {
    public static void Create(string root, params string[] folders) {
      var fullPath = Path.Combine(Application.dataPath, root);

      if (!Directory.Exists(fullPath)) {
        Directory.CreateDirectory(fullPath);
      }

      foreach (var folder in folders) {
        CreateSubFolders(fullPath, folder);
      }
    }

    private static void CreateSubFolders(string rootPath, string folderHierarchy) {
      var folders = folderHierarchy.Split('/');
      var currentPath = rootPath;

      foreach (var folder in folders) {
        currentPath = Path.Combine(currentPath, folder);

        if (!Directory.Exists(currentPath)) {
          Directory.CreateDirectory(currentPath);
        }
      }
    }

    public static void Move(string newParent, string folderName) {
      var sourcePath = $"Assets/{folderName}";

      if (AssetDatabase.IsValidFolder(sourcePath)) {
        var destinationPath = $"Assets/{newParent}/{folderName}";
        var error = AssetDatabase.MoveAsset(sourcePath, destinationPath);

        if (!string.IsNullOrEmpty(error)) {
          Debug.LogError($"Failed to move {folderName}: {error}");
        }
      }
    }

    public static void Delete(string folderName) {
      var pathToDelete = $"Assets/{folderName}";

      if (AssetDatabase.IsValidFolder(pathToDelete)) {
        AssetDatabase.DeleteAsset(pathToDelete);
      }
    }
  }
}