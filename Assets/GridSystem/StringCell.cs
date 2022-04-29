namespace DefaultNamespace.GridSystem
{
    public class StringCell : Cell
    {
        [Exposed(ExposedAttribute.ValueType.String)]
        public string specificVal;
    }
}