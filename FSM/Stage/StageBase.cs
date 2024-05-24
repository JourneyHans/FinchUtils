namespace HzFramework.FSM.Stage {
    public class StageBase : State<StageManager> {
        public void ChangeStage<T>() where T : StageBase {
            StageManager.Ins.ChangeStage<T>();
        }
    }
}