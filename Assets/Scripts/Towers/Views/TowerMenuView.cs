using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Towers.Views
{
    public class TowerMenuView : MonoBehaviour
    {
        [SerializeField] 
        private TowerMenuItem towerMenuItemPrefab;
        
        private List<TowerMenuItem> _items = new List<TowerMenuItem>();
        
        public event Action<TowerMenuItemArgs> MenuItemClick;

        private void OnDestroy()
        {
            foreach (var menuItem in _items)
            {
                Destroy(menuItem.gameObject);
            }
            _items.Clear(); 
        }

        public void AddItems(params TowerMenuItemArgs[] args)
        {
            foreach (var arg in args)
            {
                var item = Instantiate(towerMenuItemPrefab, transform);

                var button = item.GetComponent<Button>();
                
                if(arg.IsActive)
                    button.onClick.AddListener(()=>MenuItemOnItemClick(arg));

                button.interactable = arg.IsActive;
                    
                item.SetData(arg);
                _items.Add(item);
            }
        }
        
        private void MenuItemOnItemClick(TowerMenuItemArgs arg)
        {
            MenuItemClick?.Invoke(arg);
        }
    }
}