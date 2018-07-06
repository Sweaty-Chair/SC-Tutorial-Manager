using UnityEngine;
using System.Collections;

public static class GameObjectUtils
{

	/// <summary>
	/// Gets the object by path, '/' is the default path separator in Unity.
	/// </summary>
	public static GameObject GetObjectByPath(string nameOrPath, char separator = '/')
	{
		if (string.IsNullOrEmpty(nameOrPath))
			return null;

		string[] names = nameOrPath.Split(separator);
		GameObject go = null;
		if (names != null && names.Length > 0) {
			GameObject[] rootGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (GameObject rootGO in rootGOs) {
				if (rootGO.name == names[0]) {
					go = rootGO;
					break;
				}
			}
			if (go == null)
				return null;
		}
		if (names.Length > 1) {
			string path = string.Join(separator.ToString(), names, 1, names.Length - 1);
			path = path.TrimEnd(' '); // Remove bank space at end
			Transform temp = go.transform.Find(path);
			return temp == null ? null : temp.gameObject;
		} 

		return go;
	}

	/// <summary>
	/// Instantiate prefab in Resources and parent it to a GameObject.
	/// </summary>
	public static GameObject AddChild(this GameObject parent, GameObject prefab)
	{
		return GameObject.Instantiate(prefab, parent == null ? null : parent.transform);
	}

	/// <summary>
	/// Instantiate prefab in Resources and parent it to a GameObject.
	/// </summary>
	public static GameObject AddChild(this GameObject parent, string resourcesPrefabPath)
	{
		return GameObject.Instantiate(GetResourcePrefab(resourcesPrefabPath), parent.transform);
	}

	/// <summary>
	/// Get the prefab in Resource by path.
	/// </summary>
	private static GameObject GetResourcePrefab(string resourcesPrefabPath)
	{
		GameObject go = Resources.Load<GameObject>(resourcesPrefabPath);
		if (go == null) {
			Debug.LogErrorFormat("GameObjectHelper::GetResourcePrefab - Invalid path={0}", resourcesPrefabPath);
			return null;
		}
		return go;
	}

	/// <summary>
	/// Instantiate prefab in Resources by path and parent it to a GameObject by path.
	/// </summary>
	public static GameObject AddChildToPath(string parentPath, string resourcesPrefabPath)
	{
		return AddChildToPath(parentPath, GetResourcePrefab(resourcesPrefabPath));
	}

	/// <summary>
	/// Instantiate prefab and parent it to a GameObject by path.
	/// </summary>
	public static GameObject AddChildToPath(string parentPath, GameObject prefab)
	{
		if (prefab == null)
			return null;

		Transform parentTF = null;

		if (!string.IsNullOrEmpty(parentPath)) {
			parentTF = TransformUtils.GetTransformByPath(parentPath);
			if (parentTF == null)
				Debug.LogWarningFormat("GameObjectHelper:AddChildToPath - Invalid parent path={0}", parentPath);
		}

		return GameObject.Instantiate (prefab, parentTF == null ? null : parentTF);
	}

	public static void Destroy(GameObject go)
	{
		if (Application.isPlaying)
			GameObject.Destroy(go);
		else
			GameObject.DestroyImmediate(go);
	}

}