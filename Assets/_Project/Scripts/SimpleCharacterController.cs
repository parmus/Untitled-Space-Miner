using System;
using UnityEngine;

namespace SpaceGame
{
    public class SimpleCharacterController : MonoBehaviour
    {
        [SerializeField] private Transform _body = default;
        [SerializeField] private Transform _head = default;
        [SerializeField] private CharacterController _characterController = default;

        [Header("Movement parameters")]
        [SerializeField] private float _rotationSpeed =  1f;
        [SerializeField] private float _walkSpeed =  1f;
        [SerializeField] private float _runSpeed =  1.5f;
        [SerializeField] private float _gravity = -9.81f;

        private ICharacterControls _characterControls;
        private float _xRotation = 0f;
        private float _velocity = 2f;

        private void Awake()
        {
            _characterControls = GetComponent<ICharacterControls>();
        }

        private void Update()
        {
            LookAround();
            Move();
        }

        private void Move()
        {
            var move = _body.right * _characterControls.Movement.x + _body.forward * _characterControls.Movement.y;

            move *= _characterControls.InRunning ? _runSpeed : _walkSpeed;

            _velocity += _gravity * Time.deltaTime;
            if (_characterController.isGrounded) _velocity = -0.2f;
            move += Vector3.up * _velocity;

            _characterController.Move(move * Time.deltaTime);
        }

        private void LookAround()
        {
            _xRotation -= _characterControls.OnLook.y * _rotationSpeed * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _head.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _body.Rotate(Vector3.up * (_characterControls.OnLook.x * _rotationSpeed * Time.deltaTime));
        }

        private void Reset()
        {
            _characterControls = GetComponent<ICharacterControls>();
            _body = transform;
            _characterController = GetComponent<CharacterController>();
        }
    }
}
