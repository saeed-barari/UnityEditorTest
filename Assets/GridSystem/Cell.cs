using System;

namespace DefaultNamespace.GridSystem
{
    [System.Serializable]
    public class Cell
    {
        [AttributeUsage(AttributeTargets.Field)]
        public class ExposedAttribute : Attribute
        {
            public ValueType valueType;
            public bool isDetail;

            public enum ValueType
            {
                Float, String, Int, Boolean
            }
            public ExposedAttribute(ValueType valueType, bool isDetail = false)
            {
                this.valueType = valueType;
                this.isDetail = isDetail;
            }
        }
        
        [Exposed(ExposedAttribute.ValueType.Int)]
        public int power;
        [Exposed(ExposedAttribute.ValueType.Float)]
        public float height;
        [Exposed(ExposedAttribute.ValueType.String)]
        public string tag;

        [Exposed(ExposedAttribute.ValueType.Int, true)]
        public int detailedInt;
        [Exposed(ExposedAttribute.ValueType.Int, true)]
        public int detailedInt2;
        [Exposed(ExposedAttribute.ValueType.Int, true)]
        public int detailedInt3;
    }

}