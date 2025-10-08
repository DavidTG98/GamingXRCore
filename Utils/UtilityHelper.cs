using System;
using UnityEngine;

public static class UtilityHelper
{
    public static int GetEnumIndexByName<TEnum>(this string name) where TEnum : Enum
    {
        var names = Enum.GetNames(typeof(TEnum));
        for (int i = 0; i < names.Length; i++)
        {
            if (string.Equals(names[i], name, StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return 0;
    }
}
