using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Towers.Views
{
    public class TowerSlotView : MonoBehaviour, IPointerClickHandler
    {
        private TowerView _towerView;

        public event Action<TowerSlotView> SlotClick;
        
        public void SetTower(TowerView towerView)
        {
            _towerView = towerView;
        }

        public bool SellTower()
        {
            if (_towerView == null) return false;
            
            Destroy(_towerView.gameObject);
            _towerView = null;
            return true;

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SlotClick?.Invoke(this);
        }
    }
}