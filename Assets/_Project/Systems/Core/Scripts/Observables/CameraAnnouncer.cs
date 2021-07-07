using UnityEngine;

namespace SpaceGame.Core.Observables
{
    [RequireComponent(typeof(Camera))]
    public class CameraAnnouncer : MonoBehaviour
    {
        [SerializeField] private CameraAnchor _cameraAnchor;
        private void Start() => _cameraAnchor.Value.Set(GetComponent<Camera>());
    }
}
