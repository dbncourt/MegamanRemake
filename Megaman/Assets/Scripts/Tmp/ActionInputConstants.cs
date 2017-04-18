using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayerController.InputController
{
    public sealed class ActionInputConstants
    {
        private readonly String value;

        private ActionInputConstants(string value)
        {
            this.value = value;
        }

        public static readonly ActionInputConstants JUMP = new ActionInputConstants("Jump");
        public static readonly ActionInputConstants DASH = new ActionInputConstants("Dash");

        public override string ToString()
        {
            return this.value;
        }
    }
}