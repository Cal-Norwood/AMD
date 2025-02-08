using System;
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
        change.x *= m_Data.YawSensitivity;
        change.y *= m_Data.PitchSensitivity;

        Vector3 euler = m_SpringArmKnuckle.localEulerAngles;

        float yaw = euler.y - change.x;
        float pitch = (euler.x > 180) ? euler.x - 360 : euler.x;

        pitch -= change.y;

        pitch = Mathf.Clamp(pitch, m_Data.MinPitch, m_Data.MaxPitch);

        m_SpringArmKnuckle.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    public void ChangeCameraDistance(float amount)
	{
		m_CameraDist += amount;
		//probably want to constrain this value
	}

	private void LateUpdate()
	{
        m_SpringArmKnuckle.position = transform.position + m_TargetOffset;

        m_CameraMount.position = m_SpringArmKnuckle.position - m_SpringArmKnuckle.forward * m_CameraDist;
        m_CameraMount.LookAt(m_SpringArmKnuckle.position);
    }
}