#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace WhaleTee.Runtime.Serialization {
  [InitializeOnLoad]
  public static class SceneRefValidatorOnSave {
    private const string PREFS_KEY = "KBCore/ValidateRefsOnSave";
    private const string MENU_ITEM_TEXT = "Tools/KBCore/Validate Refs on Save";

    public static bool validateRefsOnSave {
      get => EditorPrefs.GetBool(PREFS_KEY, false);
      private set => EditorPrefs.SetBool(PREFS_KEY, value);
    }

    static SceneRefValidatorOnSave() {
      EditorSceneManager.sceneSaving += OnSceneSaving;
    }

    [MenuItem(MENU_ITEM_TEXT, false, 1000)]
    public static void ToggleValidateRefsOnSave() {
      validateRefsOnSave = !validateRefsOnSave;
      Menu.SetChecked(MENU_ITEM_TEXT, validateRefsOnSave);
    }

    [MenuItem(MENU_ITEM_TEXT, true)]
    public static bool ToggleValidateRefsOnSaveValidate() {
      Menu.SetChecked(MENU_ITEM_TEXT, validateRefsOnSave);

      return true;
    }

    private static void OnSceneSaving(Scene scene, string path) {
      if (!validateRefsOnSave) return;

      SceneRefAttributeValidator.ValidateAllRefs();
    }
  }
}

#endif