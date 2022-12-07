using common;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace day7
{
    public class Day7 : IAdventOfCodeDay
    {
        public Day7(IEnumerable<string> input)
        {
            RootElfFolder = ElfFolder.Parse(input);
        }

        public ElfFolder RootElfFolder { get; }

        public string GetAnswerForPart1()
        {
            var atMost100k = RootElfFolder
                .AllSubFolders()
                .Select(f => (folder: f, totalSize: f.GetTotalSize()))
                .Where(f => f.totalSize <= 100_000);

            return atMost100k.Sum(x => x.totalSize).ToString();
        }

        public string GetAnswerForPart2()
        {
            const int totalDiskSpace = 70000000;
            const int totalFreeSpaceRequired = 30000000;
            var usedDiskSpace = RootElfFolder.GetTotalSize();
            var currentFreeSpace = totalDiskSpace - usedDiskSpace;
            var additionalSpaceRequired = totalFreeSpaceRequired - currentFreeSpace;

            var allFolders = RootElfFolder.AllSubFolders().Concat(new[] { RootElfFolder });

            var result = allFolders
                .Select(f => (folder: f, totalSize: f.GetTotalSize()))
                .Where(f => f.totalSize >= additionalSpaceRequired)
                .MinBy(f => f.totalSize);

            return result.totalSize.ToString();
        }
    }

    public interface FileSystemEntry
    {
        string Name { get; }
    }

    public record ElfFile : FileSystemEntry
    {
        public required int Size { get; init; }
        public required string Name { get; init; }
    }

    public class ElfFolder : FileSystemEntry
    {
        public ElfFolder(ElfFolder Parent, string Name)
        {
            this.Parent = Parent;
            this.Name = Name;
        }

        public ElfFolder()
        {
            Parent = null;
            Name = "/";
        }

        public string Name { get; private set; }
        public ElfFolder? Parent { get; }

        public int GetTotalSize() => Entries.Sum(e => e switch {
                ElfFolder fo => fo.GetTotalSize(),
                ElfFile f => f.Size,
                _ => throw new UnreachableException()
            });

        public IEnumerable<ElfFolder> AllSubFolders()
        {
            for (var i = 0; i < Entries.Count; i++)
            {
                if (Entries[i] is ElfFolder folder)
                {
                    yield return folder;
                    foreach (var f in folder.AllSubFolders())
                    {
                        yield return f;
                    }
                }
            }
        }

        public List<FileSystemEntry> Entries { get; private set; } = new();
        public static ElfFolder Parse(IEnumerable<string> input)
        {
            var content = input.Where(line => !string.IsNullOrEmpty(line)).ToArray();
            var contentIndex = 0;

            var root = new ElfFolder();
            var currentFolder = root;
            while (!DoneParsing())
            {
                var command = content[contentIndex];
                if (command[0] != '$')
                {
                    throw new InvalidDataException($"Expected {command} to be a command.");
                }

                if (IsChangeDirectory(command))
                {
                    var targetFolderName = command[5..];
                    if (targetFolderName == "/")
                    {
                        currentFolder = root;
                    }
                    else if (targetFolderName == "..")
                    {
                        currentFolder = currentFolder.Parent ?? throw new NotSupportedException($"Can't issue cd .. when at root.");
                    }
                    else
                    {
                        var targetFolder = currentFolder
                            .Entries
                            .Find(f => f is ElfFolder folder && folder.Name == targetFolderName) as ElfFolder;

                        currentFolder = targetFolder ?? throw new KeyNotFoundException($"This folder does not contain subfolder {targetFolderName}.");
                    }
                    contentIndex++;
                    continue;
                }
                else if (IsListCommand(command))
                {
                    // we now enumerate everything in this directory
                    contentIndex++;
                    if (DoneParsing())
                    {
                        // there was nothing in this directory and this is the end of the input.
                        continue;
                    }
                    var item = content[contentIndex];

                    while (!IsCommand(item) && !DoneParsing())
                    {
                        var file = FileRegex.Match(item);

                        if (file.Success)
                        {
                            currentFolder!.AddFile(new ElfFile
                            {
                                Name = file.Groups["FileName"].Value,
                                Size = int.Parse(file.Groups["Size"].ValueSpan)
                            });
                        }
                        else if (IsDirectory(item))
                        {
                            currentFolder!.AddFolder(item[4..]);
                        }
                        else
                        {
                            throw new NotSupportedException($"Expected {item} to be either a file or a folder.");
                        }

                        contentIndex++;
                        if (DoneParsing())
                        {
                            continue;
                        }
                        item = content[contentIndex];
                    }
                    continue;
                }
                else
                {
                    throw new NotSupportedException($"Command {command} not supported.");
                }
            }
            return root;

            bool DoneParsing() => contentIndex >= content.Length;
        }

        private void AddFolder(string name)
        {
            Entries.Add(new ElfFolder(this, name));
        }

        private void AddFile(ElfFile elfFile)
        {
            Entries.Add(elfFile);
        }

        public void WriteToStringBuilder(StringBuilder sb, int level = 0)
        {
            sb.AppendLine($"{ new string(' ', level * 2) }- {Name} (dir)");

            foreach (var e in Entries)
            {
                if (e is ElfFolder folder)
                {
                    folder.WriteToStringBuilder(sb, level + 1);
                }
                else if (e is ElfFile file) {
                    sb.AppendLine($"{new string(' ', (level + 1) * 2)}- {file.Name} (file, size={file.Size})");
                }
                else
                {
                    throw new UnreachableException();
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            WriteToStringBuilder(sb, 0);
            return sb.ToString().TrimEnd();
        }

        private static bool IsChangeDirectory(string s) => s.StartsWith("$ cd ");
        private static bool IsListCommand(string s) => s == "$ ls";
        private static bool IsCommand(string s) => s.StartsWith('$');
        private static bool IsDirectory(string s) => s.StartsWith("dir ");
        public static Regex FileRegex = new Regex(@"^(?<Size>\d*) (?<FileName>.*)$");
    }
}