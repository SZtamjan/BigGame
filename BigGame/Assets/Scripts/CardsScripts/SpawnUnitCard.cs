using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUnitCard : MonoBehaviour
{
    public UnitScriptableObjects stats;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI cost;

    private void Start()
    {
        GetCardStats();
    }
    public void SpawnUnit()
    {
        if (PatchControler.Instance.PlayerCastle.jednostka == null)
        {
            bool canIPurchase = Economy.Instance.Purchase(stats.cost);
            if (canIPurchase) GameManager.gameManager.GetComponent<SpawnerScript>().SpawnMyUnit(stats);
        }
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
