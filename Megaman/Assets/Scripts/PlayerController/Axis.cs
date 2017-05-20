using UnityEngine;

namespace PlayerController.InputController
{
    public class Axis
    {
        public delegate void AxisEvent(float value);
        private AxisEvent axisEvent;

        private string axisName;

        public Axis(string axisName)
        {
            this.axisName = axisName;
        }

        public void EvaluateAxisEvent()
        {
            if(axisEvent != null)
            {
                float value = Input.GetAxisRaw(axisName);
                axisEvent(value);
            }
        }

        public void BindAxis(AxisEvent axisEvent)
        {
            this.axisEvent += axisEvent;
        }
    }
}
