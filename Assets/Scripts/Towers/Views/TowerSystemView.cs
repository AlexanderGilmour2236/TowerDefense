using System;
using System.Collections.Generic;
using UnityEngine;

namespace Towers.Views
{
    public class TowerSystemView : MonoBehaviour
    {
        [SerializeField]
        private List<TowerSlotView> towerSlotViews;

        private List<TowerView> _towerViews = new List<TowerView>();
        
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
            _towerViews.Add(view);
        }
        
        public void SellTower(TowerSlotView towerSlot)
        {
            _towerViews.Remove(towerSlot.TowerView);
            towerSlot.SellTower();
        }
        
        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            TowerSlotClick?.Invoke(towerSlot);
        }
    }
}