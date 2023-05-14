using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private GameObject QuickMenu;
    [SerializeField] private GameObject DeckCarkd;
    [SerializeField] private GameObject BuildingsCards;
    [SerializeField] private bool BuildingsCardShowing = false;

    [SerializeField] private TextMeshProUGUI ShowFunds;
    [SerializeField] private TextMeshProUGUI ShowTurn;
    [SerializeField] private TextMeshProUGUI ShowEndDisplay;
    [SerializeField] private TextMeshProUGUI ShowEconomyWarming;
    [SerializeField] private Slider PlayerCastle;
    [SerializeField] private Slider ComputerCastle;
    // mute tu ma te¿ byæ

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        BuildingsCards.SetActive(false);
        BuildingsCardShowing = false;
    }

    public void ChangeBuildingCardsShow()
    {
        if (!BuildingsCardShowing)
        {
            BuildingsCards.SetActive(true);            
            BuildingsCardShowing = true;            
        }
        else
        {
            BuildingsCards.SetActive(false);
            BuildingsCardShowing = false;
        }
    }
    public void ChangeBuildingCardsShow(bool state)
    {        
            BuildingsCards.SetActive(state);
            BuildingsCardShowing = state;
                
    }

}
