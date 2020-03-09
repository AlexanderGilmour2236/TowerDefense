using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AppInfrastructure
{
    public class PresenterLoader
    {
        private const string _path = "PresenterViews/";
        
        private Dictionary<Type, Presenter> _loadedPresenters = new Dictionary<Type, Presenter>();

        public void Load<TView>(ViewablePresenter<TView> presenter, string resourceName, Transform parent = null) where TView : MonoBehaviour
        {
            var view = Object.Instantiate(Resources.Load<GameObject>(_path + resourceName));
            
            if(parent!=null)
                view.transform.SetParent(parent, false);
            
            presenter.SetView(view.GetComponent<MonoBehaviour>());
            _loadedPresenters[presenter.GetType()] = presenter;
            presenter.OnPresenterLoaded();
        }

        public void UnLoad(Type presenterType)
        {
            if (!_loadedPresenters.ContainsKey(presenterType))
            {
                Debug.Log($"There is no loaded module of type {presenterType.Name}");
                return;
            }
            Object.Destroy(_loadedPresenters[presenterType].GetView.gameObject);
            _loadedPresenters[presenterType].OnPresenterUnloaded();
            _loadedPresenters[presenterType] = null;
            _loadedPresenters.Remove(presenterType);
        }

        public void Clear()
        {
            foreach (var presenter in _loadedPresenters)
            {
                Object.Destroy(presenter.Value.GetView.gameObject);
            }
            _loadedPresenters.Clear();
        }
    }
}