using System;
using System.Diagnostics;
using System.IO;

namespace GeoCybernetica.CodeGenerationExtention.Files
{
    /// <summary>
    /// Base
    /// </summary>
    public static class PackageEnvironment
    {
        private const string VisualStudioFolderName = "Visual Studio 2013";

        private const string PackageFolderName = "T4 ClassTemplates";

        public static string CurrentDirectory { get; private set; }

        public const string ClassFile = "Class.txt";

        public const string ScriptDirectoryName = "Script";

        public const string IgnoreListFile = "IgnoreList.txt";

        public const string ReadOnlyFile = "ReadOnlyList.txt";

        public const string LogFile = "Log.txt";

        public static string IgnoreListFullPath
        {
            get { return Path.Combine(CurrentDirectory, IgnoreListFile); }
        }

        public static string ReadOnlyFileFullPath { get { return Path.Combine(CurrentDirectory, IgnoreListFile); } }

        public static string LogFileFullPath { get { return Path.Combine(CurrentDirectory, LogFile); } }

        public static string ClassFileFullPath { get { return Path.Combine(CurrentDirectory, ClassFile); } }


        public static string ScriptDirectoryFullPath { get { return Path.Combine(CurrentDirectory, ScriptDirectoryName); } }

        static PackageEnvironment()
        {
            InspectDirectory();
            InspectFiles();
        }

        private static void InspectFiles()
        {
            RecreateIfNeeded(IgnoreListFullPath);
            RecreateIfNeeded(ReadOnlyFileFullPath);
            RecreateIfNeeded(LogFileFullPath, true);
            RecreateIfNeeded(ClassFileFullPath);
            RecreateScriptDirectoryIfNeeded();
        }

        private static void RecreateScriptDirectoryIfNeeded()
        {
            if (!Directory.Exists(ScriptDirectoryFullPath))
            {
                Directory.CreateDirectory(ScriptDirectoryFullPath);
            }
        }

        private static void RecreateIfNeeded(string fullPath, bool isNeedToRemoveOldFile = false)
        {
            try
            {
                if (isNeedToRemoveOldFile && File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                if (!File.Exists(fullPath))
                {
                    var sw = File.CreateText(fullPath);
                    sw.Close();
                }
            }
            catch (Exception)
            {
                Debug.WriteLine(string.Format("Can't create {0} list.", fullPath));
            }
        }

        /// <summary>
        /// Check documents, vs and existing.
        /// </summary>
        private static void InspectDirectory()
        {
            try
            {
                var path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents");
                if (Directory.Exists(Path.Combine(path, VisualStudioFolderName)))
                    path = Path.Combine(path, VisualStudioFolderName);

                CurrentDirectory = CreateNewDirecotyIfNeeded(path);
            }
            catch (Exception)
            {
            }
        }

        private static string CreateNewDirecotyIfNeeded(string path)
        {
            var resultFolder = Path.Combine(path, PackageFolderName);
            if (!Directory.Exists(resultFolder))
            {
                Directory.CreateDirectory(resultFolder);
            }
            return resultFolder;
        }
    }
}