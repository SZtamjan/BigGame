using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{

    private Image Healthbar;
    public int CurrentHealth;
    private int MaxHealth;
    public GameObject unit;

    public Transform atCam;

    private void Start()
    {
        //Find
        Healthbar = GetComponent<Image>();
        MaxHealth = unit.GetComponent<UnitStatistic>().ReturnHp();

        atCam = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(atCam.position);

        CurrentHealth = unit.GetComponent<UnitStatistic>().ReturnHp();
        Healthbar.fillAmount = 1.0f * CurrentHealth / MaxHealth;
    }

}
