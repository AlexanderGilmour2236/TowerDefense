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
        [SerializeField] 
        private RectTransform itemsParent;
        
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
                var item = Instantiate(towerMenuItemPrefab, itemsParent);

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
        
        public void ShowMenu(TowerSlotView towerSlot, TowerMenuView towerMenuView, RectTransform canvasRectTransform)
        {
            var viewportPosition = Camera.main.WorldToViewportPoint(towerSlot.transform.position);

            var proportionalPosition = new Vector2(
                viewportPosition.x * canvasRectTransform.sizeDelta.x,
                viewportPosition.y * canvasRectTransform.sizeDelta.y
            );

            var uiOffset = new Vector2(
                canvasRectTransform.sizeDelta.x / 2f,
                canvasRectTransform.sizeDelta.y / 2f
            );

            towerMenuView.transform.localPosition = proportionalPosition - uiOffset;

            LayoutRebuilder.ForceRebuildLayoutImmediate(itemsParent);

            itemsParent.localPosition = (proportionalPosition - uiOffset).y > 0 ? new Vector2(0,itemsParent.rect.height * -0.5f) : new Vector2(0,itemsParent.rect.height * 0.5f);
        }
    }
}