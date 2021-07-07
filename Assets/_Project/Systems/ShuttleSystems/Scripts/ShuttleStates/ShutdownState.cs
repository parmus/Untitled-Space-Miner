using System.Collections;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class ShutdownState : ShuttleStateMachine.State {

        public ShutdownState(ShuttleStateMachine shuttleStateMachine) : base(shuttleStateMachine)
        {
        }

        public override void Enter(Shuttle shuttle) {
            shuttle.Thrusters.enabled = false;
            shuttle.CameraControl.enabled = false;
            shuttle.InertiaDampers.enabled = false;
            shuttle.MiningTool.enabled = false;

            shuttle.StartCoroutine(Die(shuttle));
        }

        private IEnumerator Die(Shuttle shuttle) {
            var rigidbody = shuttle.GetComponent<Rigidbody>();
            rigidbody.AddTorque(Vector3.one * 20f);

            yield return new WaitForSeconds(10);

            Object.Destroy(shuttle.gameObject);
        }
    }
}
