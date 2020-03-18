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
            if (target == Target)
            {
                return;
            }
            
            if (target != null)
            {
                if (_shootingCoroutine == null)
                {
                    Target = target;
                    _shootingCoroutine = StartCoroutine(TowerShootingCoroutine());
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

            var targetDirection = Target.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), Time.time * 0.3f);
        }

        private void PlayShootAnimation()
        {
            _animator.Play("TowerShoot");
        }
        
        private IEnumerator TowerShootingCoroutine()
        {
            while (Target != null)
            {
                var normalizedDirection = (Target.transform.position - transform.position).normalized;
                var targetInFrontOf = Vector3.Dot(normalizedDirection, transform.forward);
                
                if (targetInFrontOf > 0.8f)
                {
                    Target.TakeDamage(TowerData.Damage);
                    PlayShootAnimation();
                }
                else
                {
                    yield return this;
                    continue;
                }

                yield return new WaitForSeconds(TowerData.ShootInterval);
            }
            _shootingCoroutine = null;
        }

        private void OnDisable()
        {
            if (_shootingCoroutine == null) return;

            Target = null;
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }
    }
}