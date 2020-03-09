using UnityEngine;

namespace AppInfrastructure
{
    public abstract class Presenter : ILoadablePresenter
    {
        public virtual void OnPresenterLoaded()
        {
        }

        public virtual void OnPresenterUnloaded()
        {
        }

        public abstract void SetView(MonoBehaviour view);
        public abstract MonoBehaviour GetView { get; }
    }
    
}