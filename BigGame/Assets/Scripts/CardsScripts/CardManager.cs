using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<GameObject> CardInHand;

    private GameObject _WhereToSpawnCard;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private int MaxCardInHand = 2;

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
        if(CardInHand.Count < MaxCardInHand)
        {
            SpawnCard(GetRandomCardFromCollection());
        }
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
        UIController.Instance.AddCardToDrawableViewer(newDrawable);
    }

    private void SpawnCard(UnitScriptableObjects unitCardStats)
    {
        GameObject thisCard = Instantiate(cardPrefab, _WhereToSpawnCard.transform);
        thisCard.GetComponent<SpawnUnitCard>().stats = unitCardStats;
        //CardInHand.Insert(0, thisCard);
        CardInHand.Add(thisCard);

        UIController.Instance.ArrangeCards();
    }

    public void LimitCard()
    {
        int CardCount = CardInHand.Count;
        if (CardCount > MaxCardInHand)
        {
            Debug.Log("ultra wybuchy");
            Debug.Log(CardInHand.First().GetComponent<SpawnUnitCard>().stats.name);
            Destroy(CardInHand.First());
            CardInHand.RemoveAt(0);
            UIController.Instance.ArrangeCards();
        }
    }

    public void RevomeCard(GameObject card)
    {
        CardInHand.Remove(card);
    }
}
