using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpicyConsole.Setup {

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SpicyCommandAttribute : Attribute
    {
        public string CustomName { get; }

        public SpicyCommandAttribute(string customName)
        {
            CustomName = customName;
        }
    }

}