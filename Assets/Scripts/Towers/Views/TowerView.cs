using UnityEngine;

namespace Towers.Views
{
    public class TowerView : MonoBehaviour
    {
        public TowerData TowerData { get; private set; }
        public void SetData(TowerData towerData)
        {
            TowerData = towerData;
        }
    }
}