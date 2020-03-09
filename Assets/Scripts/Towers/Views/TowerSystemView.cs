using System;
using System.Collections.Generic;
using AppInfrastructure;
using UnityEngine;

namespace Towers.Views
{
    public class TowerSystemView : MonoBehaviour
    {
        [SerializeField]
        private List<TowerSlotView> towerSlotViews;

        [SerializeField] 
        private TowerMenuView buyTowerMenuViewPrefab;
        [SerializeField]
        private TowerMenuView editTowerMenuViewPrefab;
        
        [SerializeField] 
        private TowerData[] availableTowers;
        
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

        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            TowerSlotClick?.Invoke(towerSlot);
        }

        public void ShowMenu(TowerMenuView towerMenuView)
        {
            if(_currentTowerMenuView != null) 
                Destroy(_currentTowerMenuView.gameObject);

            _currentTowerMenuView = towerMenuView;
        }
    }
}