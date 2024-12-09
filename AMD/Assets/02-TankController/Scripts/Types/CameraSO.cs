using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "DataObject/CameraData")]
public class CameraSO : ScriptableObject
{
	public float YawSensitivity;
	public float PitchSensitivity;
	public float ZoomSensitivity;
	public float MaxPitch;
	public float MinPitch;
	public float MaxDist;
	public float MinDist;
	public float CameraProbeSize;
}
