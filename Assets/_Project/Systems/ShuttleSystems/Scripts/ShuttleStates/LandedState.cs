using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class LandedState : ShuttleStateMachine.State {
        public LandedState(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) : base(shuttleStateMachine, shuttle) { }

        public override void Enter() {
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.MiningTool.enabled = false;
            
            _shuttle.InputReader.OnFlightThrust += OnFlightThrust;
        }

        public override void Leave()
        {
            base.Leave();
            _shuttle.InputReader.OnFlightThrust -= OnFlightThrust;
        }

        private void OnFlightThrust(Vector2 thrust)
        {
            if (thrust.y > Mathf.Epsilon) _shuttleStateMachine.SetState<TakeOffState>();
        }
    }
}
