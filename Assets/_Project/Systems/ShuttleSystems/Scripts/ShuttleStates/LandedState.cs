using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class LandedState : ShuttleStateMachine.State {
        public LandedState(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) : base(shuttleStateMachine, shuttle) { }

        public override void Enter() {
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.MiningTool.enabled = false;
        }

        public override void Tick() {
            if (_shuttle.ShuttleControls.Thrust.y > Mathf.Epsilon) _shuttleStateMachine.SetState<TakeOffState>();
        }
    }
}