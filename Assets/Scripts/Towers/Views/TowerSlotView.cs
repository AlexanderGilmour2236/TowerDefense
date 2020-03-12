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
            towerView.transform.SetParent(transform);
            towerView.transform.localPosition = Vector3.zero;
        }

        public void SellTower()
        {
            if (TowerView == null) return;
            
            Destroy(TowerView.gameObject);
            TowerView = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SlotClick?.Invoke(this);
        }
    }
}