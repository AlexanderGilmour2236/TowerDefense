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
        private TowerData[] towerDatas;
        [SerializeField] 
        private TowerMenuPresenter _towerMenuPresenter;
        
        [Header("Player")]
        [SerializeField, Range(0,100)] 
        private int startHealth;
        [SerializeField] 
        private int startGold;

        [Header("Enemies")] 
        [SerializeField] 
        private EnemySystemPresenter _enemySystemPresenter;
        
        private TowerSlotView _selectedTowerSlot;
        private Player.Player _player;
        
        private void Start()
        {
            towerSystemPresenter.Init();
            towerSystemPresenter.TowerSlotClick += OnTowerSlotClick;
           
            _enemySystemPresenter.Init();

            _player = new Player.Player();
            
            _player.Gold.ValueChanged += OnGoldValueChanged;
            _player.Health.ValueChanged += OnHealthValueChanged;
            
            _player.Gold.Value = startGold;
            _player.Health.Value = startHealth;
            
            _towerMenuPresenter.Init(_player, towerDatas);
            _towerMenuPresenter.MenuItemClick += OnMenuItemClick;
        }

        private void OnDestroy()
        {
            towerSystemPresenter.TowerSlotClick -= OnTowerSlotClick;
            _player.Gold.ValueChanged -= OnGoldValueChanged;
            _player.Health.ValueChanged -= OnHealthValueChanged;
            _towerMenuPresenter.MenuItemClick -= OnMenuItemClick;
        }
        
        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            _selectedTowerSlot = towerSlot;
            
            _towerMenuPresenter.HideTowerMenu();
            _towerMenuPresenter.ShowTowerMenu(towerSlot);
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
                _player.Gold.Value += args.TowerData.BuiltPrice * args.TowerData.SellMultiplier;
            }
        }
        
        private void  OnGoldValueChanged(float gold)
        {
            
        }
        
        private void OnHealthValueChanged(int health)
        {
            
        }
    }
}