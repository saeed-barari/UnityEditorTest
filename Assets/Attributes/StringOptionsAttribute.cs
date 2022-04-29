using System;
using UnityEngine;

namespace DefaultNamespace
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StringOptionsAttribute : PropertyAttribute
    {
        public string methodName;
        public StringOptionsAttribute(string listMethodName)
        {
            methodName = listMethodName;
        }
    }
}