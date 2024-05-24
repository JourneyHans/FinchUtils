using System;
using System.IO;
using UnityEngine;

namespace HzFramework.Common {
    public static class PathUtils {
        #region Temporary路径

        public static string TemporaryPath {
            get {
#if UNITY_EDITOR
                // 电脑上的Temporary路径太远了，用自定义的Assets同目录下的Temporary路径
                return $"{Directory.GetParent(Application.dataPath).FullName}/Temporary";
#else
                return Application.temporaryCachePath;
#endif
            }
        }

        public static string ReadTemporaryText(string path) {
            return SafeReadAllText($"{TemporaryPath}/{path}");
        }

        public static byte[] ReadTemporaryBytes(string path) {
            return SafeReadAllBytes($"{TemporaryPath}/{path}");
        }

        public static void DeleteTemporaryFile(string path) {
            SafeDeleteFile($"{PersistentPath}/{path}");
        }

        #endregion

        #region Persistent路径

        public static string PersistentPath {
            get {
#if UNITY_EDITOR
                // 电脑上的Persistent路径太远了，用自定义的Assets同目录下的Cache路径
                return $"{Directory.GetParent(Application.dataPath).FullName}/Cache";
#else
                return Application.persistentDataPath;
#endif
            }
        }

        public static string ReadPersistentText(string path) {
            return SafeReadAllText($"{PersistentPath}/{path}");
        }

        public static byte[] ReadPersistentBytes(string path) {
            return SafeReadAllBytes($"{PersistentPath}/{path}");
        }

        public static void DeletePersistentFile(string fileName) {
            SafeDeleteFile($"{PersistentPath}/{fileName}");
        }

        #endregion

        #region StreamingAssets路径

        public static byte[] ReadStreamingAssetsBytes(string path) {
#if UNITY_ANDROID
            var www = new WWW($"{Application.streamingAssetsPath}/{path}"); 
            while (!www.isDone) {} 
            return www.bytes;
#else
            return SafeReadAllBytes($"{Application.streamingAssetsPath}/{path}");
#endif
        }

        public static string ReadStreamingAssetsText(string path) {
#if UNITY_ANDROID
            var www = new WWW($"{Application.streamingAssetsPath}/{path}"); 
            while (!www.isDone) { } 
            return www.text;
#else
            return SafeReadAllText($"{Application.streamingAssetsPath}/{path}");
#endif
        }

        #endregion

        private static byte[] SafeReadAllBytes(string path) {
            try {
                return File.Exists(path) ? File.ReadAllBytes(path) : null;
            }
            catch (Exception e) {
                Debug.LogError(e.ToString());
            }

            return null;
        }

        public static bool SafeWriteAllBytes(string path, byte[] bytes) {
            if (!EnsureParentFolder(path)) {
                return false;
            }

            File.WriteAllBytes(path, bytes);
            return true;
        }

        public static string SafeReadAllText(string path) {
            try {
                return File.Exists(path) ? File.ReadAllText(path) : null;
            }
            catch (Exception e) {
                Debug.LogError(e.ToString());
            }

            return null;
        }

        public static bool SafeWriteAllText(string path, string text) {
            if (!EnsureParentFolder(path)) {
                return false;
            }

            File.WriteAllText(path, text);
            return true;
        }

        public static bool EnsureParentFolder(string path) {
            DirectoryInfo dir = Directory.GetParent(path);
            if (dir == null) {
                return false;
            }

            if (!dir.Exists) {
                dir.Create();
            }

            return true;
        }

        public static void SafeDeleteFile(string path) {
            try {
                if (File.Exists(path)) {
                    File.Delete(path);
                }
            }
            catch (Exception e) {
                Debug.LogError(e.ToString());
            }
        }

        public static void SafeCopyDir(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite) {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists) {
                Debug.LogError($"Source directory does not exist or could not be found: {sourceDirName}");
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, overwrite);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    SafeCopyDir(subdir.FullName, tempPath, copySubDirs, overwrite);
                }
            }
        }

        public static DirectoryInfo DeleteAndCreate(string directoryPath) {
            if (Directory.Exists(directoryPath)) {
                Directory.Delete(directoryPath, true);
            }

            return Directory.CreateDirectory(directoryPath);
        }

        /// <summary>
        /// 文件夹拷贝，只考虑了第一层，后续有需求再扩展递归
        /// 拷贝方式，会先确认目标文件夹被清空，然后再拷贝
        /// </summary>
        /// <param name="originDir"></param>
        /// <param name="destinationDir"></param>
        public static void CopyDirectory(string originDir, string destinationDir) {
            DeleteAndCreate(destinationDir);

            string[] originFiles = Directory.GetFiles(originDir);
            foreach (string originFile in originFiles) {
                string destinationFile = Path.Combine(destinationDir, Path.GetFileName(originFile));
                File.Copy(originFile, destinationFile, true);
            }
        }
    }
}