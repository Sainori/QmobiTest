using System;
using InputSystem.Interfaces;
using UnityEngine;

namespace InputSystem
{
    public class InputSystem : MonoBehaviour, IInputSystem
    {
        public Action OnRight { get; set; }
        public Action OnLeft { get; set; }
        public Action OnUp { get; set; }
        public Action OnDown { get; set; }
        public Action OnSpace { get; set; }
        public Action OnRestart { get; set; }

        public void DirectUpdate()
        {
            ProcessKeyInput(KeyCode.UpArrow, onKey: OnUp);
            ProcessKeyInput(KeyCode.DownArrow, onKey: OnDown);
            ProcessKeyInput(KeyCode.RightArrow, onKey: OnRight);
            ProcessKeyInput(KeyCode.LeftArrow, onKey: OnLeft);
            ProcessKeyInput(KeyCode.Space, onKeyDown: OnSpace);
            ProcessKeyInput(KeyCode.R, onKeyDown: OnRestart);
        }

        public void Reset()
        {
            OnRight = () => { };
            OnLeft = () => { };
            OnUp = () => { };
            OnDown = () => { };
            OnSpace = () => { };
            OnRestart = () => { };
        }

        private void ProcessKeyInput(KeyCode keyCode, Action onKeyDown = null, Action onKey = null, Action onKeyUp = null)
        {
            if (onKeyDown != null && Input.GetKeyDown(keyCode))
            {
                onKeyDown();
            }

            if (onKey != null && Input.GetKey(keyCode))
            {
                onKey();
            }

            if (onKeyUp != null && Input.GetKeyUp(keyCode))
            {
                onKeyUp();
            }
        }
    }
}