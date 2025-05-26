using System;
using System.Collections.Generic;
using System.Linq;

namespace WhaleTee.Runtime.StateMachine {
  // Core class for managing states and transitions
  public class StateMachine {
    // The currently active state node
    private StateNode current;
    // Maps state types to their corresponding nodes
    private readonly Dictionary<Type, StateNode> nodes = new();
    // Transitions that can be triggered from any state
    private readonly HashSet<Transition> anyTransitions = new();

    // Called every frame to update the current state and handle transitions
    public void Update() {
      var transition = GetTransition();

      if (transition != null) ChangeState(transition.To);

      current.State?.Update();
    }

    // Called every physics frame to update the current state
    public void FixedUpdate() {
      current.State?.FixedUpdate();
    }

    // Sets the initial or current state
    public void SetState(State state) {
      current = nodes[state.GetType()];
      current.State?.OnEnter();
    }

    // Handles state changes, calling exit/enter hooks as needed
    private void ChangeState(State state) {
      if (state == current.State) return;

      var previousState = current.State;
      var nextState = nodes[state.GetType()].State;

      previousState?.OnExit();
      nextState?.OnEnter();
      current = nodes[state.GetType()];
    }

    // Finds a valid transition, prioritizing anyTransitions
    private Transition GetTransition() {
      foreach (var transition in anyTransitions.Where(transition => transition.Condition.Evaluate())) return transition;

      return current.Transitions.FirstOrDefault(transition => transition.Condition.Evaluate());
    }

    // Adds a transition from one state to another with a condition
    public void AddTransition(State from, State to, Predicate condition) {
      GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    // Adds a transition that can be triggered from any state
    public void AddAnyTransition(State to, Predicate condition) {
      anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    // Retrieves an existing node or creates a new one for a state
    private StateNode GetOrAddNode(State state) {
      var node = nodes.GetValueOrDefault(state.GetType());

      if (node == null) {
        node = new StateNode(state);
        nodes.Add(state.GetType(), node);
      }

      return node;
    }

    // Internal class representing a state and its outgoing transitions
    private class StateNode {
      public State State { get; }
      public HashSet<Transition> Transitions { get; }

      public StateNode(State state) {
        this.State = state;
        Transitions = new HashSet<Transition>();
      }

      // Adds a transition to another state with a condition
      public void AddTransition(State to, Predicate condition) {
        Transitions.Add(new Transition(to, condition));
      }
    }
  }
}