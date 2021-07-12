using SpaceGame.Utility.SaveSystem;
using UnityEngine;

namespace SpaceGame.ShuttleSystems {

    [AddComponentMenu("Shuttle Systems/Camera Control")]
    public class CameraControl : MonoBehaviour, IPersistable {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _cameraRotationSpeed = 10f;

        private Vector2 _rotations = Vector2.zero;
        private Vector2 _lookMovement = Vector2.zero;
        private Shuttle _shuttle;

        private void Awake() => _shuttle = GetComponent<Shuttle>();


        private void OnFlightLook(Vector2 movement) => _lookMovement = movement;

        private void OnEnable() {
            var currentRotation = transform.rotation;
            _rotations.x = currentRotation.eulerAngles.y;
            _rotations.y = -currentRotation.eulerAngles.x;
            _shuttle.InputReader.OnFlightLook += OnFlightLook;
        }

        private void OnDisable()
        {
            _shuttle.InputReader.OnFlightLook -= OnFlightLook;
            OnFlightLook(Vector2.zero);
        }
        
        private void Update() {
            _rotations += _lookMovement * (Time.deltaTime * _cameraRotationSpeed);
            _rotations.y = Mathf.Clamp(_rotations.y, -85, 85);

            _rigidbody.MoveRotation(Quaternion.Euler(
                -_rotations.y,
                _rotations.x,
                0f
            ));
        }

        private void Reset() => _rigidbody = GetComponent<Rigidbody>();
        
        #region IPersistable
        public object CaptureState() => _rotations;

        public void RestoreState(object state) => _rotations = (Vector2) state;
        #endregion
        
    }
}
