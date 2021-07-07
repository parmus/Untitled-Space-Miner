using System.Collections;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class TakeOffState : ShuttleStateMachine.State {
        public TakeOffState(ShuttleStateMachine shuttleStateMachine) : base(shuttleStateMachine)
        {
        }

        public override void Enter(Shuttle shuttle) {
            shuttle.Thrusters.enabled = false;
            shuttle.CameraControl.enabled = false;
            shuttle.MiningTool.enabled = false;
            shuttle.StartCoroutine(TakeOff(shuttle));
        }

        private IEnumerator TakeOff(Shuttle shuttle)
        {
            var rigidbody = shuttle.GetComponent<Rigidbody>();
            rigidbody.AddRelativeForce(Vector3.up * 100f);
            yield return new WaitForSeconds(1.5f);
            _shuttleStateMachine.SetState<FlyingState>();
        }
    }
}
