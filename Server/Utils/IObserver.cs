namespace TrisGame
{
    public interface IObserver<in T>
    {
        public void OnNotify(T obj);
    }
}