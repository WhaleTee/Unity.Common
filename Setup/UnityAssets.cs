using System;
using System.IO;
using UnityEditor;

namespace WhaleTee.Setup {
  public static class UnityAssets {
    public static void ImportAsset(string asset, string folder) {
      string basePath;

      if (Environment.OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix) {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        basePath = Path.Combine(homeDirectory, "Library/Unity/Asset Store-5.x");
      } else {
        var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Unity");
        basePath = Path.Combine(EditorPrefs.GetString("AssetStoreCacheRootPath", defaultPath), "Asset Store-5.x");
      }

      asset = asset.EndsWith(".unitypackage") ? asset : asset + ".unitypackage";

      var fullPath = Path.Combine(basePath, folder, asset);

      if (!File.Exists(fullPath)) {
        throw new FileNotFoundException($"The asset package was not found at the path: {fullPath}");
      }

      AssetDatabase.ImportPackage(fullPath, false);
    }
  }
}