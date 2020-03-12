using System.Collections;
using EnemySystem.Views;
using UnityEngine;

namespace Towers.Views
{
    public class TowerView : MonoBehaviour
    {
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
                StopAllCoroutines();
            }

//            target.GetComponent<MeshRenderer>().material.color = Color.red;
//            if(_target!=null)
//                _target.GetComponent<MeshRenderer>().material.color = Color.white;
            
            Target = target;
            StartCoroutine(TowerShootingCoroutine());
        }

        private void Update()
        {
            if (Target == null)
            {
                return;
            }

            var targetDirerction = Target.transform.position - transform.position;
            //targetDirerction.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirerction), Time.time * 0.5f);
        }

        private IEnumerator TowerShootingCoroutine()
        {
            while (true)
            {
                if (Target == null)
                {
                    yield break;
                }
                Target.TakeDamage(TowerData.Damage);
                yield return new WaitForSeconds(TowerData.ShootInterval);
            }
        }
    }
}