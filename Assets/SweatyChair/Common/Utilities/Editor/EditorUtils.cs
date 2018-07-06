using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using SweatyChair;

public static class EditorUtils
{

	/// <summary>
	/// Writes code into the script at Asset path. Warning: everything will be overwritten.
	/// </summary>
	public static void WriteScript(string filePath, string code, UnityAction onComplete)
	{
		bool success = false;

		FileUtils.UnsetReadOnly(filePath);
		using (StreamWriter writer = new StreamWriter(filePath, false)) {
			try {
				writer.WriteLine("{0}", code);
				success = true;
			} catch (System.Exception ex) {
				string msg = " \n" + ex.ToString();
				Debug.LogError(msg);
				EditorUtility.DisplayDialog("EditorUtils:WriteScript - Error when trying to regenerate file at path=" + filePath, msg, "OK");
			}
		}

		if (success) {
			AssetDatabase.Refresh();
			if (onComplete != null)
				onComplete();
		}
	}

}