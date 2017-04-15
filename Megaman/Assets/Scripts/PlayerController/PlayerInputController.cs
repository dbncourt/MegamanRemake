using UnityEngine;
using System.Collections;

namespace PlayerController.InputController
{
    public class PlayerInputController : MonoBehaviour
    {
        public enum KeyStatus
        {
            Down,
            Pressed,
            Up
        }
        public delegate void ActionEvent();
        public delegate void AxisEvent(float value);
        #region Jump Action Events
        private ActionEvent delegateJumpPressedDown;
        private ActionEvent delegateJumpPressed;
        private ActionEvent delegateJumpPressedUp;
        #endregion Jump Action Events
        #region Dash Action Events
        private ActionEvent delegateDashPressedDown;
        private ActionEvent delegateDashPressed;
        private ActionEvent delegateDashPressedUp;
        #endregion Dash Action Events
        #region Horizontal Axis Events
        private AxisEvent delegateHorizontalAxisEvent;
        #endregion Horizontal Axis Events
        #region Vertical Axis Events
        private AxisEvent delegateVerticalAxisEvent;
        #endregion Vertical Axis Events


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown(ActionInputConstants.JUMP.ToString()))
            {
                if (delegateJumpPressedDown != null)
                {
                    delegateJumpPressedDown();
                }
            }

            if (Input.GetButton(ActionInputConstants.JUMP.ToString()))
            {
                if (delegateJumpPressed != null)
                {
                    delegateJumpPressed();
                }
            }
            if (Input.GetButtonUp(ActionInputConstants.JUMP.ToString()))
            {
                if (delegateJumpPressedUp != null)
                {
                    delegateJumpPressedUp();
                }
            }

            if (Input.GetButtonDown(ActionInputConstants.DASH.ToString()))
            {
                if (delegateDashPressedDown != null)
                {
                    delegateDashPressedDown();
                }
            }
            if (Input.GetButton(ActionInputConstants.DASH.ToString()))
            {
                if (delegateDashPressed != null)
                {
                    delegateDashPressed();
                }
            }
            if (Input.GetButtonUp(ActionInputConstants.DASH.ToString()))
            {
                if (delegateDashPressedUp != null)
                {
                    delegateDashPressedUp();
                }
            }
        }

        void FixedUpdate()
        {
            if (delegateHorizontalAxisEvent != null)
            {
                delegateHorizontalAxisEvent(Input.GetAxis(AxisInputConstants.HORIZONTAL.ToString()));
            }
            if (delegateVerticalAxisEvent != null)
            {
                delegateVerticalAxisEvent(Input.GetAxis(AxisInputConstants.VERTICAL.ToString()));
            }
        }

        public void BindAxis(AxisInputConstants axisConstant, AxisEvent delegateEvent)
        {
            if (axisConstant == AxisInputConstants.HORIZONTAL)
            {
                delegateHorizontalAxisEvent += delegateEvent;
            }
            else if (axisConstant == AxisInputConstants.VERTICAL)
            {
                delegateVerticalAxisEvent += delegateEvent;
            }
        }

        public void BindAction(ActionInputConstants actionConstant, ActionEvent delegateEvent, PlayerInputController.KeyStatus keyStatus)
        {
            if (actionConstant == ActionInputConstants.JUMP)
            {
                switch (keyStatus)
                {
                    case KeyStatus.Down:
                        {
                            delegateJumpPressedDown += delegateEvent;
                            break;
                        }
                    case KeyStatus.Pressed:
                        {
                            delegateJumpPressed += delegateEvent;
                            break;
                        }
                    case KeyStatus.Up:
                        {
                            delegateJumpPressedUp += delegateEvent;
                            break;
                        }
                }
            }
            else if (actionConstant == ActionInputConstants.DASH)
            {
                switch (keyStatus)
                {
                    case KeyStatus.Down:
                        {
                            delegateDashPressedDown += delegateEvent;
                            break;
                        }
                    case KeyStatus.Pressed:
                        {
                            delegateDashPressed += delegateEvent;
                            break;
                        }
                    case KeyStatus.Up:
                        {
                            delegateDashPressedUp += delegateEvent;
                            break;
                        }
                }
            }
        }
    }
}