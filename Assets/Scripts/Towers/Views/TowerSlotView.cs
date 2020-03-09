using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Towers.Views
{
    public class TowerSlotView : MonoBehaviour, IPointerClickHandler
    {
        public TowerView TowerView { get; private set; }

        public event Action<TowerSlotView> SlotClick;
        
        public void SetTower(TowerView towerView)
        {
            TowerView = towerView;
        }

        public bool SellTower()
        {
            if (TowerView == null) return false;
            
            Destroy(TowerView.gameObject);
            TowerView = null;
            return true;

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SlotClick?.Invoke(this);
        }
    }
}