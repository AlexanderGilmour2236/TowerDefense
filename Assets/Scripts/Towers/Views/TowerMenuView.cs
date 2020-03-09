using System;
using UnityEngine;

namespace Towers.Views
{
    public class TowerMenuView : MonoBehaviour
    {
        [SerializeField]
        private TowerMenuItem[] _items;

        public event Action<TowerMenuItemAction, TowerData> MenuItemClick;
        
        public void Init()
        {
            foreach (var menuItem in _items)
            {
                menuItem.ItemClick += MenuItemOnItemClick;
            }
        }
        
        public void Dispose()
        {
            foreach (var menuItem in _items)
            {
                menuItem.ItemClick -= MenuItemOnItemClick;
            }
        }

        private void MenuItemOnItemClick(TowerMenuItemAction actionType, TowerData towerData)
        {
            MenuItemClick?.Invoke(actionType, towerData);
        }
    }
}