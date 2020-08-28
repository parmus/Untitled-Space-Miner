using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame.Utility.UI
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private int _firstTab = 0;
        [SerializeField] private List<Tab> _tabs = new List<Tab>();
        
        [SerializeField] private Color _normalTint = Color.white;
        [SerializeField] private Color _disabledTint = Color.grey;
        [SerializeField] private Color _selectedTint = Color.yellow;
        [SerializeField] private Color _hoverTint = Color.blue;

        private int _currentTab;
        
        private void Start()
        {
            _currentTab = _firstTab;
            foreach (var tabItem in _tabs) tabItem.Button.TabGroup = this;
            OnSelect(_tabs[_currentTab].Button);
        }

        private void ClearTabButtonState() =>
            _tabs.ForEach(item => item.Button.Image.color = item.Button.Disabled ? _disabledTint : _normalTint);

        private int IndexOf(TabButton tabButton) =>
            _tabs.FindIndex(item => item.Button == tabButton);

        public void OnSelect(TabButton tabButton)
        {
            _currentTab = IndexOf(tabButton);
            Assert.IsTrue(_currentTab < _tabs.Count);
            
            ClearTabButtonState();
            tabButton.Image.color = _selectedTint;

            for (var i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].GameObject.SetActive(i == _currentTab);
            }
        }
        
        public void OnEnter(TabButton tabButton) =>
            tabButton.Image.color = IndexOf(tabButton) == _currentTab ? _selectedTint : _hoverTint;

        public void OnLeave(TabButton tabButton) =>
            tabButton.Image.color = IndexOf(tabButton) == _currentTab ? _selectedTint : _normalTint;

        [System.Serializable]
        public class Tab
        {
            public TabButton Button;
            public GameObject GameObject;
        }
    }
}
