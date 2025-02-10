using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
	private AM_02Tank m_ActionMap; //input
	[SerializeField] private TankSO m_Data; //type object link
	//Component links
	[SerializeField] private Rigidbody m_RB;
	[SerializeField] private CameraController m_CameraController;
	[SerializeField] private Turret m_TurretController;
	[SerializeField] private Barrel m_BarrelController;
	[SerializeField] private DriveWheel[] m_DriveWheels;

	private float m_InAccelerate;

	private float m_InSteer;
	private bool m_IsSteering;
	private Coroutine m_CRSteer;

	private bool m_IsFiring;

	private void Awake()
	{
		m_ActionMap = new AM_02Tank();

		m_RB ??= GetComponent<Rigidbody>();
		m_CameraController ??= GetComponent<CameraController>();

		m_TurretController ??= GetComponent<Turret>();
		m_TurretController.Init(m_Data);

		foreach (DriveWheel wheel in m_DriveWheels)
		{
			wheel.Init(m_Data, m_RB);
		}
	}

	private void OnEnable()
	{
		m_ActionMap.Enable();

		m_ActionMap.Default.Accelerate.performed += Handle_AcceleratePerformed;
		m_ActionMap.Default.Accelerate.canceled += Handle_AccelerateCanceled;
		m_ActionMap.Default.Steer.performed += Handle_SteerPerformed;
		m_ActionMap.Default.Steer.canceled += Handle_SteerCanceled;
		m_ActionMap.Default.Fire.performed += Handle_FirePerformed;
		m_ActionMap.Default.Fire.canceled += Handle_FireCanceled;
		m_ActionMap.Default.Aim.performed += Handle_AimPerformed;
		m_ActionMap.Default.Zoom.performed += Handle_ZoomPerformed;
		m_ActionMap.Default.SwitchAmmo.performed += Handle_AmmoSwitch;
	}
	private void OnDisable()
	{
		m_ActionMap.Disable();

		m_ActionMap.Default.Accelerate.performed -= Handle_AcceleratePerformed;
		m_ActionMap.Default.Accelerate.canceled -= Handle_AccelerateCanceled;
		m_ActionMap.Default.Steer.performed -= Handle_SteerPerformed;
		m_ActionMap.Default.Steer.canceled -= Handle_SteerCanceled;
		m_ActionMap.Default.Fire.performed -= Handle_FirePerformed;
		m_ActionMap.Default.Fire.canceled -= Handle_FireCanceled;
		m_ActionMap.Default.Aim.performed -= Handle_AimPerformed;
		m_ActionMap.Default.Zoom.performed -= Handle_ZoomPerformed;
        m_ActionMap.Default.SwitchAmmo.performed += Handle_AmmoSwitch;
    }

	private void Handle_AcceleratePerformed(InputAction.CallbackContext context)
	{
		m_InAccelerate = context.ReadValue<float>();
		foreach (DriveWheel wheel in m_DriveWheels)
		{
			wheel.SetAcceleration(m_InAccelerate);
		}
		m_TurretController.SetRotationDirty();
	}
	private void Handle_AccelerateCanceled(InputAction.CallbackContext context)
	{
		m_InAccelerate = context.ReadValue<float>();
		foreach (DriveWheel wheel in m_DriveWheels)
		{
			wheel.SetAcceleration(m_InAccelerate);
		}
		m_TurretController.SetRotationDirty();
	}

	private void Handle_SteerPerformed(InputAction.CallbackContext context)
	{
		m_InSteer = context.ReadValue<float>();
		foreach (DriveWheel wheel in m_DriveWheels)
		{
			wheel.SetSteer(m_InSteer);
		}
	}

	private void Handle_SteerCanceled(InputAction.CallbackContext context)
	{
		m_InSteer = context.ReadValue<float>();
		foreach (DriveWheel wheel in m_DriveWheels)
		{
			wheel.SetSteer(m_InSteer);
		}
	}

	private void Handle_FirePerformed(InputAction.CallbackContext context)
	{
		m_IsFiring = true;
		m_BarrelController.Fire();
	}
	private void Handle_FireCanceled(InputAction.CallbackContext context)
	{
		m_IsFiring = false;
		//this is here in the case you want to expand to support magazine style auto-loaders but thats large scope
	}

	private void Handle_AimPerformed(InputAction.CallbackContext context)
	{
		m_CameraController.RotateSpringArm(context.ReadValue<Vector2>());
		m_TurretController.SetRotationDirty();
	}

	private void Handle_ZoomPerformed(InputAction.CallbackContext context)
	{
		m_CameraController.ChangeCameraDistance(context.ReadValue<float>());
		m_TurretController.SetRotationDirty();
	}

	private void Handle_AmmoSwitch(InputAction.CallbackContext context)
	{
		m_BarrelController.SwitchAmmunition(context.ReadValue<float>());
	}
}