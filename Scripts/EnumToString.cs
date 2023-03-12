using System;

public static class EnumToString
{
    public static string ConvertToString(this Enum eff)
    {
        return Enum.GetName(eff.GetType(), eff);
    }
}