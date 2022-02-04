﻿using System;
using System.Linq;

namespace BPOBackend
{
    public static class MyStringExtension
    {
        public static string WithMaxLength(this string value, int maxLength)
        {
            return value?.Substring(0, Math.Min(value.Length, maxLength));
        }
        public static string GetFileFormat(this string value)
        {
            string filename=value.Split('/').LastOrDefault();
            return filename.Split('.').LastOrDefault();

        }
        public static string GetFileName(this string value)
        {
            string filename = value.Split('/').LastOrDefault();
            return filename.Split('.').FirstOrDefault();

        }
    }
}
