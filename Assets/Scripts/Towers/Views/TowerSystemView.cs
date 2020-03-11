using System;
using System.Collections.Generic;
using UnityEngine;

namespace Towers.Views
{
    public class TowerSystemView : MonoBehaviour
    {
        [SerializeField]
        private List<TowerSlotView> towerSlotViews;
        
        public event Action<TowerSlotView> TowerSlotClick;

        public void Start()
        {
            foreach (var towerSlot in towerSlotViews)
            {
                towerSlot.SlotClick += OnTowerSlotClick;
            }
        }

        private void OnDestroy()
        {
            foreach (var towerSlot in towerSlotViews)
            {
                towerSlot.SlotClick -= OnTowerSlotClick;
                Destroy(towerSlot.gameObject);
            }
            towerSlotViews.Clear();
        }

        public void SetTower(TowerSlotView towerSlot, TowerView view)
        {
            towerSlot.SetTower(view);
        }
        
        public void SellTower(TowerSlotView towerSlot)
        {
            towerSlot.SellTower();
        }
        
        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            TowerSlotClick?.Invoke(towerSlot);
        }
    }
}