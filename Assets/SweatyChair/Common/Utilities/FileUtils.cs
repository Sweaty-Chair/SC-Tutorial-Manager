using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SweatyChair
{

	public static class FileUtils
	{
		private static string key;

		#region Delete

		public static void Delete(string path)
		{
			if (Directory.Exists(path)) {
				DeleteDirectory(path);
			} else if (File.Exists(path)) {
				DeleteFile(path);
			} else {
				Debug.LogFormat("FileHelper:Delete - Can't find folder/file at {0}", path);
			}
		}

		public static void DeleteDirectory(string path)
		{
			DirectoryInfo dirDI = new DirectoryInfo(path);
			if (dirDI != null) {
				if (dirDI.GetFiles().Length == 0 && dirDI.GetDirectories().Length == 0) {
					ForceDeleteDirectory(path);
					return;
				}
				foreach (string entry in Directory.GetFileSystemEntries(path)) {
					if (File.Exists(entry)) {
						DeleteFile(entry);
					} else {
						DirectoryInfo di = new DirectoryInfo(entry);
						if (di.GetFiles().Length != 0 || di.GetDirectories().Length != 0)
							DeleteDirectory(di.FullName); // Delete the sub-folders recursively
						ForceDeleteDirectory(entry);
					}
				}
				ForceDeleteDirectory(path); // Delete self
			} else {
				Debug.LogWarningFormat("FileHelper:DeleteDirectory - Can't find folder at {0}", path);
			}
		}

		private static void ForceDeleteDirectory(string path, bool showWarning = false) // Delete a folder even if it is readonly
		{
			try {
				DirectoryInfo di = new DirectoryInfo(path);
				if (di != null) {
					if (di.Attributes.ToString().IndexOf("ReadOnly") != -1)
						di.Attributes = FileAttributes.Normal;
					Directory.Delete(path);
				} else if (showWarning) {
					Debug.LogWarningFormat("FileHelper:ForceDeleteDirectory - Can't find folder at {0}", path);
				}
			} catch (DirectoryNotFoundException e) {
				if (showWarning)
					Debug.LogWarningFormat("FileHelper:ForceDeleteDirectory - Path: {1}, Error: {0}", path, e);
			}
		}

		public static bool IsWritable(string path)
		{
			if (File.Exists(path)) {
				FileInfo fi = new FileInfo(path);
				if (fi.Attributes.ToString().IndexOf("ReadOnly") == -1)
					return true;
			}
			return false;
		}

		public static void UnsetReadOnly(string path, bool showWarning = true)
		{
			if (File.Exists(path)) {
				FileInfo fi = new FileInfo(path);
				if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1) {
					fi.Attributes = FileAttributes.Normal;
					if (showWarning)
						Debug.LogWarningFormat("FileHelper:UnsetReadOnly - File is read only, make sure to checkout in perforce first, path={0}", path);
				}
			} else if (showWarning) {
				Debug.LogWarningFormat("FileHelper:UnsetReadOnly - Can't find file at {0}", path);
			}
		}

		public static void DeleteFile(string path, bool showWarning = false)
		{
			if (File.Exists(path)) {
				FileInfo fi = new FileInfo(path);
				if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
					fi.Attributes = FileAttributes.Normal;
				File.Delete(path);
			} else if (showWarning) {
				Debug.LogWarningFormat("FileHelper:DeleteFile - Can't find file at {0}", path);
			}
		}

		public static void DeletePersistentDataFile(string filename, bool showWarning = false)
		{
			DeleteFile(Path.Combine(Application.persistentDataPath, filename), showWarning);
		}

		// Empty a folder excluding a list sub-folder/files
		public static void EmptyDirectory(string path, params string[] excludes)
		{
			List<string> excludesList = new List<string>();
			if (excludes != null && excludes.Length > 0)
				excludesList = new List<string>(excludes);
			foreach (string entry in Directory.GetFileSystemEntries (path)) {
				if (File.Exists(entry)) {
					if (excludesList.Contains(Path.GetFileName(entry)))
						continue;
					DeleteFile(entry);
				} else { // Directory
					if (excludesList.Contains(Path.GetDirectoryName(entry)))
						continue;
					DirectoryInfo di = new DirectoryInfo(entry);
					if (di.GetFiles().Length != 0 || di.GetDirectories().Length != 0)
						DeleteDirectory(di.FullName); // Delete the sub-folders recursively
					ForceDeleteDirectory(entry);
				}
			}
		}

		#endregion

		#region Copy

		public static void Copy(string src, string dst)
		{
			if (Directory.Exists(src)) {
				CopyDirectory(src, dst);
			} else if (File.Exists(src)) {
				CopyFile(src, dst);
			} else {
				Debug.LogWarningFormat("BuildHelper:Copy - Can't find source folder/file at {0}", src);
			}
		}

		public static void CopyDirectory(string src, string dst)
		{
			DirectoryInfo sourceDI = new DirectoryInfo(src);
			if (!sourceDI.Exists) {
				Debug.LogWarningFormat("BuildHelper:CopyDirectory - Can't find source folder {0}", src);
				return;
			}

			Directory.CreateDirectory(dst);
			foreach (FileSystemInfo fsi in sourceDI.GetFileSystemInfos ()) {
				string targetPath = Path.Combine(dst, fsi.Name);
				if (fsi is FileInfo) {
					// Skip the system useless files
					if (Path.GetFileName(fsi.FullName) == ".DS_Store")
						continue;
					DeleteFile(targetPath, false); // Overwrite old file if any, delete the old one first
					File.Copy(fsi.FullName, targetPath);
				} else {
					Directory.CreateDirectory(targetPath);
					CopyDirectory(fsi.FullName, targetPath);	// Recursively copy files and sub folders
				}
			}
		}

		public static void CopyFile(string sourcePath, string destinationPath)
		{
			if (File.Exists(sourcePath)) {
				File.Copy(sourcePath, destinationPath);
			} else {
				Debug.LogWarningFormat("BuildHelper:CopyFile - Can't find source folder/file at {0}", sourcePath);
			}
		}

		#endregion

		#region Rename

		public static void Rename(string src, string dst)
		{
			if (Directory.Exists(src)) {
				if (Directory.Exists(dst))
					DeleteDirectory(dst);
				Directory.Move(src, dst);
			} else if (File.Exists(src)) {
				File.Move(src, dst);
			} else {
				Debug.LogWarningFormat("BuildHelper:Rename - Can't find source file {0}", src);
			}
		}

		#endregion

		#region Count

		public static int GetDirectoryCount(string path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			if (di != null)
				return di.GetDirectories().Length;
			return 0;
		}

		#endregion

		#region Image

		// filePath example: /User/ABC/image.jpg
		public static void SaveImage(Texture2D texture, string filePath)
		{
			if (string.IsNullOrEmpty(filePath)) {
				Debug.LogErrorFormat("FileUtils:SaveImage - filePath cannot be empty");
				return;
			}
			string filePathLower = filePath.ToLower();
			bool hasAlpha = !filePathLower.Contains(".jpg") && !filePathLower.Contains("jpeg");
			File.WriteAllBytes(filePath, hasAlpha ? texture.EncodeToPNG() : texture.EncodeToJPG());
		}

		// filename example: image.jpg, image.png
		public static void SavePersistentDataImage(Texture2D texture, string filename)
		{
			if (string.IsNullOrEmpty(filename)) {
				Debug.LogErrorFormat("FileUtils:SavePersistentDataImage - filename cannot be empty");
				return;
			}
			SaveImage(texture, Path.Combine(Application.persistentDataPath, filename));
		}

		// filename example: image.jpg, image.png
		// key example: ScreenshotImage
		public static void SavePlayerPrefsImage(Texture2D texture, string filename, string key)
		{
			if (string.IsNullOrEmpty(filename)) {
				Debug.LogErrorFormat("FileUtils:SavePlayerPrefsImage - filename cannot be empty");
				return;
			}
			PlayerPrefs.SetString(key, filename);
			SavePersistentDataImage(texture, PlayerPrefs.GetString(key));
		}

		// filename example: image.jpg, image.png --> will saved as image001.jpg, image002.jpg, etc
		// key example: ScreenshotImage
		public static void SavePlayerPrefsImages(Texture2D[] textures, string filename, string key)
		{
			if (string.IsNullOrEmpty(filename)) {
				Debug.LogErrorFormat("FileUtils:SavePlayerPrefsImages - filename cannot be empty");
				return;
			}
			int lastDotIndex = filename.LastIndexOf('.');
			string filenameRaw = filename.Substring(0, lastDotIndex);
			string filenameExtension = filename.Substring(lastDotIndex);
			List<string> filenameList = new List<string>();
			for (int i = 0, imax = textures.Length; i < imax; i++) {
				Texture2D texture = textures[i];
				string textureFilename = filenameRaw + i.ToString("D3") + filenameExtension;
				filenameList.Add(textureFilename);
				SavePersistentDataImage(texture, textureFilename);
			}
		}

		public static string GetPlayerPrefsImagePath(string key)
		{
			if (string.IsNullOrEmpty(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImage - key cannot be empty");
				return null;
			}
			if (!PlayerPrefs.HasKey(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImage - key not found");
				return null;
			}
			return Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString(key));
		}

		public static Texture2D LoadImage(string filePath)
		{
			if (string.IsNullOrEmpty(filePath)) {
				Debug.LogErrorFormat("FileUtils:LoadImage - filePath cannot be empty");
				return null;
			}
			Texture2D tex = null;
			if (File.Exists(filePath)) {
				byte[] bytes = File.ReadAllBytes(filePath);
				tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
				tex.hideFlags = HideFlags.HideAndDontSave;
				tex.LoadImage(bytes); // This will auto-resize the texture dimensions
			}
			return tex;
		}

		public static Texture2D LoadPersistentDataImage(string filename)
		{
			if (string.IsNullOrEmpty(filename)) {
				Debug.LogErrorFormat("FileUtils:LoadPersistentDataImage - filename cannot be empty");
				return null;
			}
			return LoadImage(Path.Combine(Application.persistentDataPath, filename));
		}

		public static Texture2D LoadPlayerPrefsImage(string key)
		{
			if (string.IsNullOrEmpty(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImage - key cannot be empty");
				return null;
			}
			if (!PlayerPrefs.HasKey(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImage - key not found");
				return null;
			}
			return LoadPersistentDataImage(PlayerPrefs.GetString(key));
		}

		public static Texture2D[] LoadPlayerPrefsImages(string key)
		{
			if (string.IsNullOrEmpty(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImage - key cannot be empty");
				return null;
			}
			if (!PlayerPrefs.HasKey(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImage - key not found");
				return null;
			}
			string[] filenames = PlayerPrefsX.GetStringArray(key);
			Texture2D[] textures = new Texture2D[filenames.Length];
			for (int i = 0, imax = filenames.Length; i < imax; i++) {
				string filename = filenames[i];
				textures[i] = LoadPersistentDataImage(filename);
			}
			return textures;
		}

		#region Texture as Bytes

		public static void SaveImageAsBytes(Texture2D texture, string filePath)
		{
			if (string.IsNullOrEmpty(filePath)) {
				Debug.LogErrorFormat("FileUtils:SaveImageAsBytes - filePath cannot be empty");
				return;
			}
			string filePathLower = filePath.ToLower();
			File.WriteAllBytes(filePath, texture.GetRawTextureData());
		}

		public static void SavePersistentDataImageAsBytes(Texture2D texture, string filename)
		{
			if (string.IsNullOrEmpty(filename)) {
				Debug.LogErrorFormat("FileUtils:SavePersistentDataImageAsBytes - filename cannot be empty");
				return;
			}
			SaveImageAsBytes(texture, Path.Combine(Application.persistentDataPath, filename));
		}

		public static void SavePlayerPrefsImageAsBytes(Texture2D texture, string filename, string key)
		{
			if (string.IsNullOrEmpty(filename)) {
				Debug.LogErrorFormat("FileUtils:SavePlayerPrefsImageAsBytes - filename cannot be empty");
				return;
			}
			PlayerPrefs.SetString(key, filename);
			SavePersistentDataImageAsBytes(texture, PlayerPrefs.GetString(key));
		}

		public static Texture2D LoadImageFromBytes(string filePath, TextureFormat format, int width, int height)
		{
			if (string.IsNullOrEmpty(filePath)) {
				Debug.LogErrorFormat("FileUtils:LoadImageFromBytes - filePath cannot be empty");
				return null;
			}
			Texture2D tex = null;
			if (File.Exists(filePath)) {
				byte[] bytes = File.ReadAllBytes(filePath);
				tex = new Texture2D(width, height, format, false);
				tex.hideFlags = HideFlags.HideAndDontSave;
				tex.LoadRawTextureData(bytes);
			}
			return tex;
		}

		public static Texture2D LoadPersistentDataImageFromBytes(string filename, TextureFormat format, int width, int height)
		{
			if (string.IsNullOrEmpty(filename)) {
				Debug.LogErrorFormat("FileUtils:LoadPersistentDataImageFromBytes - filename cannot be empty");
				return null;
			}
			return LoadImageFromBytes(Path.Combine(Application.persistentDataPath, filename), format, width, height);
		}

		public static Texture2D LoadPlayerPrefsImageFromBytes(string key,TextureFormat format, int width, int height)
		{
			if (string.IsNullOrEmpty(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImageFromBytes - key cannot be empty");
				return null;
			}
			if (!PlayerPrefs.HasKey(key)) {
				Debug.LogErrorFormat("FileUtils:LoadPlayerPrefsImageFromBytes - key not found");
				return null;
			}
			return LoadPersistentDataImageFromBytes(PlayerPrefs.GetString(key), format, width, height);
		}

		#endregion

		#endregion

	}

}