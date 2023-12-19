using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<GameObject> CardInHand;

    private GameObject _WhereToSpawnCard;
    [SerializeField] private GameObject cardPrefab;

    private int CurrentMaxCardInHand = 2;
    [Tooltip("This will apply, when there is no buildings on start")]
    [SerializeField] private int startCardLimit = 2;
    [SerializeField] private List<LimitStep> cardsStepLimits = new List<LimitStep>();

    [Tooltip("Karty z jakimi zacznie gracz")]
    public List<UnitScriptableObjects> PlayerCards;

    [Tooltip("Karty mozliwe do wylosowania")]
    public List<UnitScriptableObjects> CollectionCardsToDraw;

    private void Awake()
    {
        CardInHand = new List<GameObject>();
        
        instance = this;
        if (PlayerCards.Count == 0)
        {
            Debug.Log("Gracz bez kard");
        }
    }

    private void Start()
    {
        _WhereToSpawnCard = UIController.Instance.HereCardsAre();
        CheckAndUpdateCardLimit();
    }

    public void SpawnStartCards()
    {
        foreach (var item in PlayerCards)
        {
            GameObject thisCard = Instantiate(cardPrefab, _WhereToSpawnCard.transform);
            thisCard.GetComponent<SpawnUnitCard>().stats = item;
            //CardInHand.Insert(0, thisCard);
            CardInHand.Add(thisCard);
        }

        UIController.Instance.ArrangeCards();
    }
    
    public void GetNewCardToHand() //It's called every player move
    {
        
        SpawnCard(GetRandomCardFromCollection());
        
    }

    private UnitScriptableObjects GetRandomCardFromCollection()
    {
        int randomNumber = Random.Range(0, CollectionCardsToDraw.Count);
        UnitScriptableObjects pickedCard = CollectionCardsToDraw[randomNumber];
        return pickedCard;
    }

    public void AddCardToDrawableCollection(UnitScriptableObjects newDrawable)
    {
        CollectionCardsToDraw.Add(newDrawable);
        StartCoroutine(UIController.Instance.AddCardToDrawableViewer(newDrawable));
    }

    public void RemoveCardToDrawableCollection(UnitScriptableObjects newDrawable)
    {
        CollectionCardsToDraw.Remove(newDrawable);
        StartCoroutine(UIController.Instance.RemoveCardToDrawableViewer(newDrawable));
    }

    private void SpawnCard(UnitScriptableObjects unitCardStats)
    {
        LimitCardCheck();
        GameObject thisCard = Instantiate(cardPrefab, _WhereToSpawnCard.transform);
        thisCard.GetComponent<SpawnUnitCard>().stats = unitCardStats;
        //CardInHand.Insert(0, thisCard);
        CardInHand.Add(thisCard);

        UIController.Instance.ArrangeCards();
    }

    private void LimitCardCheck()
    {
        if (CardInHand.Count >= CurrentMaxCardInHand)
        {
            Debug.Log("ultra wybuchy");
            Debug.Log(CardInHand.First().GetComponent<SpawnUnitCard>().stats.name);
            Destroy(CardInHand.First());
            CardInHand.RemoveAt(0);
            UIController.Instance.ArrangeCards();
        }
    }

    public void CheckAndUpdateCardLimit()
    {
        int budynks = Building.Instance.Budynks.Count;
        if (budynks == 0)
        {
            CurrentMaxCardInHand = startCardLimit;
        }
        else
        {
            for (int i = 1; i <= cardsStepLimits.Count; i++)
            {
                if (budynks > cardsStepLimits[i - 1].ifBiggerThan && budynks < cardsStepLimits[i].ifBiggerThan)
                {
                    CurrentMaxCardInHand = cardsStepLimits[i - 1].newLimit;
                }
            }
        }
    }
    
    public void RevomeCard(GameObject card)
    {
        //giga wybuchy
        CardInHand.Remove(card);
    }
    
}
[Serializable]
public struct LimitStep
{
    public int newLimit;
    public int ifBiggerThan;
}