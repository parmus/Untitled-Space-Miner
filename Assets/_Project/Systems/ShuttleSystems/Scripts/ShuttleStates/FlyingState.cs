namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class FlyingState : ShuttleStateMachine.State {
        public FlyingState(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) : base(shuttleStateMachine, shuttle) {}

        public override void Enter() {
            _shuttle.Thrusters.enabled = true;
            _shuttle.CameraControl.enabled = true;
            _shuttle.MiningTool.enabled = true;
        }

        public override void Tick() {
            if (_shuttle.PowerSystem.IsEmpty) _shuttleStateMachine.SetState<ShutdownState>();
        }
    }
}