namespace WhaleTee.Runtime.StateMachine {
  public class Transition {
    public State to { get; }
    public Predicate condition { get; }

    public Transition(State to, Predicate condition) {
      this.to = to;
      this.condition = condition;
    }
  }
}