using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform m_SpringArmKnuckle;
	[SerializeField] private Transform m_CameraMount;
	[SerializeField] private Camera m_Camera;
	[SerializeField] private CameraSO m_Data;

	private float m_CameraDist = 5f;

	[SerializeField] private Vector3 m_TargetOffset;

	public void RotateSpringArm(Vector2 change)
	{
		//Break the problem down into 2; yaw and pitch
		//yaw is dealt with first and is the world y rotation
		//Then deal with pitch on the local x rotation
		//This is where you limit the pitch value but the limits arent simple as you need to remap a (0 to 360) value into a (-180 to 180) value using the provided Remap function which is an extension of the float type
		float b = 5;
		float a = b.Remap360To180PN();
		//you may want to limit the amount of change rather than after rotating so tha camera doesnt jitter. Refer to the healthComponent from C4E for the idea

	}

	public void ChangeCameraDistance(float amount)
	{
		m_CameraDist += amount;
		//probably want to constrain this value
	}

	private void LateUpdate()
	{
		//set the Knuckle to be the position of the tank plus the offset
		//REMEMBER: this script is ON THE TANK. It is pulling the camera each frame

		//Expand here by using a sphere trace from the tank backwards to see if the camera needs to move forward, out the way of geometry
	}
}