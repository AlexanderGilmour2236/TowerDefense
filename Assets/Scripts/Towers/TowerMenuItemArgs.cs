using UnityEngine;

namespace Towers
{
    public class TowerMenuItemArgs
    {
        public TowerData TowerData { get; }
        public TowerMenuItemAction MenuItemAction { get; }
        public bool IsActive { get; }
        
        public TowerMenuItemArgs(TowerMenuItemAction menuItemAction, TowerData towerData, bool isActive = true)
        {
            MenuItemAction = menuItemAction;
            TowerData = towerData;
            IsActive = isActive;
        }
    }
}