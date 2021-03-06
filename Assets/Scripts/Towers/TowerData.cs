﻿using Towers.Views;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Tower")]
    public class TowerData : ScriptableObject
    {
        public string Name;
        public float BuiltPrice;
        public float Range;
        public float ShootInterval;
        public float Damage;
        public float SellMultiplier;
        public TowerView towerPrefab;
    }
}

