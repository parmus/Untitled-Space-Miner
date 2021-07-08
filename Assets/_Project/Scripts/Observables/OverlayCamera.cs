using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SpaceGame.Observables
{
    [RequireComponent(typeof(Camera))]
    public class OverlayCamera : MonoBehaviour
    {
        [SerializeField] private CameraAnchor _cameraAnchor;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            _cameraAnchor.Subscribe(ConnectCamera);
        }

        private void OnDisable()
        {
            _cameraAnchor.Unsubscribe(ConnectCamera);
            if (_cameraAnchor.Value == null) return;
            var stack = _cameraAnchor.Value.GetUniversalAdditionalCameraData().cameraStack;
            stack.Remove(_camera);
        }

        private void ConnectCamera(Camera camera)
        {
            if (camera == null) return;
            var stack = camera.GetUniversalAdditionalCameraData().cameraStack;
            stack.Add(_camera);
        }
    }
}
