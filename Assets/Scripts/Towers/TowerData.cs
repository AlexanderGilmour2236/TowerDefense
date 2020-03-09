using Towers.Views;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Tower")]
    public class TowerData : ScriptableObject
    {
        public float BuiltPrice;
        public float Range;
        public float ShootInterval;
        public float Damage;
        public TowerView towerPrefab;
    }
}

