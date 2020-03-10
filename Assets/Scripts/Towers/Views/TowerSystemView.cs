using System;
using System.Collections.Generic;
using System.Linq;
using AppInfrastructure;
using UnityEngine;

namespace Towers.Views
{
    public class TowerSystemView : MonoBehaviour
    {
        [SerializeField]
        private List<TowerSlotView> towerSlotViews;
        
        private TowerMenuView _currentTowerMenuView;
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

        public void SetTower(TowerSlotView towerSlot, TowerData data)
        {
            towerSlot.SetTower(Instantiate(data.towerPrefab));
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