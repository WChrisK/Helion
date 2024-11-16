﻿using System;

namespace Helion.Util.Container;

public static class DynamicArrayExtensions
{
    public static void FlushReferences<T>(this DynamicArray<T> array) where T : class
    {
        for (int i = 0; i < array.Capacity; i++)
            array[i] = null!;
    }

    public static void FlushStruct<T>(this DynamicArray<T> array) where T : struct
    {
        for (int i = 0; i < array.Capacity; i++)
            array[i] = default;
    }
}
