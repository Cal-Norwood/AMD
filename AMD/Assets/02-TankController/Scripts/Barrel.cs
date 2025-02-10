using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	[SerializeField] private TankSO m_Data;
	[SerializeField] private Shell m_ShellPrefab;
	[SerializeField] public ShellSO[] m_AmmoTypes;
	[SerializeField] private int[] m_AmmoCounts;
	[SerializeField] private Transform barrel;
	[SerializeField] public int m_SelectedShell;

	private float m_CurrentDispersion;

	private bool readyToFire = false;

	public Action<int> Shoot;
	public Action SwitchAmmo;

	//Expand this class as you see fit, it is essentially your weapon
	//spawn the base shell and inject the data from m_AmmoTypes after spawning
	
	public void Init(TankSO inData)
	{
		m_Data = inData;
	}

    private void Awake()
    {
		readyToFire = true;
        m_SelectedShell = 0;
        for (int i = 0; i < m_AmmoCounts.Length; i++)
        {
            m_AmmoCounts[i] = 3;
        }
    }

    public void Fire()
	{
		if (m_AmmoCounts[m_SelectedShell] > 0 && readyToFire == true)
		{
            readyToFire = false;
			m_AmmoCounts[m_SelectedShell]--;
			StartCoroutine(Reload());
            var shell = Instantiate(m_ShellPrefab.gameObject, barrel.position, Quaternion.identity);
            shell.transform.up = barrel.forward;
            shell.GetComponent<Shell>().velocity = m_AmmoTypes[m_SelectedShell].Velocity;
            shell.GetComponent<Shell>().damage = m_AmmoTypes[m_SelectedShell].Damage;
        }
    }

	public void SwitchAmmunition(float change)
	{
        StartCoroutine(Reload());
		readyToFire = false;
		m_SelectedShell += (int)change;

        if (m_SelectedShell > m_AmmoTypes.Length - 1)
		{
            m_SelectedShell = 0;
		}
		else if(m_SelectedShell < 0)
		{
            m_SelectedShell = m_AmmoTypes.Length - 1;
        }

        SwitchAmmo?.Invoke();
    }

	private IEnumerator Reload()
	{
		Shoot.Invoke(0);
		yield return new WaitForSeconds(3f);
		readyToFire = true;
        Shoot?.Invoke(1);
    }
}
