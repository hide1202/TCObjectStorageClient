using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.IO.Path;

namespace TCObjectStorageClient.Models
{
    interface IDiskEntity
    {
        string Path { get; }

        bool IsDirectory { get; }

        List<IDiskEntity> Children { get; }
    }

    static class DiskEntityUtility
    {
        public static bool IsDirectory(string path)
        {
            var attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) != 0;
        }
    }

    class DirectoryEntity : IDiskEntity
    {
        public string Path { get; }
        public bool IsDirectory { get; } = true;
        public List<IDiskEntity> Children { get; } = new List<IDiskEntity>();

        public DirectoryEntity(string path)
        {
            Path = path;

            var info = new DirectoryInfo(path);
            var dirInfos = info.GetDirectories();
            var fileInfos = info.GetFiles();

            dirInfos.ForEach(i => Children.Add(new DirectoryEntity(i.FullName)));
            fileInfos.ForEach(i => { if(!i.Attributes.HasFlag(FileAttributes.Hidden)) Children.Add(new FileEntity(i.FullName)); });
        }

        public IEnumerable<(string parent, string pathFromBase, IDiskEntity entity)> GetAllChildren()
        {
            var basePath = Path;

            var enumerable = ForEachInternal(basePath.Trim(DirectorySeparatorChar), this);
            foreach (var el in enumerable)
                yield return el;
        }

        private IEnumerable<(string parent, string pathFromBase, IDiskEntity entity)> ForEachInternal(string basePath, IDiskEntity entity)
        {
            foreach (var child in entity.Children)
            {
                if (child.IsDirectory)
                {
                    var enumerable = ForEachInternal(basePath, child);
                    foreach (var e in enumerable)
                        yield return e;
                }
                else
                {
                    var parentEndIndex = child.Path.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                    var parentStartIndex = child.Path.Substring(0, parentEndIndex).LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                    var parent = child.Path.Substring(parentStartIndex + 1, parentEndIndex - parentStartIndex - 1);
                    yield return (parent, child.Path.Substring(basePath.Length + 1), child);
                }
            }
        }
    }

    class FileEntity : IDiskEntity
    {
        public string Path { get; }
        public bool IsDirectory { get; } = false;
        public List<IDiskEntity> Children { get; } = null;

        public FileEntity(string path)
        {
            Path = path;
        }
    }
}