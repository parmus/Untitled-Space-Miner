using UnityEngine;

namespace SpaceGame.Utility
{
    public class SimpleRotate : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 30f;
        [SerializeField] private Vector3 _rotationAxis = Vector3.up;
        [SerializeField] private Space _relativeTo = Space.Self;
    
        private Transform _transform;

        private void Awake() => _transform = transform;

        private void Update() => _transform.Rotate(_rotationAxis * (_rotationSpeed * Time.deltaTime), _relativeTo);
    }
}
