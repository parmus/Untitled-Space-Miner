using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class LandedState : ShuttleStateMachine.State {
        public LandedState(ShuttleStateMachine shuttleStateMachine) : base(shuttleStateMachine) { }

        public override void Enter(Shuttle shuttle) {
            shuttle.Thrusters.enabled = false;
            shuttle.CameraControl.enabled = false;
            shuttle.MiningTool.enabled = false;
            
            shuttle.InputReader.OnFlightThrust += OnFlightThrust;
        }

        public override void Leave(Shuttle shuttle)
        {
            base.Leave(shuttle);
            shuttle.InputReader.OnFlightThrust -= OnFlightThrust;
        }

        private void OnFlightThrust(Vector2 thrust)
        {
            if (thrust.y > Mathf.Epsilon) _shuttleStateMachine.SetState<TakeOffState>();
        }
    }
}
