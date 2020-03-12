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
            
        }

        private void Update()
        {
            if (_target == null)
            {
                transform.LookAt(Vector3.zero);
                return;
            }
            transform.LookAt(_target);
        }
    }
}