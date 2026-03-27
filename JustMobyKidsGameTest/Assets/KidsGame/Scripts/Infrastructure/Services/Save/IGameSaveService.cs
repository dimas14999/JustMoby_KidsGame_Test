namespace KidsGame.Scripts.Infrastructure.Services.Save
{
    public interface IGameSaveService
    {
        void Save(GameSaveData data);
        GameSaveData Load();
    }
}

