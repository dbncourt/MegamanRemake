using UnityEngine;

namespace PlayerController.InputController
{
    public class Key
    {
        public delegate void ActionEvent();
        private ActionEvent keyDown;
        private ActionEvent keyPressed;
        private ActionEvent keyUp;

        public struct KeyStatus
        {
            public bool isDown;
            public bool isPressed;
            public bool isUp;
        }

        public enum EKeyStatus
        {
            Down,
            Pressed,
            Up
        }

        private string keyName;
        public KeyStatus inputKeyStatus;

        public Key(string keyName)
        {
            this.keyName = keyName;
        }

        public void BindAction(ActionEvent keyEvent, EKeyStatus keyStatus)
        {
            switch (keyStatus)
            {
                case EKeyStatus.Down:
                    {
                        keyDown += keyEvent;
                        break;
                    }
                case EKeyStatus.Pressed:
                    {
                        keyPressed += keyEvent;
                        break;
                    }
                case EKeyStatus.Up:
                    {
                        keyUp += keyEvent;
                        break;
                    }
            }
        }

        public void EvaluateActionKey()
        {
            if (Input.GetButtonDown(keyName))
            {
                inputKeyStatus.isDown = true;
                if (keyDown != null)
                {
                    keyDown();
                }
            }
            else
            {
                inputKeyStatus.isDown = false;
            }

            if (Input.GetButton(keyName))
            {
                inputKeyStatus.isPressed = true;
                if (keyPressed != null)
                {
                    keyPressed();
                }
            }
            else
            {
                inputKeyStatus.isPressed = false;
            }

            if (Input.GetButtonUp(keyName))
            {
                inputKeyStatus.isUp = true;
                if (keyUp != null)
                {
                    keyUp();
                }
            }
            else
            {
                inputKeyStatus.isUp = false;
            }
        }
    }
}