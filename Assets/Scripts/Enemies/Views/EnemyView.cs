using System;
using UnityEngine;

namespace EnemySystem.Views
{
    public class EnemyView : MonoBehaviour
    {
        public EnemyData EnemyData { get; private set; }
        public float Health { get; private set; }

        public event Action<EnemyView> EnemyDie;
        
        public void SetData(EnemyData data)
        {
            EnemyData = data;
            Health = data.Health;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                EnemyDie?.Invoke(this);
            }
                
        }
    }


}