using System;
using Towers.Views;
using UnityEngine;

namespace Towers
{
    public class TowerSystemPresenter : MonoBehaviour
    {
        [SerializeField] 
        private TowerSystemView view;
        public event Action<TowerSlotView> TowerSlotClick;
        
        public void Init()
        {
            view.TowerSlotClick += OnTowerSlotClick;
        }

        private void OnDestroy()
        {
            view.TowerSlotClick -= OnTowerSlotClick;
        }

        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            TowerSlotClick?.Invoke(towerSlot);
        }

        public void SetTower(TowerSlotView towerSlotView, TowerData towerData) => view.SetTower(towerSlotView, Instantiate(towerData.towerPrefab));

        public void SellTower(TowerSlotView selectedTowerSlot) => view.SellTower(selectedTowerSlot);

    }
}