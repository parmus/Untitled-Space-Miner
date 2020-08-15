namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class FlyingState : FSM.State {
        public FlyingState(FSM fsm, Shuttle shuttle) : base(fsm, shuttle) {}

        private bool _readyToLand = false;

        public override void Enter() {
            base.Enter();
            _shuttle.Thrusters.enabled = true;
            _shuttle.CameraControl.enabled = true;
            _shuttle.MiningTool.enabled = true;
            _readyToLand = false;
        }

        public override void Tick() {
            if (!_readyToLand && !_shuttle.LandingPad) _readyToLand = true;

            if (_readyToLand && _shuttle.LandingPad) _fsm.SetState<LandingState>();

            if (_shuttle.PowerSystem.IsEmpty) {
                _fsm.SetState<ShutdownState>();
            }
        }
    }
}