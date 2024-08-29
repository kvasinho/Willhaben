namespace Willhaben.Domain.Utils;

public static class EnumExtensions
{
    public static int GetValue(this Enum @enum)
    {
        return Convert.ToInt32(@enum);
    }
    
}