using EnemySystem.Views;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public int Health;
    public float MovingSpeed;
    public float Damage;
    public int MinGold;
    public int MaxGold;
    public EnemyView enemyViewPrefab;
}
