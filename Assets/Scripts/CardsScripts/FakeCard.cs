using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FakeCard : MonoBehaviour
{
    [SerializeField] private new TextMeshProUGUI namee;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private TextMeshProUGUI cost;

    public new string name;

    public void SetUpCard(UnitScriptableObjects stats)
    {
        Sprite cardArtwork = stats.artwork;

        gameObject.GetComponent<Image>().sprite = cardArtwork;
        namee.text = stats.name;
        name = stats.name;
        desc.text = stats.desc;
        cost.text = stats.resources.Gold.ToString();
    }
}
