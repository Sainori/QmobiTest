using PlayerController.Interfaces;

namespace UiController.Interfaces
{
    public interface IUiController
    {
        void Initialize(ScoreCounter scoreCounter, IPlayerController playerController, uint secondsToQuit);
        void Reset();
        void SetScreen(Screen screen, bool status);
    }
}