namespace WhaleTee.Runtime.StateMachine {
  public interface State {
    void OnEnter();
    void Update();
    void FixedUpdate();
    void OnExit();
  }
}