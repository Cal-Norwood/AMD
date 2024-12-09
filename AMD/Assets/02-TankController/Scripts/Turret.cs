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
		//if already dirty then return
		//else set the value and start the below coroutine
	}

	private IEnumerator C_AimTurret()
	{
		//Fix this to loop while the rotation is dirty, rotate towards the needed vector and unset dirty when facing
		//TIP: simplfy the problem into 2D with the Vector3.ProjectOnPlane function and diagram it out. The turret rotates around the local y and the barrel around the local x
		//Make use of the remap function as shown in Camera.cs to help
		//extention here could include some SUVAT formula work to adjust the aim to hit where the camera is pointing, accounting for gravity
		yield return null;
	}
}
