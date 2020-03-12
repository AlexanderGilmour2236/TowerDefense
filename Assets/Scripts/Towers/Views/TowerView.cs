using System;
using System.Collections;
using EnemySystem.Views;
using UnityEngine;

namespace Towers.Views
{
    public class TowerView : MonoBehaviour
    {
        private Coroutine _shootingCoroutine;
        private Animator _animator;
        public EnemyView Target { get; private set; }
        public TowerData TowerData { get; private set; }

        public void Init()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetData(TowerData towerData)
        {
            TowerData = towerData;
        }

        public void SetTarget(EnemyView target)
        {
            if (Target != null)
            {
                //if (target == Target) return;

                if (_shootingCoroutine == null)
                {
                    Target = target;
                    _shootingCoroutine = StartCoroutine(TowerShootingCoroutine());
                    return;
                }
            }
            Target = target;
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

        private void PlayShootAnimation()
        {
            _animator.Play("TowerShoot");
        }
        
        private IEnumerator TowerShootingCoroutine()
        {
            while (Target != null)
            {
                Target.TakeDamage(TowerData.Damage);
                PlayShootAnimation();
                
                yield return new WaitForSeconds(TowerData.ShootInterval);
            }
            _shootingCoroutine = null;
        }
    }
}