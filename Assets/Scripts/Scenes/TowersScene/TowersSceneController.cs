using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using EnemySystem;
using EnemySystem.Views;
using TMPro;
using Towers;
using Towers.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.TowersScene
{
    public class TowersSceneController : MonoBehaviour
    {
        [Header("Towers")]
        [SerializeField]
        private TowerSystemPresenter towerSystemPresenter;
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
        [SerializeField]
        private GameWaves gameWaves;
        
        [Header("UI")] 
        [SerializeField] 
        private TextMeshProUGUI goldsText; 
        [SerializeField] 
        private TextMeshProUGUI healthText;
        [SerializeField] 
        private TextMeshProUGUI wavesText;
        [SerializeField] 
        private Camera camera;
        [SerializeField]
        private GameOverScreenView gameOverScreenView;
        [SerializeField] 
        private TextMeshProUGUI countDownNumberText;
        [SerializeField]
        private TextMeshProUGUI countDownText;
        
        private TowerSlotView _selectedTowerSlot;
        private Player.Player _player;
        private Coroutine _countDownCoroutine;

        private void Start()
        {
            towerSystemPresenter.Init();
            towerSystemPresenter.TowerSlotClick += OnTowerSlotClick;
           
            enemySystemPresenter.Init();
            enemySystemPresenter.EnemyDie += OnEnemyDie;
            enemySystemPresenter.EnemyCompletePath += OnEnemyCompletePath;
            enemySystemPresenter.WaveComplete += OnWaveComplete;
            
            _player = new Player.Player();
            
            _player.Gold.ValueChanged += OnGoldValueChanged;
            _player.Health.ValueChanged += OnHealthValueChanged;
            _player.CurrentWaveCount.ValueChanged += OnCurrentWaveCountChanged;
            
            _player.Gold.Value = startGold;
            _player.Health.Value = startHealth;
            _player.CurrentWaveCount.Value = 0;
            
            towerMenuPresenter.Init(_player, towerDatas);
            towerMenuPresenter.MenuItemClick += OnMenuItemClick;
            
            gameOverScreenView.ButtonClick += StartGame;
            
            StartGame();
        }

        private void Update()
        {
            foreach (var tower in towerSystemPresenter.Towers)
            {
                var maxEnemyCompletePathPercent = 0.0f;
                EnemyView targetEnemyView = null;
                
                foreach (var enemy in enemySystemPresenter.Enemies)
                {
                    var enemyCompletePathPercent = enemySystemPresenter.CompletePathPercent(enemy);
                    var distanceToTower = Vector3.Distance(enemy.transform.position, tower.transform.position);
                    
                    if (distanceToTower <= tower.TowerData.Range && enemyCompletePathPercent >= maxEnemyCompletePathPercent)
                    {
                        maxEnemyCompletePathPercent = enemyCompletePathPercent;
                        targetEnemyView = enemy;
                    }
                }
                tower.SetTarget(targetEnemyView);
            }
        }

        private void OnDestroy()
        {
            towerSystemPresenter.TowerSlotClick -= OnTowerSlotClick;
            towerMenuPresenter.MenuItemClick -= OnMenuItemClick;
            
            _player.Gold.ValueChanged -= OnGoldValueChanged;
            _player.Health.ValueChanged -= OnHealthValueChanged;
            _player.CurrentWaveCount.ValueChanged -= OnCurrentWaveCountChanged;
            
            enemySystemPresenter.EnemyDie -= OnEnemyDie;
            enemySystemPresenter.EnemyCompletePath -= OnEnemyCompletePath;
            enemySystemPresenter.WaveComplete -= OnWaveComplete;
            
            gameOverScreenView.ButtonClick -= StartGame;
        }

        private void OnWaveComplete()
        {
            if (_countDownCoroutine != null)
            {
                StopCoroutine(_countDownCoroutine);
                _countDownCoroutine = null;
            }
            
            var nextWaveCount = ++_player.CurrentWaveCount.Value;

            if (gameWaves.EnemyWaves.Length <= nextWaveCount)
            {
                GameOver(true);                
                return;
            }
            
            _countDownCoroutine = StartCoroutine(CountDownBeforeWave(5, () =>
            {
                enemySystemPresenter.StartWave(gameWaves.EnemyWaves[nextWaveCount]);
            }));
        }

        private IEnumerator CountDownBeforeWave(float seconds, Action callback)
        {
            countDownText.gameObject.SetActive(true);
            countDownNumberText.gameObject.SetActive(true);
            
            var secondsLeft = seconds;
            while (secondsLeft>=0)
            {
                countDownNumberText.text = secondsLeft.ToString();
                yield return new WaitForSeconds(1);
                secondsLeft--;
            }
            countDownText.gameObject.SetActive(false);
            countDownNumberText.gameObject.SetActive(false);

            callback?.Invoke();
        }

        private void StartGame()
        {
            _player.CurrentWaveCount.Value = 0;
            
            gameOverScreenView.gameObject.SetActive(false);
            _player.Gold.Value = startGold;
            _player.Health.Value = startHealth;
            
            _countDownCoroutine = StartCoroutine(CountDownBeforeWave(3, () =>
            {
                enemySystemPresenter.StartWave(gameWaves.EnemyWaves[0]);
            }));
        }
        
        private void LoseEnemyTarget(EnemyView enemyView)
        {
            towerSystemPresenter.LoseTarget(enemyView);
        }
        
        private void OnEnemyDie(EnemyView enemy)
        {
            _player.Gold.Value += Random.Range(enemy.EnemyData.MinGold, enemy.EnemyData.MaxGold);
            LoseEnemyTarget(enemy);
        }

        private void OnEnemyCompletePath(EnemyView enemy)
        {
            _player.Health.Value -= enemy.EnemyData.Damage;
            if (_player.Health.Value <= 0)
            {
                GameOver(false);
                return;
            }
            LoseEnemyTarget(enemy);
            camera.transform.DOShakePosition(0.1f, 0.2f);
        }

        private void OnTowerSlotClick(TowerSlotView towerSlot)
        {
            if (_selectedTowerSlot != null && _selectedTowerSlot == towerSlot)
            {
                _selectedTowerSlot = null;
                towerMenuPresenter.HideTowerMenu();
                return;
            }
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
        
        private void GameOver(bool isWin)
        {
            camera.transform.DOShakePosition(2.0f, 1.5f);

            towerMenuPresenter.HideTowerMenu();
            towerSystemPresenter.ClearTowers();
            gameOverScreenView.gameObject.SetActive(true);
            gameOverScreenView.SetText(isWin ? "YOU WON" : "GAME OVER");
            gameOverScreenView.SetButtonText("RESTART");
            
            foreach (var enemy in enemySystemPresenter.Enemies)
            {
                LoseEnemyTarget(enemy);
            }
            enemySystemPresenter.ClearEnemies();
        }
        
        private void OnGoldValueChanged(float gold)
        {
            goldsText.text = $"{gold}g";
        }
        
        private void OnHealthValueChanged(int health)
        {
            healthText.text = $"{health}hp";
        }

        private void OnCurrentWaveCountChanged(int count)
        {
            wavesText.text = $"{count+1}/{gameWaves.EnemyWaves.Length}";
        }
    }
}