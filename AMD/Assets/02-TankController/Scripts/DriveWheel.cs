using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveWheel : MonoBehaviour
{
	public event Action<bool> OnGroundedChanged;

	[SerializeField] private Rigidbody m_RB;
	[SerializeField] private TankSO m_Data;
	[SerializeField] private Suspension[] m_SuspensionWheels;
	[SerializeField] private int m_NumGroundedWheels;
	[SerializeField] private bool m_Grounded;

	[SerializeField] private bool m_LeftWheel = false;
	[SerializeField] private bool m_RightWheel = false;

	private float m_Acceleration;
	private float m_SteerAmount;
	public void SetAcceleration(float amount) => m_Acceleration = amount;

    private void Start()
    {
		m_Acceleration = MathF.Sqrt((m_Data.EngineData.HorsePower * (float)745.6992) / (m_RB.mass));
    }

    public void SetSteer(float amount)
	{
		if(m_LeftWheel)
        {
			m_SteerAmount = amount * 0.3f;
		}

		if (m_RightWheel)
		{
			m_SteerAmount = -amount * 0.3f;
		}
	}

	public void Init(TankSO inData, Rigidbody _RBRef)
	{
		m_Data = inData;
		m_RB ??= _RBRef;

		m_NumGroundedWheels = 0;

		foreach (Suspension wheel in m_SuspensionWheels)
		{
			wheel.Init(m_Data.SuspensionData, m_RB);
			wheel.OnGroundedChanged += Handle_WheelGroundedChanged;
		}
	}

	private void Handle_WheelGroundedChanged(bool newGrounded)
	{
		if (newGrounded)
		{
			m_Grounded = true;
			m_NumGroundedWheels++;
		}
		else
		{
			m_NumGroundedWheels--;

			if(m_NumGroundedWheels == 0)
            {
				m_Grounded = false;
            }
		}
	}

	private void FixedUpdate()
	{
		if(m_Grounded)
        {
			float tracktion = ((float)m_NumGroundedWheels / (float)m_SuspensionWheels.Length);

			if(Mathf.Abs(m_SteerAmount) > 0.3)
            {
				m_RB?.AddForceAtPosition(gameObject.transform.forward * m_Acceleration * 3 * tracktion * m_SteerAmount, gameObject.transform.position, ForceMode.Acceleration);
			}
			else
            {
				m_RB?.AddForceAtPosition(gameObject.transform.forward * m_Acceleration * 3 * tracktion, gameObject.transform.position, ForceMode.Acceleration);
			}
		}

        foreach (Suspension wheel in m_SuspensionWheels)
        {
            wheel.Bounce();
        }

        //deal with acceleration here
        //you could retrofit this to be a coroutine based on when SetAcceleration brings in a value or a 0
        //TIP: acceleration is not as simple as plugging values in from the typeData, Unity works in metric units (metric tons, meters per second, etc)
        //No need to make a full engine simulation with gearing here that is going too deep, you have a couple of weeks at most for this
    }
}