using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class LandedState : FSM.State {
        public LandedState(FSM fsm, Shuttle shuttle) : base(fsm, shuttle) { }

        public override void Enter() {
            base.Enter();
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.MiningTool.enabled = false;
        }

        public override void Tick() {
            if (_shuttle.ShuttleControls.Thrust.y > Mathf.Epsilon) _fsm.SetState<TakeOffState>();
        }
    }
}