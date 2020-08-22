namespace SpaceGame.Utility.SaveSystem
{
    public interface IPersistable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}