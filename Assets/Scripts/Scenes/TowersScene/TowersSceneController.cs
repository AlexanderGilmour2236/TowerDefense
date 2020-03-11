using System.Linq;
using Towers;
using Towers.Views;
using UnityEngine;

namespace Scenes.TowersScene
{
    public class TowersSceneController : MonoBehaviour
    {
        [Header("Towers")]
        [SerializeField]
        private TowerSystemPresenter towerSystemPresenter;
        [SerializeField] 
        private Canvas canvas;
        [SerializeField] 
        private TowerMenuView towerMenuPrefab;
        [SerializeField] 
        private TowerData[] towerDatas;

        [Header("Player")]
        [SerializeField, Range(0,100)] 
        private int startHealth;
        [SerializeField] 
        private int startGold;

        [Header("Enemies")] 
        [SerializeField] 
        private EnemySystemPresenter _enemySystemPresenter;
        
        private TowerMenuView _currentTowerMenuView;
        private RectTransform _canvasRectTransform;
        private TowerSlotView _selectedTowerSlot;
        private Player.Player _player;
        
        private void Start()
        {
            towerSystemPresenter.Init();
            towerSystemPresenter.TowerSlotClick += OnTowerSlotClick;
            
            _enemySystemPresenter.Init();
            
            _canvasRectTransform = canvas.GetComponent<RectTransform>();

            _player = new Player.Player();
            
            _player.Gold.ValueChanged += OnGoldValueChanged;
            _player.Health.ValueChanged += OnHealthValueChanged;
            
            _player.Gold.Value = startGold;
            _player.Health.Value = startHealth;
        }

        private void OnDestroy()
        {
            towerSystemPresenter.TowerSlotClick -= OnTowerSlotClick;
            _player.Gold.ValueChanged -= OnGoldValueChanged;
            _player.Health.ValueChanged -= OnHealthValueChanged;
        }
        
        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            _selectedTowerSlot = towerSlot;
            
            HideTowerMenu();
            ShowTowerMenu(towerSlot);

            _currentTowerMenuView.MenuItemClick += OnMenuItemClick;
        }

        private void HideTowerMenu()
        {
            if (_currentTowerMenuView == null) return;
            
            Destroy(_currentTowerMenuView.gameObject);
            _currentTowerMenuView.MenuItemClick -= OnMenuItemClick;
        }

        private void ShowTowerMenu(TowerSlotView towerSlot)
        {
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
        }

        private void OnMenuItemClick(TowerMenuItemArgs args)
        {
            if (args.MenuItemAction == TowerMenuItemAction.Upgrade)
            {
                towerSystemPresenter.SetTower(_selectedTowerSlot, args.TowerData);
                _player.Gold.Value -= args.TowerData.BuiltPrice;
            }
            else
            {
                towerSystemPresenter.SellTower(_selectedTowerSlot);
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