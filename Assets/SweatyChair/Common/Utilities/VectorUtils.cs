using UnityEngine;

public static class VectorUtils
{
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}

	/// <summary>
	/// Multiplies both vectors by (xMultiplier, yMultiplier, zMultiplier) and returns the distance between those products.
	/// </summary>
	public static float DistanceScale (Vector3 a, Vector3 b, float xMultiplier = 1f, float yMultiplier = 1f, float zMultiplier = 1f)
	{
		return DistanceScale (a, b, new Vector3 (xMultiplier, yMultiplier, zMultiplier));
	}

	/// <summary>
	/// Multiplies both vectors by 'multiplier' and returns the distance between those products.
	/// </summary>
	public static float DistanceScale (Vector3 a, Vector3 b, Vector3 multiplier)
	{
		return Vector3.Distance (Vector3.Scale (a, multiplier), Vector3.Scale (b, multiplier));
	}

	/// <summary>
	/// Returns the Mid Point between two vectors.
	/// </summary>
	public static Vector3 MidPoint (Vector3 a, Vector3 b)
	{
		return Vector3.Lerp (a, b, 0.5f);
	}

	/// <summary>
	/// Clamps an angle used in a rotation between a known min and max
	/// </summary>
	/// <param name="minAngle"></param>
	/// <param name="maxAngle"></param>
	/// <param name="angle"></param>
	/// <param name="angleOffset"></param>
	public static float ClampRotation(float angle, float minAngle, float maxAngle, float angleOffset = 0)
	{
		angleOffset += 180;
		angle -= angleOffset;
		angle = WrapAngle(angle);
		angle -= 180;
		angle = Mathf.Clamp(angle, minAngle, maxAngle);
		angle += 180;
		return angle + angleOffset;
	}

	/// <summary>
	/// Wraps an angle to always be between 0 and 360 degrees
	/// </summary>
	/// <param name="angle"></param>
	/// <returns></returns>
	public static float WrapAngle(float angle)
	{
		while (angle < 0)
			angle += 360;

		return Mathf.Repeat(angle, 360);
	}

}