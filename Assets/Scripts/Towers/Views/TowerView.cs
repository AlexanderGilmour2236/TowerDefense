using System;
using System.Collections;
using EnemySystem.Views;
using UnityEngine;

namespace Towers.Views
{
    public class TowerView : MonoBehaviour
    {
        private Coroutine shootingCoroutine;
        
        public EnemyView Target { get; private set; }
        public TowerData TowerData { get; private set; }
        public void SetData(TowerData towerData)
        {
            TowerData = towerData;
        }

        public void SetTarget(EnemyView target)
        {
            if (Target != null)
            {
                if (target == Target) return;
            }
            
            Target = target;
            
            if (shootingCoroutine == null)
                shootingCoroutine = StartCoroutine(TowerShootingCoroutine());
        }

        private void Update()
        {
            if (Target == null)
            {
                return;
            }

            var targetDirerction = Target.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirerction), Time.time * 0.5f);
        }

        private IEnumerator TowerShootingCoroutine()
        {
            while (true)
            {
                if (Target != null)
                {
                    Target.TakeDamage(TowerData.Damage);
                }
                yield return new WaitForSeconds(TowerData.ShootInterval);
                if (Target == null) break;
            }
            shootingCoroutine = null;
        }
    }
}