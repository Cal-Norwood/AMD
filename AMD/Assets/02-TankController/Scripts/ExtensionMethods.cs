using UnityEngine;

public static class FloatExtension
{
	public static float Remap360To180PN(this float inAngle)
	{
		return Mathf.Repeat(180f + inAngle, 180f) - 180f;
	}
}