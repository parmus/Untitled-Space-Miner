using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame
{
    public class TabSystem : MonoBehaviour
    {
        [SerializeField] [Delayed] private int _tabIndex = 0;
        [SerializeField] private List<TabItem> _tabs = default;

        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                _tabIndex = Mathf.Clamp(value, 0, _tabs.Count - 1);
                SetCurrent(_tabs[_tabIndex]);
            }
        }

        private void Start() => _tabs.ForEach(tab => tab.TabButton.onClick.AddListener(() => SetCurrent(tab)));


        private void SetCurrent(TabItem tabItem)
        {
            _tabs.ForEach(tab => tab.TabPanel.SetActive(false));
            tabItem.TabPanel.SetActive(true);
        }

        private void OnValidate()
        {
            if (_tabs.Count == 0) return;
            _tabIndex = Mathf.Clamp(_tabIndex, 0, _tabs.Count - 1);
            SetCurrent(_tabs[_tabIndex]);
        }

        [System.Serializable]
        public class TabItem
        {
            [SerializeField] private Button _tabButton = default;
            [SerializeField] private GameObject _tabPanel = default;

            public Button TabButton => _tabButton;
            public GameObject TabPanel => _tabPanel;
        }
    }
}
