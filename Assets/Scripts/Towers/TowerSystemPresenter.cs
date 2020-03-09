using 
    AppInfrastructure;
using Towers.Views;
using UnityEngine;

namespace Towers
{
    public class TowerSystemPresenter : ViewablePresenter<TowerSystemView>
    {
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
            Debug.Log("CLICKED");
        }
    }
}