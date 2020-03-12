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
            itemNameText.text = args.MenuItemAction == TowerMenuItemAction.Upgrade ? 
                args.TowerData.Name : args.MenuItemAction.ToString();
            
            itemPriceText.text = args.MenuItemAction == TowerMenuItemAction.Upgrade ? 
                args.TowerData.BuiltPrice.ToString() : (args.TowerData.BuiltPrice * args.TowerData.SellMultiplier).ToString();
        }
    }
}