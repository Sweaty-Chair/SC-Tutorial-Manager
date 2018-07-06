using UnityEngine;
using System.Collections;

namespace SweatyChair{
public static class LayerUtils
{

	public static void SetLayerRecursively(this GameObject obj, string layerName)
	{
		SetLayerRecursively(obj, LayerMask.NameToLayer(layerName));
	}

	public static void SetLayerRecursively(this GameObject obj, int layer)
	{
		obj.layer = layer;
		foreach (Transform child in obj.transform)
			child.gameObject.SetLayerRecursively(layer);
	}

	/// <summary>
	/// Sets the layer recursively excluding the objects having T component.
	/// </summary>
	public static void SetLayerRecursivelyExclude<T>(this GameObject obj, string layerName)
	{
		SetLayerRecursivelyExclude<T>(obj, LayerMask.NameToLayer(layerName));
	}

	public static void SetLayerRecursivelyExclude<T>(this GameObject obj, int layer)
	{
		if (obj.GetComponent<T>() == null)
			obj.layer = layer;
		foreach (Transform child in obj.transform)
			child.gameObject.SetLayerRecursivelyExclude<T>(layer);
	}

}
}