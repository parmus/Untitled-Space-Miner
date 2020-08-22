namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class FlyingState : ShuttleStateMachine.State {
        public FlyingState(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) : base(shuttleStateMachine, shuttle) {}

        private bool _readyToLand = false;

        public override void Enter() {
            _shuttle.Thrusters.enabled = true;
            _shuttle.CameraControl.enabled = true;
            _shuttle.MiningTool.enabled = true;
            _readyToLand = false;
        }

        public override void Tick() {
            if (!_readyToLand && !_shuttle.LandingPad) _readyToLand = true;

            if (_readyToLand && _shuttle.LandingPad) _shuttleStateMachine.SetState<LandingState>();

            if (_shuttle.PowerSystem.IsEmpty) {
                _shuttleStateMachine.SetState<ShutdownState>();
            }
        }
    }
}