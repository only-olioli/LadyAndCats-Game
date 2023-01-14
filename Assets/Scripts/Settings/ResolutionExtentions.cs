using System;
using UnityEngine;

namespace Settings
{
    public static class ResolutionExtentions
    {
        public static bool Compare(this UnityEngine.Resolution resolution, UnityEngine.Resolution other)
        {
            return resolution.width == other.width &&
                resolution.height == other.height;
        }

        public static int Index(this UnityEngine.Resolution resolution)
        {
            UnityEngine.Resolution[] resolutions = Screen.resolutions;
            for (int i = 0; i < resolutions.Length; i++)
            {
                var current = resolutions[i];
                if (current.Compare(resolution))
                {
                    return i;
                }
            }
            return resolutions.Length - 1;
        }

        public static bool TryParse(string str, out UnityEngine.Resolution resolution)
        {
            resolution = default;
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            ReadOnlySpan<char> span = str;
            int xIndex = span.IndexOf('x');
            string widthStr = span[..xIndex].ToString();
            if (!int.TryParse(widthStr, out int width))
            {
                return false;
            }

            string heightStr = span.Slice(xIndex + 1).ToString();
            if (int.TryParse(heightStr, out int height))
            {
                resolution = new()
                {
                    width = width,
                    height = height
                };
                return true;
            }
            return false;
        }
    }
}
