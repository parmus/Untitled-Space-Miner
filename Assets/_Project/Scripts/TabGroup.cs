using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceGame
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private int _currentTab = 0;
        [SerializeField] private List<TabButton> _tabButtons = new List<TabButton>();
        [SerializeField] private List<GameObject> _tabs = new List<GameObject>();
        
        [SerializeField] private Color _normalTint = Color.white;
        [SerializeField] private Color _selectedTint = Color.white;
        [SerializeField] private Color _hoverTint = Color.white;

        private void Start()
        {
            foreach (var tabButton in _tabButtons)
            {
                tabButton.TabGroup = this;
            }

            OnSelect(_tabButtons[_currentTab]);
        }

        private void ClearTabButtonState()
        {
            _tabButtons.ForEach(tabButton => tabButton.Image.color = _normalTint);
        }

        public void OnSelect(TabButton tabButton)
        {
            _currentTab = _tabButtons.IndexOf(tabButton);
            Assert.IsTrue(_currentTab < _tabs.Count);
            
            ClearTabButtonState();
            tabButton.Image.color = _selectedTint;

            for (var i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].SetActive(i == _currentTab);
            }

        }

        public void OnEnter(TabButton tabButton)
        {
            var index = _tabButtons.IndexOf(tabButton);
            tabButton.Image.color = index == _currentTab ? _selectedTint : _hoverTint;
        }

        public void OnLeave(TabButton tabButton)
        {
            var index = _tabButtons.IndexOf(tabButton);
            tabButton.Image.color = index == _currentTab ? _selectedTint : _normalTint;
        }
    }
}
