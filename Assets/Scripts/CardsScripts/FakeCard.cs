using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FakeCard : MonoBehaviour
{
    [SerializeField] private new TextMeshProUGUI namee;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI food;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI damage;

    public new string name;

    public void SetUpCard(CardScriptableObject stats)
    {
        Sprite cardArtwork = stats.artwork;

        gameObject.GetComponent<Image>().sprite = cardArtwork;
        
        namee.text = stats.name;
        name = stats.name;
        desc.text = stats.desc;
        
        gold.text = stats.resources.Gold.ToString();
        food.text = stats.resources.Food.ToString();
        
        var cd = stats.GetStatsCard();
        if(cd == null) return;
        if(cd[1] < 0) return;
        hp.text = cd[0].ToString();
        damage.text = cd[1].ToString();
    }
}
