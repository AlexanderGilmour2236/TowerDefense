using UnityEngine;

namespace Towers.Views
{
    public class TowerView : MonoBehaviour
    {
        private Transform _target;
        public TowerData TowerData { get; private set; }
        public void SetData(TowerData towerData)
        {
            TowerData = towerData;
        }

        public void SetTarget(Transform target)
        {
            
            if (_target != null && target == _target) return;
            
//            target.GetComponent<MeshRenderer>().material.color = Color.red;
//            if(_target!=null)
//                _target.GetComponent<MeshRenderer>().material.color = Color.white;
            
            _target = target;
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            var targetDirerction = _target.position - transform.position;
            //targetDirerction.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirerction), Time.time * 0.5f);
            
        }
    }
}