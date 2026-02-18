namespace JM.Saves
{
    public interface ISaveService
    {
        void Save(GameProgressDTO progress);
        GameProgressDTO Load();
    }
}
