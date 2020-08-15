using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class LandingState : FSM.State {
        private const float RotationSpeed = 60f;
        private const float FlightSpeed = 100f;
        private const float LandingDelay = 1f;


        public LandingState(FSM fsm, Shuttle shuttle) : base(fsm, shuttle) { }

        public override void Enter() {
            base.Enter();
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.MiningTool.enabled = false;
            _shuttle.StartCoroutine(Land());
        }

        private IEnumerator Land() {
            var r = _shuttle.GetComponent<Rigidbody>();
            r.velocity = Vector3.zero;
            r.angularVelocity = Vector3.zero;

            var transform = _shuttle.transform;
            var landingPadUp = _shuttle.LandingPad.up;
            var landingPadPosition =  _shuttle.LandingPad.position;
            
            var right = Vector3.Cross(landingPadUp, landingPadPosition - transform.position).normalized;
            var forward = Vector3.Cross(landingPadUp, -right).normalized;

            var landingRotation = Quaternion.LookRotation(forward, Vector3.up);
            var rotationTime = Quaternion.Angle(transform.rotation, landingRotation) / RotationSpeed;
            var flightTime = Mathf.Max(
                Vector3.Distance(landingPadPosition, transform.position) / FlightSpeed,
                rotationTime + LandingDelay
            );
            var alignTime = Quaternion.Angle(landingRotation, _shuttle.LandingPad.rotation) / RotationSpeed;

            var seq = DOTween.Sequence();
            seq.Append(transform
                .DORotateQuaternion(landingRotation, rotationTime)
                .SetEase(Ease.InOutBack)
            );
            seq.Join(transform
                .DOMove(landingPadPosition, flightTime)
                .SetEase(Ease.OutQuad)
            );
            seq.Append(transform
                .DORotateQuaternion(_shuttle.LandingPad.rotation, alignTime)
                .SetEase(Ease.InSine)
            );
            yield return seq.WaitForCompletion();

            _fsm.SetState<LandedState>();
        }
    }
}