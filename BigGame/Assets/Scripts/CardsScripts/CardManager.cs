using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<GameObject> CardInHand;

    private GameObject _WhereToSpawnCard;
    [SerializeField] private GameObject cardPrefab;

    [Tooltip("Karty z jakimi zacznie gracz")]
    public List<UnitScriptableObjects> PlayerCards;
    [SerializeField] private List<GameObject> testo;


    private void Awake()
    {
        CardInHand=new List<GameObject>();
        testo = CardInHand;

        instance = this;
        if (PlayerCards.Count==0)
        {
            Debug.Log("Gracz bez kard");
        }
    }

    private void Start()
    {
        _WhereToSpawnCard = UIController.instance.HereCardsAre();
    }

    public void StartSpawnCards()
    {
        foreach (var item in PlayerCards)
        {
            GameObject thisCard = Instantiate(cardPrefab,_WhereToSpawnCard.transform);
            thisCard.GetComponent<SpawnUnitCard>().stats = item;
            CardInHand.Add(thisCard);
        }
        UIController.instance.ArrangeCards();
    }





}
