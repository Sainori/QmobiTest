using System;

namespace InputSystem.Interfaces
{
    public interface IInputSystem
    {
        Action OnRight { get; set; }
        Action OnLeft { get; set; }
        Action OnUp { get; set; }
        Action OnDown { get; set; }
        Action OnSpace { get; set; }
        Action OnRestart { get; set; }
        Action OnEscape { get; set; }
        Action OnEscapeUp { get; set; }
        void DirectUpdate();
        void Reset();
    }
}