using SpaceGame.Utility;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.ShuttleSystems.MiningTool
{
    public class SelectUnderCursor : MonoBehaviour
    {
        #region Private variables
        private static readonly Vector3 ScreenCenter = new Vector3(0.5f, 0.5f);
        private Camera _camera;
        private Ray _ray;
        private RaycastHit _hit;
        private Transform _cameraTransform;
        #endregion

        #region Scanner Configuration
        [Header("Scanner configuration")]
        [SerializeField] private float _maxScanDistance = 100f;
        [SerializeField] private LayerMask _layerMask = default;
        #endregion

        public static IReadonlyObservable<GameObject> Target => _target;
        public static Vector3 Hit { get; private set; }


        private static readonly Observable<GameObject> _target = new Observable<GameObject>();

        private void Awake() {
            _camera = Camera.main;
            Assert.IsNotNull(_camera);
            _cameraTransform = _camera.transform;
            Hit = Vector3.zero;
        }

        private void Update()
        {
            _ray = _camera.ViewportPointToRay(ScreenCenter);
            if (Physics.Raycast(_ray, out _hit, _maxScanDistance, _layerMask)) {
                Set(_hit.collider.gameObject, _hit.point);
            } else {
                Set(null, _cameraTransform.position + _cameraTransform.forward * _maxScanDistance);
            }
        }

        private static void Set(GameObject target, Vector3 hit) {
            Hit = hit;
            _target.Value = target;
        }

    }
}
