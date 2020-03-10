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

        [SerializeField, Range(0,100)] 
        private int startHealth;
        [SerializeField] 
        private int startGold;
        
        private TowerMenuView _currentTowerMenuView;
        private PresenterLoader _presenterLoader;
        private TowerSystemPresenter _towerSystemPresenter;
        private RectTransform _canvasRectTransform;
        private TowerSlotView _selectedTowerSlot;
        private Player.Player _player;
        
        private void Start()
        {
            _presenterLoader = new PresenterLoader();
            _towerSystemPresenter = new TowerSystemPresenter();
            
            _presenterLoader.Load(_towerSystemPresenter, "TowerSystem", towerSystemParent);
            _towerSystemPresenter.TowerSlotClick += OnTowerSlotClick;

            _canvasRectTransform = canvas.GetComponent<RectTransform>();

            _player = new Player.Player();
            
            _player.Gold.ValueChanged += OnGoldValueChanged;
            _player.Health.ValueChanged += OnHealthValueChanged;
            
            _player.Gold.Value = startGold;
            _player.Health.Value = startHealth;
        }

        private void OnDestroy()
        {
            _towerSystemPresenter.TowerSlotClick -= OnTowerSlotClick;
            _player.Gold.ValueChanged -= OnGoldValueChanged;
            _player.Health.ValueChanged -= OnHealthValueChanged;
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
                _currentTowerMenuView.AddItems(towerDatas.Select(towerData => new TowerMenuItemArgs(
                    TowerMenuItemAction.Upgrade, 
                    _player.Gold.Value >= towerData.BuiltPrice,
                    towerData)).ToArray());
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
                _player.Gold.Value -= args.TowerData.BuiltPrice;
            }
            else
            {
                _towerSystemPresenter.SellTower(_selectedTowerSlot);
            }
            
            if (_currentTowerMenuView != null) 
                Destroy(_currentTowerMenuView.gameObject);
        }
        
        private void  OnGoldValueChanged(float gold)
        {
            
        }
        
        private void OnHealthValueChanged(int health)
        {
            
        }
    }
}