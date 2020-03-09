using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Towers.Views
{
    public class TowerMenuItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] 
        private TowerData _towerData;
        [SerializeField]
        private TowerMenuItemAction _menuItemAction;

        public event Action<TowerMenuItemAction, TowerData> ItemClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            ItemClick?.Invoke(_menuItemAction, _towerData);
        }
    }
}