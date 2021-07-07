using SpaceGame.Utility;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class FlyingState : ShuttleStateMachine.State {
        public FlyingState(ShuttleStateMachine shuttleStateMachine) : base(shuttleStateMachine) {}

        public override void Enter(Shuttle shuttle) {
            shuttle.Thrusters.enabled = true;
            shuttle.CameraControl.enabled = true;
            shuttle.MiningTool.enabled = true;
            shuttle.PowerSystem.Charge.Subscribe(OnChargeChange);
        }

        public override void Leave(Shuttle shuttle) => shuttle.PowerSystem.Charge.Unsubscribe(OnChargeChange);

        private void OnChargeChange(float charge)
        {
            if (charge.IsZero()) _shuttleStateMachine.SetState<ShutdownState>();
        }
    }
}
