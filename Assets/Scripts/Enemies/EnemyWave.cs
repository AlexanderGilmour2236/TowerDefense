using System;
using UnityEngine;

namespace EnemySystem
{
    [Serializable]
    public class EnemyWave
    {
        public float Duration;
        public float SpawnEach;
        public EnemyWaveItem[] WaveItems;
    }
}