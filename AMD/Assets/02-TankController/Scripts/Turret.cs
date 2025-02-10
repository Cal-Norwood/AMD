using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	[SerializeField] private Transform m_CameraMount;
	[SerializeField] private Transform m_Turret;
	[SerializeField] private Transform m_Barrel;

	private TankSO m_Data;
	private bool m_RotationDirty;
	private Coroutine m_CRAimingTurret;

	private void Awake()
	{
		m_RotationDirty = false;
	}

	public void Init(TankSO inData)
	{
		m_Data = inData;
	}

	public void SetRotationDirty()
	{
		if (m_RotationDirty)
		{
			return;
		}
		else
		{
			m_RotationDirty = true;
			StartCoroutine(C_AimTurret());
		}
		//if already dirty then return
		//else set the value and start the below coroutine
	}

    private IEnumerator C_AimTurret()
    {
		while (m_RotationDirty)
		{
			Vector3 projectedCamTurret = Vector3.ProjectOnPlane(m_CameraMount.forward, m_Turret.up);
			Quaternion targetRotationTurret = Quaternion.LookRotation(projectedCamTurret, m_Turret.up);

			Vector3 projectedCamBarrel = Vector3.ProjectOnPlane(m_CameraMount.forward, m_Turret.right);
			Quaternion targetRotationBarrel = Quaternion.LookRotation(projectedCamBarrel, m_Barrel.up);

			float barrelPitch = Vector3.SignedAngle(m_Turret.forward, projectedCamBarrel, m_Barrel.right);

			//barrelPitch = (targetRotationBarrel.eulerAngles.x > 180) ? targetRotationBarrel.eulerAngles.x - 360 : targetRotationBarrel.eulerAngles.x;

			barrelPitch = Mathf.Clamp(barrelPitch, m_Data.TurretData.ElevationLimit, m_Data.TurretData.DepressionLimit);

			targetRotationBarrel = Quaternion.Euler(barrelPitch, targetRotationBarrel.eulerAngles.y, targetRotationBarrel.eulerAngles.z);

			if (Quaternion.Angle(m_Turret.rotation, targetRotationTurret) > 0.1f)
			{
				m_Turret.rotation = Quaternion.RotateTowards(m_Turret.rotation, targetRotationTurret, m_Data.TurretData.TurretTraverseSpeed * Time.deltaTime);
			}

			if (Quaternion.Angle(m_Barrel.rotation, targetRotationBarrel) > 0.1f)
			{
				m_Barrel.rotation = Quaternion.RotateTowards(m_Barrel.rotation, targetRotationBarrel, m_Data.TurretData.BarrelTraverseSpeed * Time.deltaTime);
			}

			if (Quaternion.Angle(m_Turret.rotation, targetRotationTurret) < 0.1f && Quaternion.Angle(m_Barrel.rotation, targetRotationBarrel) < 0.1f)
			{
				break;
			}

			Debug.Log(barrelPitch);

			yield return null;
		}

        m_RotationDirty = false;
    }
}
