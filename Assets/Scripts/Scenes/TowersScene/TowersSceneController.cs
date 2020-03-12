using System.Linq;
using DG.Tweening;
using EnemySystem.Views;
using TMPro;
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
        private TowerMenuPresenter towerMenuPresenter;
        
        [Header("Player")]
        [SerializeField, Range(0,100)] 
        private int startHealth;
        [SerializeField] 
        private int startGold;
        [SerializeField] 
        private Transform playerCastle;

        [Header("Enemies")] 
        [SerializeField] 
        private EnemySystemPresenter enemySystemPresenter;

        [Header("UI")] 
        [SerializeField] 
        private TextMeshProUGUI goldsText; 
        [SerializeField] 
        private TextMeshProUGUI healthText;
        [SerializeField] 
        private TextMeshProUGUI wavesText;
        [SerializeField] 
        private Camera camera;
        
        private TowerSlotView _selectedTowerSlot;
        private Player.Player _player;
        
        private void Start()
        {
            towerSystemPresenter.Init();
            towerSystemPresenter.TowerSlotClick += OnTowerSlotClick;
           
            enemySystemPresenter.Init();
            enemySystemPresenter.EnemyDie += OnEnemyDie;
            enemySystemPresenter.EnemyCompletePath += OnEnemyCompletePath;
            
            _player = new Player.Player();
            
            _player.Gold.ValueChanged += OnGoldValueChanged;
            _player.Health.ValueChanged += OnHealthValueChanged;
            
            _player.Gold.Value = startGold;
            _player.Health.Value = startHealth;
            
            towerMenuPresenter.Init(_player, towerDatas);
            towerMenuPresenter.MenuItemClick += OnMenuItemClick;
        }

        private void Update()
        {
            foreach (var tower in towerSystemPresenter.Towers)
            {
                var minDistanceToCastle = 0.0f;
                EnemyView targetEnemyView = null;
                
                foreach (var enemy in enemySystemPresenter.Enemies)
                {
                    var enemyCompletePathPercent = enemySystemPresenter.CompletePathPercent(enemy);
                    var distanceToTower = Vector3.Distance(enemy.transform.position, tower.transform.position);
                    
                    if (distanceToTower <= tower.TowerData.Range && enemyCompletePathPercent >= minDistanceToCastle)
                    {
                        minDistanceToCastle = enemyCompletePathPercent;
                        targetEnemyView = enemy;
                    }
                }
                tower.SetTarget(targetEnemyView);
            }
        }

        private void OnDestroy()
        {
            towerSystemPresenter.TowerSlotClick -= OnTowerSlotClick;
            _player.Gold.ValueChanged -= OnGoldValueChanged;
            _player.Health.ValueChanged -= OnHealthValueChanged;
            towerMenuPresenter.MenuItemClick -= OnMenuItemClick;
            enemySystemPresenter.EnemyDie -= OnEnemyDie;
            enemySystemPresenter.EnemyCompletePath -= OnEnemyCompletePath;
        }
        
        private void DestroyEnemy(EnemyView enemyView)
        {
            towerSystemPresenter.LoseTarget(enemyView);
        }
        
        private void OnEnemyDie(EnemyView enemy)
        {
            _player.Gold.Value += Random.Range(enemy.EnemyData.MinGold, enemy.EnemyData.MaxGold);
            DestroyEnemy(enemy);
        }

        private void OnEnemyCompletePath(EnemyView enemy)
        {
            _player.Health.Value -= enemy.EnemyData.Damage;
            DestroyEnemy(enemy);
            camera.transform.DOShakePosition(0.1f, 0.2f);
        }
        
        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            _selectedTowerSlot = towerSlot;
            
            towerMenuPresenter.HideTowerMenu();
            towerMenuPresenter.ShowTowerMenu(towerSlot);
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
            goldsText.text = ((int)gold).ToString();
        }
        
        private void OnHealthValueChanged(int health)
        {
            healthText.text = health.ToString();
        }
    }
}