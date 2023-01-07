#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Utility.Editor
{
    public static class ObjectExtentions
    {
        const string CLASS = "--- !u!114";
        const string NAME = "  m_Name";

        public static void RemoveAssets(this UnityEngine.Object obj, List<string> exclusions, bool refreshDatabase = true)
        {
            string objectName = obj.name;
            string path = new DirectoryInfo(AssetDatabase.GetAssetPath(obj)).FullName;
            string[] lines = File.ReadAllLines(path);
            List<string> newLines = new();
            bool anyDeleted = false;

            int i = AddLines(0, true);
            while (i < lines.Length)
            {
                i = AddLines(i, Contains(i));
            }

            if (anyDeleted)
            {
                File.WriteAllLines(path, newLines);
                if (refreshDatabase)
                {
                    AssetDatabase.Refresh();
                }
            }

            int AddLines(int i, bool add)
            {
                if (add)
                {
                    newLines.Add(lines[i]);
                    return AddLines(i + 1);
                }

                i++;
                while (i < lines.Length)
                {
                    string line = lines[i];
                    if (line.StartsWith(CLASS))
                    {
                        break;
                    }

                    i++;
                }
                return i;

                int AddLines(int i)
                {
                    while (i < lines.Length)
                    {
                        string line = lines[i];
                        if (line.StartsWith(CLASS))
                        {
                            break;
                        }

                        newLines.Add(line);
                        i++;
                    }
                    return i;
                }
            }


            bool Contains(int i)
            {
                while (true)
                {
                    string line = lines[i];

                    if (line.StartsWith(NAME))
                    {
                        ReadOnlySpan<char> span = line;
                        ReadOnlySpan<char> name = span[(span.IndexOf(':') + 2)..];
                        string nameStr = name.ToString();
                        bool hasName = nameStr == objectName || ((IList<string>)exclusions).Contains(nameStr);

                        if (!hasName)
                        {
                            anyDeleted = true;
                        }

                        return hasName;
                    }
                    i++;
                }
            }
        }
    }
}
#endif
