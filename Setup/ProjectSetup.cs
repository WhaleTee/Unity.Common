using UnityEditor;
using static UnityEditor.AssetDatabase;

namespace WhaleTee.Setup {
  public static class ProjectSetup {
    [MenuItem("Tools/Setup/Import Essential Assets")]
    public static void ImportEssentials() {
      UnityAssets.ImportAsset("Odin Inspector and Serializer.unitypackage", "Sirenix/Editor ExtensionsSystem");
      // UnityAssets.ImportAsset("Odin Validator.unitypackage", "Sirenix/Editor ExtensionsUtilities");
      UnityAssets.ImportAsset("DOTween Pro.unitypackage", "Demigiant/Editor ExtensionsVisual Scripting");
      UnityAssets.ImportAsset("Quantum Console.unitypackage", "QFSW/Editor ExtensionsUtilities");
      UnityAssets.ImportAsset("Hot Reload Edit Code Without Compiling.unitypackage", "The Naughty Cult/Editor ExtensionsUtilities");
      // UnityAssets.ImportAsset("Editor Console Pro.unitypackage", "FlyingWorm/Editor ExtensionsSystem");
    }
    
    [MenuItem("Tools/Setup/Import Pro Camera 2D")]
    public static void ImportProCamera2D() {
      UnityAssets.ImportAsset("Pro Camera 2D - The definitive 2D 25D camera plugin for Unity.unitypackage", "Lus Pedro Fonseca/2D");
    }
    
    [MenuItem("Tools/Setup/Import Flare Engine")]
    public static void ImportFlareEngine() {
      UnityAssets.ImportAsset("Flare Engine - 2D Tools.unitypackage", "Two Bit Machines/Complete ProjectsSystems");
    }

    [MenuItem("Tools/Setup/Install Essential Packages")]
    public static void InstallEssentialPackages() {
      UnityPackages.InstallPackages(
        new[] {
                "com.unity.ide.rider",
                "git+https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity",
                "git+https://github.com/WhaleTee/Unity.Common.git?path=/Runtime",
                "git+https://github.com/WhaleTee/Unity.Common.git?path=/Reactive/Setup",
                "com.unity.inputsystem"
              }
      );
    }

    [MenuItem("Tools/Setup/Install Essential 2D Packages")]
    public static void InstallEssential2DPackages() {
      UnityPackages.InstallPackages(
        new[] { "com.unity.feature.2d" }
      );
    }

    [MenuItem("Tools/Settings/Disable Domain Reload")]
    public static void DisableDomainReload() {
      EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload;
    }

    [MenuItem("Tools/Setup/Create Folders")]
    public static void CreateFolders() {
      UnityFolders.Create(
        "Project",
        "Animation",
        "Audio",
        "Fonts",
        "Sprites",
        "Art",
        "Art/Finals",
        "Art/Sketches",
        "Art/References",
        "Materials",
        "Models",
        "Music",
        "Prefabs",
        "Resources",
        "Shaders",
        "Terrain",
        "Text",
        "Scripts/Tests",
        "Scripts/Tests/Editor",
        "Scripts/Tests/Runtime"
      );

      Refresh();
      UnityFolders.Move("Project", "Scenes");
      UnityFolders.Move("Project", "Settings");
      UnityFolders.Delete("TutorialInfo");
      Refresh();

      MoveAsset("Assets/InputSystem_Actions.inputactions", "Assets/Project/Settings/InputSystem_Actions.inputactions");
      MoveAsset("Assets/DefaultVolumeProfile.asset", "Assets/Project/Settings/DefaultVolumeProfile.asset");
      MoveAsset("Assets/UniversalRenderPipelineGlobalSettings.asset", "Assets/Project/Settings/UniversalRenderPipelineGlobalSettings.asset");
      DeleteAsset("Assets/Readme.asset");
      Refresh();
    }
  }
}
