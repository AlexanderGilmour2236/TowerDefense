using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Towers.Views
{
    public class TowerMenuItem : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI itemNameText;
        [SerializeField] 
        private TextMeshProUGUI itemPriceText;

        public void SetData(TowerMenuItemArgs args)
        {
            if (args.MenuItemAction == TowerMenuItemAction.Upgrade)
            {
                itemNameText.text = args.TowerData.Name;
                itemPriceText.text = args.TowerData.BuiltPrice.ToString();
                return;
            }
            itemNameText.text = args.MenuItemAction.ToString();
        }
    }
}