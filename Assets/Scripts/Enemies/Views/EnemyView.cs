using System;
using UnityEngine;

namespace EnemySystem.Views
{
    public class EnemyView : MonoBehaviour
    {
        private Animator _animator;
        public EnemyData EnemyData { get; private set; }
        public float Health { get; private set; }

        public event Action<EnemyView> EnemyDie;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetData(EnemyData data)
        {
            EnemyData = data;
            Health = data.Health;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            PlayHitAnimation();
            if (Health <= 0)
            {
                EnemyDie?.Invoke(this);
            }
        }

        private void PlayHitAnimation()
        {
            _animator.Play("Hit"); 
        }
    }


}