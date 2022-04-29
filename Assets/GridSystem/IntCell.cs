namespace DefaultNamespace.GridSystem
{
    public class IntCell : Cell
    {
        [Cell.ExposedAttribute(Cell.ExposedAttribute.ValueType.Int)]
        public int specificVal;
    }
}