using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayerController.InputController
{
    public sealed class AxisInputConstants
    {
        private readonly String value;

        private AxisInputConstants(string value)
        {
            this.value = value;
        }

        public static readonly AxisInputConstants HORIZONTAL = new AxisInputConstants("Horizontal");

        public override string ToString()
        {
            return this.value;
        }
    }
}
