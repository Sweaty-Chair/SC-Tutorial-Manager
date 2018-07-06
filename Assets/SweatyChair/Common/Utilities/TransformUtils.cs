using UnityEngine;

public static class TransformUtils
{

	/// <summary>
	/// Gets the transform by path, '/' is the default path separator in Unity.
	/// </summary>
	public static Transform GetTransformByPath(string path, char separator = '/')
	{
		if (string.IsNullOrEmpty(path))
			return null;

		// Find the first root Transform first
		string[] names = path.Split(separator);
		Transform rootTF = null;
		if (names != null && names.Length > 0) {
			GameObject[] rootGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (GameObject rootGO in rootGOs) {
				if (rootGO.name == names[0]) {
					rootTF = rootGO.transform;
					break;
				}
			}
			if (rootTF == null)
				return null;
		}

		// Find the Transform with the path from root Transform
		if (names.Length > 1) {
			path = string.Join(separator.ToString(), names, 1, names.Length - 1);
			path = path.TrimEnd(' '); // Remove bank space at end
			return rootTF.Find(path);
		}

		return rootTF;
	}

	public static T GetComponentByPath<T>(string path) where T : Object
	{
		Transform targetTF = GetTransformByPath(path);
		return targetTF == null ? null : targetTF.GetComponent<T>();
	}

	public static void DestroyChildrenExclude(this Transform tf, Transform excludeTF)
	{
		Transform[] childrenTFs = new Transform[tf.childCount];
		for (int i = 0, imax = tf.childCount; i < imax; i++)
			childrenTFs[i] = tf.GetChild(i);

		foreach (Transform childTF in childrenTFs) {
			if (childTF == excludeTF)
				continue;
			if (Application.isPlaying) {
				childTF.parent = null;
				Object.Destroy(childTF.gameObject);
			} else {
				Object.DestroyImmediate(childTF.gameObject);
			}
		}
	}

	public static void DestroyChildren(this Transform tf, string childrenName)
	{
		Transform[] childrenTFs = new Transform[tf.childCount];
		for (int i = 0, imax = tf.childCount; i < imax; i++)
			childrenTFs[i] = tf.GetChild(i);

		foreach (Transform childTF in childrenTFs) {
			if (childTF.name.Contains(childrenName)) {
				if (Application.isPlaying) {
					childTF.parent = null;
					Object.Destroy(childTF.gameObject);
				} else {
					Object.DestroyImmediate(childTF.gameObject);
				}
			}
		}
	}

	public static void DestroyChildrenExclude(this Transform tf, string excludeName)
	{
		Transform[] childrenTFs = new Transform[tf.childCount];
		for (int i = 0, imax = tf.childCount; i < imax; i++)
			childrenTFs[i] = tf.GetChild(i);

		foreach (Transform childTF in childrenTFs) {
			if (childTF.name.Contains(excludeName))
				continue;
			if (Application.isPlaying) {
				childTF.parent = null;
				Object.Destroy(childTF.gameObject);
			} else {
				Object.DestroyImmediate(childTF.gameObject);
			}
		}
	}
	
	/// <summary>
	/// Rename all children
	/// </summary>
	public static void RenameChildren(this Transform tf, string newName)
	{
		foreach (Transform t in tf)
			t.name = newName;
	}

	/// <summary>
	/// Disable all children
	/// </summary>
	public static void DisableChildren(this Transform tf)
	{
		tf.ToggleChildren(false);
	}

	/// <summary>
	/// Toggle the GameObject of all children
	/// </summary>
	public static void ToggleChildren(this Transform tf, bool isShown = true)
	{
		foreach (Transform t in tf)
			t.gameObject.SetActive(isShown);
	}

	/// <summary>
	/// Destroy all specfic components in next lower level children, exclude tf inself.
	/// </summary>
	public static void DestroyChildrenComponents<T>(this Transform tf) where T:Component
	{
		foreach (Transform t in tf) {
			T obj = t.GetComponent<T>();
			if (obj != null)
				Object.Destroy(obj);
		}
	}

	/// <summary>
	/// Destroy all specfic components in all lower level children, include tf inself.
	/// </summary>
	public static void DestroyAllChildrenComponents<T>(this Transform tf, bool includeInactive) where T:Component
	{
		T[] tArray = tf.GetComponentsInChildren<T>(includeInactive);
		foreach (T obj in tArray) {
			Object.Destroy(obj);
		}
	}

    /// <summary>
    /// Get the path of a transform, '/' is the default path separator in Unity.
    /// </summary>
    public static string ToPath(this Transform tf, char separator = '/')
	{
		if (tf == null)
			return string.Empty;
		if (tf.parent == null)
			return tf.name;
		return tf.parent.ToPath() + separator + tf.name;
	}

	/// <summary>
	/// Get the path of a component, '/' is the default path separator in Unity.
	/// </summary>
	public static string ToPath<T>(this T t, char separator = '/') where T:Component
	{
		if (t == null)
			return string.Empty;
		return t.transform.ToPath();
	}

	/// <summary>
	/// Get the path of a transform from a root transform, '/' is the default path separator in Unity.
	/// </summary>
	public static string ToPath(this Transform tf, Transform rootTF, char separator = '/')
	{
		if (tf == null || tf == rootTF)
			return string.Empty;
		if (tf.parent == null || tf.parent == rootTF)
			return tf.name;
		return tf.parent.ToPath(rootTF, separator) + separator + tf.name;
	}


#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Transforms/PrintSelectedTransformsPath")]
    public static void PrintSelectedTransformsPath()
    {
        GameObject[] selectedGameObjects = UnityEditor.Selection.gameObjects;
        foreach (GameObject selectedGo in selectedGameObjects)
            Debug.Log(selectedGo.transform.ToPath());
    }

#endif
}