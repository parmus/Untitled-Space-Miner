using SpaceGame.PlayerInput;
using UnityEngine;

namespace SpaceGame
{
    public class SimpleCharacterController : MonoBehaviour
    {
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _head;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private InputReader _inputReader;

        [Header("Movement parameters")]
        [SerializeField] private float _rotationSpeed =  1f;
        [SerializeField] private float _walkSpeed =  1f;
        [SerializeField] private float _runSpeed =  1.5f;
        [SerializeField] private float _gravity = -9.81f;

        private float _xRotation = 0f;
        private float _velocity = 2f;

        private Vector2 _movement;
        private Vector2 _look;
        private float _speed;
        
        private void Awake() => _inputReader.EnableRobot(); // Probably not the right place

        private void OnEnable()
        {
            OnRun(false);
            _inputReader.OnRobotLook += OnLook;
            _inputReader.OnRobotMovement += OnMovement;
            _inputReader.OnRobotRun += OnRun;
        }

        private void OnDisable()
        {
            _inputReader.OnRobotLook -= OnLook;
            _inputReader.OnRobotMovement -= OnMovement;
            _inputReader.OnRobotRun -= OnRun;

            OnLook(Vector2.zero);
            OnMovement(Vector2.zero);
            OnRun(false);
        }

        private void OnLook(Vector2 look) => _look = look;

        private void OnMovement(Vector2 movement) => _movement = movement;

        private void OnRun(bool isRunning) => _speed = isRunning ? _runSpeed : _walkSpeed;

        
        private void Update()
        {
            LookAround();
            Move();
        }

        private void Move()
        {
            var move = _body.right * _movement.x + _body.forward * _movement.y;

            move *= _speed;

            _velocity += _gravity * Time.deltaTime;
            if (_characterController.isGrounded) _velocity = -0.2f;
            move += Vector3.up * _velocity;

            _characterController.Move(move * Time.deltaTime);
        }

        private void LookAround()
        {
            _xRotation -= _look.y * _rotationSpeed * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _head.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _body.Rotate(Vector3.up * (_look.x * _rotationSpeed * Time.deltaTime));
        }

        private void Reset()
        {
            _body = transform;
            _characterController = GetComponent<CharacterController>();
        }
    }
}
