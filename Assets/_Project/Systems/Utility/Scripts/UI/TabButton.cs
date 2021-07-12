using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceGame.Utility.UI
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private bool _disabled;

        public bool Disabled
        {
            get => _disabled;
            set => _disabled = value;
        }
        public TabGroup TabGroup { get; set; }
        public Image Image => _image;

        

        private void Awake()
        {
            _image = GetComponent<Image>();
            Assert.IsNotNull(_image);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Assert.IsNotNull(TabGroup);
            if (_disabled) return;
            TabGroup.OnSelect(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Assert.IsNotNull(TabGroup);
            if (_disabled) return;
            TabGroup.OnEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Assert.IsNotNull(TabGroup);
            if (_disabled) return;
            TabGroup.OnLeave(this);
        }

        private void Reset() => _image = GetComponent<Image>();
    }
}
