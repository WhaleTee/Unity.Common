using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WhaleTee.Runtime.Assembly;

namespace WhaleTee.Runtime.EventBus {
  /// <summary>
  /// Contains methods and properties related to event buses and event types in the Unity application.
  /// </summary>
  public static class EventBusUtil {
    private static IReadOnlyList<Type> eventTypes { get; set; }
    private static IReadOnlyList<Type> eventBusTypes { get; set; }

    #if UNITY_EDITOR

    /// <summary>
    /// Initializes the Unity Editor related components of the EventBusUtil.
    /// The [InitializeOnLoadMethod] attribute causes this method to be called every time a script
    /// is loaded or when the game enters Play Mode in the Editor. This is useful to initialize
    /// fields or states of the class that are necessary during the editing state that also apply
    /// when the game enters Play Mode.
    /// The method sets up a subscriber to the playModeStateChanged event to allow
    /// actions to be performed when the Editor's play mode changes.
    /// </summary>    
    [InitializeOnLoadMethod]
    public static void InitializeEditor() {
      EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
      EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state) {
      if (state == PlayModeStateChange.ExitingPlayMode) {
        ClearAllBuses();
      }
    }
    #endif

    /// <summary>
    /// Initializes the EventBusUtil class at runtime before the loading of any scene.
    /// The [RuntimeInitializeOnLoadMethod] attribute instructs Unity to execute this method after
    /// the game has been loaded but before any scene has been loaded, in both Play Mode and after
    /// a Build is run. This guarantees that necessary initialization of bus-related types and events is
    /// done before any game objects, scripts or components have started.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize() {
      eventTypes = PredefinedAssemblyTypeFinder.GetTypes(typeof(Event));
      eventBusTypes = InitializeAllBuses();
    }

    private static List<Type> InitializeAllBuses() {
      var busTypes = new List<Type>();
      var typedef = typeof(EventBus<>);

      foreach (var eventType in eventTypes) {
        var busType = typedef.MakeGenericType(eventType);
        busTypes.Add(busType);
        Debug.Log($"Initialized EventBus<{eventType.Name}>");
      }

      return busTypes;
    }

    /// <summary>
    /// Clears (removes all listeners from) all event buses in the application.
    /// </summary>
    private static void ClearAllBuses() {
      Debug.Log("Clearing all buses...");

      foreach (var busType in eventBusTypes) {
        var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
        clearMethod?.Invoke(null, null);
      }
    }
  }
}