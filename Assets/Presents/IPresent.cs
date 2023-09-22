namespace Feudal.Presents
{
    public interface IPresent
    {
        Session session { get; set; }
        void RefreshUIView(UIView mono);
    }

    public abstract class Present<T> : IPresent
        where T : UIView
    {
        public Session session { get; set; }

        public abstract void Refresh(T view);

        public void RefreshUIView(UIView mono)
        {
            Refresh(mono as T);
        }
    }
}