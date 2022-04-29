namespace DefaultNamespace.GridSystem
{
    public class BooleanCell : Cell
    {
        [Exposed(ExposedAttribute.ValueType.Boolean)]
        public bool specificVal;
    }
}