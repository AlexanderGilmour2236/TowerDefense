using System;
using AppInfrastructure;
using Towers.Views;

namespace Towers
{
    public class TowerSystemPresenter : ViewablePresenter<TowerSystemView>
    {
        public event Action<TowerSlotView> TowerSlotClick;
        
        public override void OnPresenterLoaded()
        {
            View.TowerSlotClick += OnTowerSlotClick;
            base.OnPresenterLoaded();
        }

        public override void OnPresenterUnloaded()
        {
            View.TowerSlotClick -= OnTowerSlotClick;
            base.OnPresenterUnloaded();
        }

        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            TowerSlotClick?.Invoke(towerSlot);
        }


        public void SetTower(TowerSlotView towerSlotView, TowerData argsTowerData) => View.SetTower(towerSlotView, argsTowerData);

        public void SellTower(TowerSlotView selectedTowerSlot) => View.SellTower(selectedTowerSlot);

    }
}