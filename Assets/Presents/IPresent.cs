namespace Feudal.Presents
{
    public interface IPresent
    {
        Session session { get; set; }
        void RefreshMonoBehaviour(UIView mono);
    }

    public abstract class Present<T> : IPresent
        where T : UIView
    {
        public Session session { get; set; }

        public abstract void Refresh(T view);

        public void RefreshMonoBehaviour(UIView mono)
        {
            Refresh(mono as T);
        }
    }
}