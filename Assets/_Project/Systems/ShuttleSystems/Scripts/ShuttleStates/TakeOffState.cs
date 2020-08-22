using System.Collections;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class TakeOffState : ShuttleStateMachine.State {
        public TakeOffState(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) : base(shuttleStateMachine, shuttle) {}

        public override void Enter() {
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.MiningTool.enabled = false;
            _shuttle.StartCoroutine(TakeOff());
        }

        private IEnumerator TakeOff() {
            var r = _shuttle.GetComponent<Rigidbody>();
            r.AddRelativeForce(Vector3.up * 100f);
            yield return new WaitForSeconds(1.5f);
            _shuttleStateMachine.SetState<FlyingState>();
        }
    }
}