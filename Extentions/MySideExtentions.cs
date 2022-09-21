public static class mySideExtentions
{
    public static bool IsFieldItemOnMySide(this MySide mySide, IFieldItem fieldItem)
    {
        return fieldItem.X > mySide.Left && fieldItem.X < mySide.Right;
    }
}