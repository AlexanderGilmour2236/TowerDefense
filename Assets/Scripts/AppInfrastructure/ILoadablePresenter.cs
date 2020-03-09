namespace AppInfrastructure
{
    public interface ILoadablePresenter
    {
        void OnPresenterLoaded();
        void OnPresenterUnloaded();
    }
}