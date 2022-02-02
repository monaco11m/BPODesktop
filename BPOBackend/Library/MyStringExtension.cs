﻿using System;

namespace BPOBackend
{
    public static class MyStringExtension
    {
        public static string WithMaxLength(this string value, int maxLength)
        {
            return value?.Substring(0, Math.Min(value.Length, maxLength));
        }
    }
}
