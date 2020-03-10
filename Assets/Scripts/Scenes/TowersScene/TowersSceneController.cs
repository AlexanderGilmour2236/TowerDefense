using System.Collections.Generic;
using System.Linq;
using AppInfrastructure;
using Towers;
using Towers.Views;
using UnityEngine;

namespace Scenes.TowersScene
{
    public class TowersSceneController : MonoBehaviour
    {
        [SerializeField] 
        private Transform towerSystemParent;
        [SerializeField] 
        private Canvas canvas;
        [SerializeField] 
        private TowerMenuView towerMenuPrefab;
        [SerializeField] 
        private TowerData[] towerDatas;
        
        private TowerMenuView _currentTowerMenuView;
        private PresenterLoader _presenterLoader;
        private TowerSystemPresenter _towerSystemPresenter;
        private RectTransform _canvasRectTransform;
        private TowerSlotView _selectedTowerSlot;
        
        private void Start()
        {
            _presenterLoader = new PresenterLoader();
            _towerSystemPresenter = new TowerSystemPresenter();
            
            _presenterLoader.Load(_towerSystemPresenter, "TowerSystem", towerSystemParent);
            _towerSystemPresenter.TowerSlotClick += OnTowerSlotClick;

            _canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        private void OnDestroy()
        {
            _towerSystemPresenter.TowerSlotClick -= OnTowerSlotClick;
        }
        
        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            _selectedTowerSlot = towerSlot;
            
            if (_currentTowerMenuView != null)
            {
                Destroy(_currentTowerMenuView.gameObject);
                _currentTowerMenuView.MenuItemClick -= OnMenuItemClick;
            }

            _currentTowerMenuView = Instantiate(towerMenuPrefab, canvas.transform);
            
            var viewportPosition = Camera.main.WorldToViewportPoint(towerSlot.transform.position);
            
            var proportionalPosition = new Vector2(
                viewportPosition.x * _canvasRectTransform.sizeDelta.x,
                viewportPosition.y * _canvasRectTransform.sizeDelta.y
                );
            
            var uiOffset = new Vector2(
                _canvasRectTransform.sizeDelta.x / 2f,
                _canvasRectTransform.sizeDelta.y / 2f
                );
            
            _currentTowerMenuView.transform.localPosition = proportionalPosition - uiOffset;

            if (towerSlot.TowerView == null)
            {
                _currentTowerMenuView.AddItems(towerDatas.Select(towerData => new TowerMenuItemArgs(TowerMenuItemAction.Upgrade, towerData: towerData)).ToArray());
            }
            else
            {
                _currentTowerMenuView.AddItems(new TowerMenuItemArgs(TowerMenuItemAction.Sell));
            }
            
            _currentTowerMenuView.MenuItemClick += OnMenuItemClick;
        }

        private void OnMenuItemClick(TowerMenuItemArgs args)
        {
            if (args.MenuItemAction == TowerMenuItemAction.Upgrade)
            {
                _towerSystemPresenter.SetTower(_selectedTowerSlot, args.TowerData);
            }
            else
            {
                _towerSystemPresenter.SellTower(_selectedTowerSlot);
            }
            
            if (_currentTowerMenuView != null) 
                Destroy(_currentTowerMenuView.gameObject);
        }
    }
}