using System.Collections;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class TakeOffState : FSM.State {
        public TakeOffState(FSM fsm, Shuttle shuttle) : base(fsm, shuttle) {}

        public override void Enter() {
            base.Enter();
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.MiningTool.enabled = false;
            _shuttle.StartCoroutine(TakeOff());
        }

        private IEnumerator TakeOff() {
            var r = _shuttle.GetComponent<Rigidbody>();
            r.AddRelativeForce(Vector3.up * 100f);
            yield return new WaitForSeconds(1.5f);
            _fsm.SetState<FlyingState>();
        }
    }
}