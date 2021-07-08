using UnityEngine;

namespace SpaceGame.Observables
{
    [RequireComponent(typeof(Camera))]
    public class CameraAnnouncer : MonoBehaviour
    {
        [SerializeField] private CameraAnchor _cameraAnchor;
        private void Start() => _cameraAnchor.Set(GetComponent<Camera>());
    }
}
