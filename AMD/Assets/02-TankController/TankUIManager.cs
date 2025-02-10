using UnityEngine;
using TMPro;

public class TankUIManager : MonoBehaviour
{
    [SerializeField] private GameObject Tank;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI reloadingText;
    private void Awake()
    {
        Tank.GetComponent<Barrel>().SwitchAmmo += AmmoSwitchUpdate;
        Tank.GetComponent<Barrel>().Shoot += ReloadingDisplay;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Barrel barrel = Tank.GetComponent<Barrel>();
        currentAmmoText.text = "CurrentShell: " + barrel.m_AmmoTypes[barrel.m_SelectedShell].name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AmmoSwitchUpdate()
    {
        Barrel barrel = Tank.GetComponent<Barrel>();
        currentAmmoText.text = "CurrentShell: " + barrel.m_AmmoTypes[barrel.m_SelectedShell].name;
    }

    private void ReloadingDisplay(int active)
    {
        if(active == 0)
        {
            reloadingText.enabled = true;
        }
        else
        {
            reloadingText.enabled = false;
        }
    }
}
