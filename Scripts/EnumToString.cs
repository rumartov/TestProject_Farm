using System;

namespace DefaultNamespace
{
    public static class EnumToString
    {
        public static String ConvertToString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }

    }
}