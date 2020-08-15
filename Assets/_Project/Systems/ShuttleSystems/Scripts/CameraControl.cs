using UnityEngine;

namespace SpaceGame.ShuttleSystems {

    [AddComponentMenu("Shuttle Systems/Camera Control")]
    public class CameraControl : MonoBehaviour {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _cameraRotationSpeed = 10f;

        private Vector2 _rotations = Vector2.zero;
        private Shuttle _shuttle;

        private void Reset() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            var currentRotation = transform.rotation;
            _rotations.x = currentRotation.eulerAngles.y;
            _rotations.y = -currentRotation.eulerAngles.x;
        }

        private void Awake() {
            _shuttle = GetComponent<Shuttle>();
        }

        private void Update() {
            _rotations += _shuttle.ShuttleControls.OnLook * (Time.deltaTime * _cameraRotationSpeed);
            _rotations.y = Mathf.Clamp(_rotations.y, -85, 85);

            _rigidbody.MoveRotation(Quaternion.Euler(
                -_rotations.y,
                _rotations.x,
                0f
            ));
        }
    }
}