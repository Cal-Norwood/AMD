using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
	public event Action<bool> OnGroundedChanged; 

	[SerializeField] private Transform m_Wheel;
	[SerializeField] private Rigidbody m_RB;

	private SuspensionSO m_Data;
	private float m_SpringSize;
	private bool m_Grounded;
    private RaycastHit m_HitInfo;

    public void Init(SuspensionSO inData, Rigidbody _RBRef)
	{
		m_RB ??= _RBRef;
		m_Data = inData;

        m_SpringSize = (m_Data.WheelDiameter / 2f) + Mathf.Abs(m_Wheel.localPosition.y);
	}

	public bool GetGrounded()
	{
		return m_Grounded;
	}

	private void FixedUpdate()
	{
        bool hit = Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out RaycastHit newHitInfo, m_SpringSize);

		if(hit != m_Grounded)
		{
			m_Grounded = !m_Grounded;
			OnGroundedChanged?.Invoke(m_Grounded);
		}
    }

    public void Bounce()
    {
        Vector3 springDirection = -gameObject.transform.up;

        if (Physics.Raycast(transform.position, springDirection, out RaycastHit hitInfo, m_SpringSize))
        {
            Vector3 worldVel = m_RB.GetPointVelocity(transform.position);
            float suspensionOffset = m_SpringSize - hitInfo.distance;
            float suspensionVelocity = (Vector3.Dot(worldVel, -springDirection));
            Vector3 suspensionForce = -springDirection * ((suspensionOffset * m_Data.SuspensionStrength) - (suspensionVelocity * m_Data.SuspensionDamper));

            float horizontalVel = Vector3.Dot(worldVel, transform.right);
            Vector3 horizontalForce = m_Data.HorizontalDrag * -horizontalVel * transform.right;

            m_RB.AddForceAtPosition(suspensionForce, transform.position, ForceMode.Acceleration);

            Vector3 targetPosition = hitInfo.point + (-springDirection * (m_Data.WheelDiameter / 2));
            m_Wheel.transform.position = Vector3.MoveTowards(m_Wheel.transform.position, targetPosition, Time.deltaTime);
        }
        else
        {
            float defaultHeight = -m_SpringSize / 2; // Default rest position of wheel
            Vector3 localPos = m_Wheel.transform.localPosition;

            if (localPos.y > defaultHeight) // Only move down if the wheel is still high
            {
                localPos.y -= Time.deltaTime; // Lower smoothly
                m_Wheel.transform.localPosition = localPos;
            }
        }
    }
}
