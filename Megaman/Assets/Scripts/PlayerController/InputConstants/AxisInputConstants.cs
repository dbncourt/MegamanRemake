﻿using System;

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
        public static readonly AxisInputConstants VERTICAL = new AxisInputConstants("Vertical");

        public override string ToString()
        {
            return value;
        }
    }
}
