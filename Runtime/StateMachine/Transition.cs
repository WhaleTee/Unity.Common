namespace WhaleTee.Runtime.StateMachine {
  public class Transition {
    public State To { get; }
    public Predicate Condition { get; }

    public Transition(State to, Predicate condition) {
      this.To = to;
      this.Condition = condition;
    }
  }
}