namespace Gameplay.Player
{
    public interface IPlayerBehaviour
    {
        PlayerFacade Facade { get; set; }
        public void InitBehaviour();
        public void OnAllBehavioursInitialized();
    }
}
