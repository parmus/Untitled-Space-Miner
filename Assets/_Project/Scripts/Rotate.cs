using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 0.3f;
    private Transform _transform = default;

    private void Awake() => _transform = transform;

    private void Update() => _transform.Rotate(Vector3.up * _rotationSpeed, Space.Self);
}
