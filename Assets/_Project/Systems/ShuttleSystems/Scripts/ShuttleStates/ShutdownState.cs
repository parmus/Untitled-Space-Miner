using System.Collections;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class ShutdownState : ShuttleStateMachine.State {
        private readonly Rigidbody _rigidbody;

        public ShutdownState(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) : base(shuttleStateMachine, shuttle) =>
            _rigidbody = shuttle.GetComponent<Rigidbody>();

        public override void Enter() {
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.InertiaDampers.enabled = false;
            _shuttle.MiningTool.enabled = false;

            _shuttle.StartCoroutine(Die());
        }

        private IEnumerator Die() {
            DeathKick();

            yield return new WaitForSeconds(10);

            Object.Destroy(_shuttle.gameObject);
        }

        private void DeathKick() => _rigidbody.AddTorque(Vector3.one * 20f);
    }
}