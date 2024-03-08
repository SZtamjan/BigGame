using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUnitsShow : MonoBehaviour
{

    private Image Healthbar;
    private int MaxHealth;

    private Image Shield;

    public GameObject hpbar;
    public GameObject shielBar;

    public Transform atCam;

    private void Awake()
    {
        Healthbar = hpbar.GetComponent<Image>();
        Shield = shielBar.GetComponent<Image>();

        atCam = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(atCam.position);
    }

    public void HPUpdate(int hp)
    {
        if (hp/MaxHealth < 1f)
        {
            GetComponent<Canvas>().enabled = true;
            Healthbar.fillAmount = 1.0f * hp / MaxHealth;
        }
        if (hp<=0)
        {
            Invoke("DisableHpBar", 1.5f);
        }
        
    }

    public void ShieldUpdate(int shield)
    {
        if (shield / MaxHealth < 1f)
        {
            GetComponent<Canvas>().enabled = true;
            Shield.fillAmount = 1.0f * shield / MaxHealth;
        }
        

    }
    public void DisableHpBar()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void MaxHP(int maxHP)
    {
        MaxHealth=maxHP;
        Healthbar.fillAmount = 1.0f;
        GetComponent<Canvas>().enabled = false;
    }

}
