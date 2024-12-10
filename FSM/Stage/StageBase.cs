namespace FinchUtils.FSM.Stage {
    public class StageBase : State<StageManager> {
        public void ChangeStage<T>() where T : StageBase {
            StageManager.Instance.ChangeStage<T>();
        }
    }
}