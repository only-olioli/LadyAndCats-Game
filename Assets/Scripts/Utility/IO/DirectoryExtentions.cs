using System.IO;

namespace Utility.IO
{
    public static class DirectoryExtentions
    {
        public static bool TryCreate(string path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }
            Directory.CreateDirectory(path);
            return true;
        }

        public static bool TryCreate(DirectoryInfo info)
        {
            if (info.Exists)
            {
                return false;
            }
            Directory.CreateDirectory(info.FullName);
            return true;
        }
    }
}
