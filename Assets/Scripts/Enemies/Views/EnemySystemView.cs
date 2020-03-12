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
        
        private Vector3[] _enemyPathPoints;
        
        public Dictionary<EnemyView, Tweener> EnemyTweens { get; } = new Dictionary<EnemyView, Tweener>();
        public Transform EnemyParent => enemyParent;
        public float CompletePathPercent(EnemyView view) => EnemyTweens[view]?.ElapsedPercentage() ?? 0;
        
        public event Action<EnemyView> EnemyCompletePath;

        private void Start()
        {
            _enemyPathPoints = enemyPath.Points.Select(point => point.position).ToArray();
        }

        public void StartEnemy(EnemyView newEnemyView, float movingSpeed)
        {
            newEnemyView.transform.position = enemyPath.Points[0].position;
            
            EnemyTweens.Add(newEnemyView,newEnemyView.transform.DOPath(_enemyPathPoints, movingSpeed)
                .SetEase(Ease.Linear)
                .SetSpeedBased(true)
                .SetLookAt(0)
                .OnKill(
                () =>
                {
                    EnemyCompletePath?.Invoke(newEnemyView);
                    EnemyTweens.Remove(newEnemyView);
                }));
        }

        public void SetPath(EnemyPath enemyPath)
        {
            this.enemyPath = enemyPath;
            _enemyPathPoints = enemyPath.Points.Select(point => point.position).ToArray();
        }
    }
}