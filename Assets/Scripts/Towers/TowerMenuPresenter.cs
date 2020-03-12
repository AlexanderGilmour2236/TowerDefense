using System;
using System.Linq;
using Towers.Views;
using UnityEngine;

namespace Towers
{
    public class TowerMenuPresenter : MonoBehaviour
    {
        [SerializeField] 
        private TowerMenuView towerMenuPrefab;
        [SerializeField]
        private RectTransform canvasRectTransform;
        
        private TowerMenuView _currentTowerMenuView;
        private Player.Player _player;
        private TowerData[] _towerDatas;

        public event Action<TowerMenuItemArgs> MenuItemClick;
        
        public void Init(Player.Player player, TowerData[] towerDatas)
        {
            _player = player;
            _towerDatas = towerDatas;
        }
        
        public void HideTowerMenu()
        {
            if (_currentTowerMenuView == null) return;
            
            Destroy(_currentTowerMenuView.gameObject);
            _currentTowerMenuView.MenuItemClick -= OnMenuItemClick;
        }

        public void ShowTowerMenu(TowerSlotView towerSlot)
        {
            _currentTowerMenuView = Instantiate(towerMenuPrefab, canvasRectTransform);
            _currentTowerMenuView.MenuItemClick += OnMenuItemClick;
            
            if (towerSlot.TowerView == null)
            {
                _currentTowerMenuView.AddItems(_towerDatas.Select(towerData => new TowerMenuItemArgs(
                    TowerMenuItemAction.Upgrade,
                    towerData,
                    _player.Gold.Value >= towerData.BuiltPrice)).ToArray());
            }
            else
            {
                _currentTowerMenuView.AddItems(new TowerMenuItemArgs(TowerMenuItemAction.Sell, towerSlot.TowerView.TowerData));
            }
            
            _currentTowerMenuView.ShowMenu(towerSlot, _currentTowerMenuView, canvasRectTransform);
        }

        private void OnMenuItemClick(TowerMenuItemArgs args)
        {            
            if (_currentTowerMenuView != null) 
                Destroy(_currentTowerMenuView.gameObject);
            
            MenuItemClick?.Invoke(args);
        }

        private void OnDestroy()
        {
            _currentTowerMenuView.MenuItemClick -= OnMenuItemClick;
        }
    }
}