using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUnitCard : MonoBehaviour
{
    public UnitScriptableObjects stats;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI cost;
    private bool MouseOver = false;
    private Vector3 startPos;
    public Vector3 tesss;
    private void Start()
    {
        GetCardStats();
        startPos = transform.localPosition;
    }
    public void InitBuy()
    {
        bool hexIsEmpty = CheckIfHexEmpty();
        if (hexIsEmpty)
        {
            bool CanIBuy = Economy.Instance.CanIBuy(stats.cost);
            if (CanIBuy)
            {
                Economy.Instance.Purchase(stats.cost);
                GameManager.gameManager.GetComponent<SpawnerScript>().SpawnMyUnit(stats);
            }
            else
            {
                DoSomething(); //Tu mo¿na zrobiæ funckje która na œrodku ekranu pokazuje tekst "YOU CAN'T AFFORD IT / NOT ENOUGH FUNDS"
            }

            bool canIPurchase = Economy.Instance.Purchase(stats.cost); //Wywo³ywaæ spawn jednostek z Economy, lub zrobiæ skrypt Buyer i tam sprawdzaæ wszystko i spawnowaæ jednostki
            if (canIPurchase) GameManager.gameManager.GetComponent<SpawnerScript>().SpawnMyUnit(stats);
        }
        else
        {
            DoSomething(); //Tu mo¿na zrobiæ funckje która na œrodku ekranu pokazuje tekst "HEX IS OCCUPIED"
        }
    }

    private bool CheckIfHexEmpty()
    {
        if (PatchControler.Instance.PlayerCastle.jednostka == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DoSomething()
    {

    }

    public void GetCardStats()
    {
        Sprite cardArtwork = stats.artwork;

        gameObject.GetComponent<Image>().sprite = cardArtwork;
        name.text = stats.name;
        desc.text = stats.desc;
        cost.text = stats.cost.ToString();
    }



}
