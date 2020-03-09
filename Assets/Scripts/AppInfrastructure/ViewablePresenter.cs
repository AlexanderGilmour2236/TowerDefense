using UnityEngine;

namespace AppInfrastructure
{
    public class ViewablePresenter<TView> : Presenter where TView : MonoBehaviour
    {
        protected TView View { get; private set; }
        public override MonoBehaviour GetView => View;

        public override void SetView(MonoBehaviour view)
        {
            View = view as TView;
        }
    }
}