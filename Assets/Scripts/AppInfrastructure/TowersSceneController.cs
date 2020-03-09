using System;
using System.Collections;
using Towers;
using Towers.Views;
using UnityEngine;

namespace AppInfrastructure
{
    public class TowersSceneController : MonoBehaviour
    {
        [SerializeField] 
        private Transform towerSystemParent;
        [SerializeField] 
        private Canvas _canvas;
        
        private PresenterLoader _presenterLoader;

        private void Start()
        {
            _presenterLoader = new PresenterLoader();

            _presenterLoader.Load(new TowerSystemPresenter(), "TowerSystem", towerSystemParent);
        }
    }
}