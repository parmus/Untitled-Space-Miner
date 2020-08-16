namespace SpaceGame
{
    public interface IPersistable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}