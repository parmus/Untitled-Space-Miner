namespace SpaceGame.Doors
{
    public interface IDoor
    {
        bool Locked { get; set; }
        bool Open { get; set; }
    }
}