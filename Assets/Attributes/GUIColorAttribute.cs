using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class GUIColorAttribute : PropertyAttribute
    {
        public Color color;
        public GUIColorAttribute(float red, float green, float blue, float alpha = 1)
        {
            color = new Color(red, green, blue, alpha);
        }
    }
}