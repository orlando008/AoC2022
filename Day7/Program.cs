using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ElfDir> fileSystem = BuildFileSystem();
            List<ElfDir> foundDirs = new List<ElfDir>();
            GetDirsWithSizeUpTo(100000, fileSystem[0], foundDirs);
            int totalSize = foundDirs.Sum(d => d.GetSize());
            Console.WriteLine(totalSize.ToString());

            int currentTotalSize = fileSystem[0].GetSize();
            int totalNeededSpace = 30000000;
            int totalDriveSize = 70000000;
            int totalFreeSpace = totalDriveSize - currentTotalSize;
            int totalWeNeedToDelete = totalNeededSpace - totalFreeSpace;
            foundDirs.Clear();
            GetDirsWithSizeAtLeast(totalWeNeedToDelete, fileSystem[0], foundDirs);
            ElfDir d = foundDirs.OrderBy(d => d.GetSize()).First();
            Console.WriteLine(d.GetSize());
            
        }

        static List<ElfDir> BuildFileSystem()
        {
            List<string> listOfOutput = new List<string>();
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    listOfOutput.Add(sr.ReadLine());
                }    
            }
            List<ElfDir> fileSystem = new List<ElfDir>();
            fileSystem.Add(new ElfDir("ROOT", null));

            ElfDir currentDir = fileSystem[0];

            foreach (string command in listOfOutput)
            {
               
                if(command == "$ cd /")
                {
                    currentDir = fileSystem[0];
                }
                else if (command.StartsWith("dir"))
                {
                    string desiredDir = command.Substring(3).Trim();
                    ElfDir foundDir = GetDirWithName(desiredDir, currentDir);
                    if(foundDir == null)
                    {
                        currentDir.SubDirectories.Add(new ElfDir(desiredDir, currentDir));
                    }
                }
                else if (command == "$ ls")
                {
                    continue;
                }
                else if (command == "$ cd ..")
                {
                    currentDir = currentDir.ParentDir;
                }
                else if (command.StartsWith("$ cd"))
                {
                    currentDir = GetDirWithName(command.Split(" ")[2].Trim(), currentDir);
                }
                else
                {
                    currentDir.SubFiles.Add(new ElfFile(command.Split(" ")[1].Trim(), command.Split(" ")[0].Trim(), currentDir));
                }
            }

            return fileSystem;
        }

        static ElfDir GetDirWithName(string name, ElfDir fileSystem)
        {
            ElfDir dir = fileSystem.SubDirectories.FirstOrDefault(dir => dir.Name == name);
            if(dir != null)
            {
                return dir;
            }
            else
            {
                foreach(ElfDir d in fileSystem.SubDirectories)
                {
                    return GetDirWithName(name, d);
                }
            }
            return null;
        }

        static void GetDirsWithSizeUpTo(int sizeUpTo, ElfDir fileSystem, List<ElfDir> allFoundDirs)
        {
            if(fileSystem.GetSize() <= sizeUpTo)
            {
                allFoundDirs.Add(fileSystem);
            }

            foreach (ElfDir subDir in fileSystem.SubDirectories)
            {
                GetDirsWithSizeUpTo(sizeUpTo, subDir, allFoundDirs);
            }
        }

        static void GetDirsWithSizeAtLeast(int size, ElfDir fileSystem, List<ElfDir> allFoundDirs)
        {
            if (fileSystem.GetSize() >= size)
            {
                allFoundDirs.Add(fileSystem);
            }

            foreach (ElfDir subDir in fileSystem.SubDirectories)
            {
                GetDirsWithSizeAtLeast(size, subDir, allFoundDirs);
            }
        }

        class ElfDir
        {
            public string Name;
            public List<ElfDir> SubDirectories;
            public List<ElfFile> SubFiles;
            public ElfDir ParentDir;

            public ElfDir(string name, ElfDir parentDir)
            {
                Name = name;
                SubDirectories = new List<ElfDir>();
                SubFiles = new List<ElfFile>();
                ParentDir = parentDir;
            }

            public string GetConsoleOutput()
            {
                string s = "";
                foreach(ElfDir d in SubDirectories)
                {
                    if(d.ParentDir == null)
                    {
                        s += "/";
                    }
                    else
                    {
                        s += d.ParentDir.Name + "/";
                    }    
                    s += d.Name + System.Environment.NewLine;
                    s += d.GetConsoleOutput();
                }

                foreach(ElfFile f in SubFiles)
                {
                    s += f.ParentDir.Name + "/" + f.Name + " " + f.Size + System.Environment.NewLine;
                }

                
                return s;
            }

            public int GetSize()
            {
                return this.SubDirectories.Sum(d => d.GetSize()) + this.SubFiles.Sum(f => f.GetSize());
            }
        }

        class ElfFile
        {
            public string Name;
            public string Size;
            public ElfDir ParentDir;

            public ElfFile(string name, string size, ElfDir parentDir)
            {
                Name = name;
                Size = size;
                ParentDir = parentDir;
            }

            public int GetSize()
            {
                return Convert.ToInt32(Size);
            }
        }
    }
}
