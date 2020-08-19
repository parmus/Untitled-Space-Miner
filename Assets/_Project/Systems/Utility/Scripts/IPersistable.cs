namespace SpaceGame.Utility
{
    public interface IPersistable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}