using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SpaceGame.ShuttleSystems.ShuttleStates {
    public class LandingState : ShuttleStateMachine.State {
        private const float ROTATION_SPEED = 60f;
        private const float FLIGHT_SPEED = 100f;
        private const float LANDING_DELAY = 1f;

        private readonly Rigidbody _rigidbody;
        private readonly Transform _transform;

        public LandingState(ShuttleStateMachine shuttleStateMachine, Shuttle shuttle) : base(shuttleStateMachine, shuttle)
        {
            _rigidbody = shuttle.GetComponent<Rigidbody>();
            _transform = _shuttle.transform;
        }

        public override void Enter() {
            _shuttle.Thrusters.enabled = false;
            _shuttle.CameraControl.enabled = false;
            _shuttle.MiningTool.enabled = false;
            _shuttle.StartCoroutine(Land());
        }

        private IEnumerator Land() {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            var position = _transform.position;
            var rotation = _transform.rotation;
            
            var right = Vector3.Cross(_shuttle.LandingPad.Up, _shuttle.LandingPad.Position - position).normalized;
            var forward = Vector3.Cross(_shuttle.LandingPad.Up, -right).normalized;

            var landingRotation = Quaternion.LookRotation(forward, Vector3.up);
            var rotationTime = Quaternion.Angle(rotation, landingRotation) / ROTATION_SPEED;
            var flightTime = Mathf.Max(
                Vector3.Distance(_shuttle.LandingPad.Position, position) / FLIGHT_SPEED,
                rotationTime + LANDING_DELAY
            );
            var alignTime = Quaternion.Angle(landingRotation, _shuttle.LandingPad.Rotation) / ROTATION_SPEED;

            var seq = DOTween.Sequence();
            seq.Append(_transform
                .DORotateQuaternion(landingRotation, rotationTime)
                .SetEase(Ease.InOutBack)
            );
            seq.Join(_transform
                .DOMove(_shuttle.LandingPad.Position, flightTime)
                .SetEase(Ease.OutQuad)
            );
            seq.Append(_transform
                .DORotateQuaternion(_shuttle.LandingPad.Rotation, alignTime)
                .SetEase(Ease.InSine)
            );
            yield return seq.WaitForCompletion();

            _shuttleStateMachine.SetState<LandedState>();
        }
    }
}