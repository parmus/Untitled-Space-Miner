using DG.Tweening;
using UnityEngine;

namespace SpaceGame.Doors
{
    public class Door : MonoBehaviour, IDoor
    {
        [SerializeField] private bool _locked;

        [Header("Animation")]
        [SerializeField] private float _height = 2f;
        [SerializeField] private float _timeToOpen = 1f;


        public bool Locked
        {
            get => _locked;
            set
            {
                _locked = value;
                if (_locked) Open = false;
            }
        }

        public bool Open
        {
            get => _open;
            set
            {
                if (value && _locked) return;
                _open = value;
                if (_open)
                {
                    _seq.PlayForward();
                }
                else
                {
                    _seq.PlayBackwards();
                }
            }
    }

        private Sequence _seq;
        private bool _open;

        private void Awake()
        {
            _seq = DOTween
                .Sequence()
                .Append(transform.DOMove(transform.position + (Vector3.up * _height), _timeToOpen))
                .SetAutoKill(false);
            _seq.PlayBackwards();
        }
    }
}
