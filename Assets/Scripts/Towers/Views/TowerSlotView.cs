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
            if (towerView == null) return;
            
            towerView.transform.SetParent(transform);
            towerView.transform.localPosition = Vector3.zero;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SlotClick?.Invoke(this);
        }
    }
}