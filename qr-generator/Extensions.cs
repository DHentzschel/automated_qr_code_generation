using System;

namespace qr_generator
{
    public static class Extensions
    {
        public static bool TryParseEnum<T>(this string input, out T result) where T : struct, Enum => Enum.TryParse(input, out result);
    }
}