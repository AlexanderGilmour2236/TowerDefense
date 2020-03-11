using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace EnemySystem.Views
{
    public class EnemySystemView : MonoBehaviour
    {
        [SerializeField] 
        private EnemyPath enemyPath;
        [SerializeField]
        private Transform enemyParent;
        
        private List<EnemyView> _enemies = new List<EnemyView>();
        private Vector3[] _enemyPathPoints;
        
        public Transform EnemyParent => enemyParent;

        public event Action<EnemyView> EnemyCompletePath;

        private void Start()
        {
            _enemyPathPoints = enemyPath.Points.Select(point => point.position).ToArray();
        }

        public void StartEnemy(EnemyView newEnemyView, float movingSpeed)
        {
            _enemies.Add(newEnemyView);
            newEnemyView.transform.position = enemyPath.Points[0].position;
            
            newEnemyView.transform.DOPath(_enemyPathPoints, movingSpeed).SetEase(Ease.Linear).SetSpeedBased(true).OnKill(
                () =>
                {
                    EnemyCompletePath?.Invoke(newEnemyView);
                });
        }

        public void SetPath(EnemyPath enemyPath)
        {
            this.enemyPath = enemyPath;
            _enemyPathPoints = enemyPath.Points.Select(point => point.position).ToArray();
        }
    }
}