using System;
using System.Collections.Generic;
using System.Linq;
using EnemySystem.Views;
using Misc;
using Towers.Views;
using UnityEngine;

namespace Towers
{
    public class TowerSystemPresenter : MonoBehaviour
    {
        [SerializeField] 
        private TowerSystemView view;

        private PrefabPoolManager<TowerView> _towersPoolManager = new PrefabPoolManager<TowerView>();
        public List<TowerView> Towers { get; } = new List<TowerView>();
        
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

        public void SetTower(TowerSlotView towerSlotView, TowerData towerData)
        {
            var towerPool = _towersPoolManager.GetPool(towerData.towerPrefab) ?? _towersPoolManager.CreatePool(view.transform, towerData.towerPrefab);
            
            var towerView =  towerPool.GetObject();
            towerView.Init();
            towerView.SetData(towerData);
            Towers.Add(towerView);
            view.SetTower(towerSlotView, towerView);
        }

        public void LoseTarget(EnemyView enemyView)
        {
            foreach (var tower in Towers.Where(tower => tower.Target == enemyView))
            {
                tower.SetTarget(null);
            }
        }

        public void ClearTowers()
        {
            foreach (var tower in Towers)
            {
                tower.SetTarget(null);
                _towersPoolManager.GetPool(tower.TowerData.towerPrefab).ReleaseObject(tower);
                view.ClearSlots();
            }
            Towers.Clear();
        }

        public void SellTower(TowerSlotView towerSlot)
        {
            _towersPoolManager.GetPool(towerSlot.TowerView.TowerData.towerPrefab).ReleaseObject(towerSlot.TowerView);
            Towers.Remove(towerSlot.TowerView);
            view.SellTower(towerSlot);
        }
    }
}