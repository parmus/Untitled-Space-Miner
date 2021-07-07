using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class LandingState : ShuttleStateMachine.State {
        private const float ROTATION_SPEED = 60f;
        private const float FLIGHT_SPEED = 100f;
        private const float LANDING_DELAY = 1f;


        public LandingState(ShuttleStateMachine shuttleStateMachine) : base(shuttleStateMachine)
        {
        }

        public override void Enter(Shuttle shuttle) {
            shuttle.Thrusters.enabled = false;
            shuttle.CameraControl.enabled = false;
            shuttle.MiningTool.enabled = false;
            shuttle.StartCoroutine(Land(shuttle));
        }

        private IEnumerator Land(Shuttle shuttle)
        {
            var rigidbody = shuttle.GetComponent<Rigidbody>();
            var transform = shuttle.transform;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            var position = transform.position;
            var rotation = transform.rotation;
            
            var right = Vector3.Cross(shuttle.LandingPad.Up, shuttle.LandingPad.Position - position).normalized;
            var forward = Vector3.Cross(shuttle.LandingPad.Up, -right).normalized;

            var landingRotation = Quaternion.LookRotation(forward, Vector3.up);
            var rotationTime = Quaternion.Angle(rotation, landingRotation) / ROTATION_SPEED;
            var flightTime = Mathf.Max(
                Vector3.Distance(shuttle.LandingPad.Position, position) / FLIGHT_SPEED,
                rotationTime + LANDING_DELAY
            );
            var alignTime = Quaternion.Angle(landingRotation, shuttle.LandingPad.Rotation) / ROTATION_SPEED;

            var seq = DOTween.Sequence();
            seq.Append(transform
                .DORotateQuaternion(landingRotation, rotationTime)
                .SetEase(Ease.InOutBack)
            );
            seq.Join(transform
                .DOMove(shuttle.LandingPad.Position, flightTime)
                .SetEase(Ease.OutQuad)
            );
            seq.Append(transform
                .DORotateQuaternion(shuttle.LandingPad.Rotation, alignTime)
                .SetEase(Ease.InSine)
            );
            yield return seq.WaitForCompletion();

            _shuttleStateMachine.SetState<LandedState>();
        }
    }
}
