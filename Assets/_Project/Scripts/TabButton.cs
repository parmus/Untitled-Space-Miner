using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceGame
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public TabGroup TabGroup { get; set; }
        public Image Image => _image;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            Assert.IsNotNull(_image);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Assert.IsNotNull(TabGroup);
            TabGroup.OnSelect(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Assert.IsNotNull(TabGroup);
            TabGroup.OnEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Assert.IsNotNull(TabGroup);
            TabGroup.OnLeave(this);
        }
    }
}
