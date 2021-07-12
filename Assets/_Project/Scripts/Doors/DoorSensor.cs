using UnityEngine;

namespace SpaceGame.Doors
{
    public class DoorSensor : MonoBehaviour
    {
        [SerializeField] private Transform _sensorOrigin;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _sensorRange = 3f;
        
        private readonly Collider[] _sensorHits = new Collider[1];
        private IDoor _door;
        
        private void Awake() => _door = GetComponent<IDoor>();

        private void Update()
        {
            var hits = Physics.OverlapSphereNonAlloc(_sensorOrigin.position, _sensorRange, _sensorHits, _layerMask);
            _door.Open = hits > 0;
        }

        private void Reset() => _sensorOrigin = transform;
    }
}
