using System.IO;

namespace Utility.IO
{
    public static class FileExtentions
    {
        public static string FindFilePath(string fileName, string startDirectory)
        {
            foreach (string file in Directory.GetFiles(startDirectory))
            {
                if (file.EndsWith(fileName))
                {
                    return file;
                }
            }

            foreach (string subDirectory in Directory.GetDirectories(startDirectory))
            {
                string result = FindFilePath(fileName, subDirectory);
                if (result is not null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
