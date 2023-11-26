using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FakeCard : MonoBehaviour
{
    public UnitScriptableObjects stats;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI amountOfCards;
    
    private void Start()
    {
        Sprite cardArtwork = stats.artwork;

        gameObject.GetComponent<Image>().sprite = cardArtwork;
        name.text = stats.name;
        desc.text = stats.desc;
        cost.text = stats.cost.ToString();
    }
}
