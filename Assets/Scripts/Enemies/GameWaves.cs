using UnityEngine;

namespace EnemySystem
{
    [CreateAssetMenu(fileName = "GameWaves", menuName = "GameEnemyWaves")]
    public class GameWaves : ScriptableObject
    {
        public EnemyWave[] EnemyWaves;
    }
}