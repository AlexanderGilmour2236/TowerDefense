using UnityEngine;

namespace EnemySystem.Views
{
    public class EnemyView : MonoBehaviour
    {
        public EnemyData EnemyData { get; private set; }
        public void SetData(EnemyData data)
        {
            EnemyData = data;
        }
    }


}