using System;

namespace InputSystem.Interfaces
{
    public interface IInputSystem
    {
        Action OnRight { get; set; }
        Action OnLeft { get; set; }
        Action OnUp { get; set; }
        Action OnDown { get; set; }
        void DirectUpdate();
    }
}