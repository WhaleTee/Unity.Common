#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WhaleTee.Runtime.Serialization {
  /// <summary>
  /// Custom property drawer for the reference attributes, making them read-only.
  /// Note: Does not apply to the Anywhere attribute as that needs to remain editable. 
  /// </summary>
  [CustomPropertyDrawer(typeof(SelfAttribute))]
  [CustomPropertyDrawer(typeof(ChildAttribute))]
  [CustomPropertyDrawer(typeof(ParentAttribute))]
  [CustomPropertyDrawer(typeof(SceneAttribute))]
  public class SceneRefAttributePropertyDrawer : PropertyDrawer {
    private bool isInitialized;
    private bool canValidateType;
    private Type elementType;
    private string typeName;

    private SceneRefAttribute SceneRefAttribute => (SceneRefAttribute)attribute;
    private bool Editable => SceneRefAttribute.HasFlags(Flag.Editable);

// unity 2022.2 makes UIToolkit the default for inspectors
    #if UNITY_2022_2_OR_NEWER
    private const string SCENE_REF_CLASS = "kbcore-refs-sceneref";

    private PropertyField propertyField;
    private HelpBox helpBox;
    private InspectorElement inspectorElement;
    private SerializedProperty serializedProperty;

    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
      serializedProperty = property;
      Initialize(property);

      VisualElement root = new();
      root.AddToClassList(SCENE_REF_CLASS);

      helpBox = new HelpBox("", HelpBoxMessageType.Error) { style = { display = DisplayStyle.None } };
      root.Add(helpBox);

      propertyField = new PropertyField(property);
      propertyField.SetEnabled(Editable);
      root.Add(propertyField);

      if (canValidateType) {
        UpdateHelpBox();
        propertyField.RegisterCallback<AttachToPanelEvent>(OnAttach);
      }

      return root;
    }

    private void OnAttach(AttachToPanelEvent attachToPanelEvent) {
      propertyField.UnregisterCallback<AttachToPanelEvent>(OnAttach);
      inspectorElement = propertyField.GetFirstAncestorOfType<InspectorElement>();

      if (inspectorElement == null)
      // not in an inspector, invalid
        return;

      // subscribe to SerializedPropertyChangeEvent so we can update when the property changes
      inspectorElement.RegisterCallback<SerializedPropertyChangeEvent>(OnSerializedPropertyChangeEvent);
      propertyField.RegisterCallback<DetachFromPanelEvent>(OnDetach);
    }

    private void OnDetach(DetachFromPanelEvent detachFromPanelEvent) {
      // unregister from all callbacks
      propertyField.UnregisterCallback<DetachFromPanelEvent>(OnDetach);
      inspectorElement.UnregisterCallback<SerializedPropertyChangeEvent>(OnSerializedPropertyChangeEvent);
      serializedProperty = null;
    }

    private void OnSerializedPropertyChangeEvent(SerializedPropertyChangeEvent changeEvent) {
      if (changeEvent.changedProperty != serializedProperty)
        return;

      UpdateHelpBox();
    }

    private void UpdateHelpBox() {
      var isSatisfied = IsSatisfied(serializedProperty);
      helpBox.style.display = isSatisfied ? DisplayStyle.None : DisplayStyle.Flex;
      var message = $"Missing {serializedProperty.propertyPath} ({typeName}) reference on {SceneRefAttribute.Loc}!";
      helpBox.text = message;
    }
    #endif

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      if (!isInitialized)
        Initialize(property);

      if (!IsSatisfied(property)) {
        var helpBoxPos = position;
        helpBoxPos.height = EditorGUIUtility.singleLineHeight * 2;
        var message = $"Missing {property.propertyPath} ({typeName}) reference on {SceneRefAttribute.Loc}!";
        EditorGUI.HelpBox(helpBoxPos, message, MessageType.Error);
        position.height = EditorGUI.GetPropertyHeight(property, label);
        position.y += helpBoxPos.height;
      }

      var wasEnabled = GUI.enabled;
      GUI.enabled = Editable;
      EditorGUI.PropertyField(position, property, label, true);
      GUI.enabled = wasEnabled;
    }

    private void Initialize(SerializedProperty property) {
      isInitialized = true;

      // the type won't change, so we only need to initialize these values once
      elementType = fieldInfo.FieldType;

      if (typeof(SerializableRef).IsAssignableFrom(elementType)) {
        var interfaceType = elementType.GetInterfaces()
        .FirstOrDefault(type =>
                        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SerializableRef<>)
        );

        if (interfaceType != null)
          elementType = interfaceType.GetGenericArguments()[0];
      }

      canValidateType = typeof(Component).IsAssignableFrom(elementType) && property.propertyType == SerializedPropertyType.ObjectReference;

      typeName = fieldInfo.FieldType.Name;

      if (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GenericTypeArguments.Length >= 1)
        typeName = typeName.Replace("`1", $"<{fieldInfo.FieldType.GenericTypeArguments[0].Name}>");
    }

    /// <summary>Is this field Satisfied with a value or optional</summary>
    private bool IsSatisfied(SerializedProperty property) {
      if (!canValidateType || SceneRefAttribute.HasFlags(Flag.Optional))
        return true;

      return property.objectReferenceValue != null;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      float helpBoxHeight = 0;

      if (!IsSatisfied(property))
        helpBoxHeight = EditorGUIUtility.singleLineHeight * 2;

      return EditorGUI.GetPropertyHeight(property, label) + helpBoxHeight;
    }
  }
}
#endif