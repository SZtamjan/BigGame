using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<GameObject> CardInHand;

    private GameObject _WhereToSpawnCard;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private int MaxCardInHand = 2;

    [Tooltip("Karty z jakimi zacznie gracz")]
    public List<UnitScriptableObjects> PlayerCards;
    

    [Tooltip("Karty z które mo¿na wyci¹gn¹æ")]
    public List<UnitScriptableObjects> CardToDraw;

    private List<UnitScriptableObjects> CardsToSpawnInHand;


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
        SpawnCards(PlayerCards);
    }

    public void SpawnCards(List<UnitScriptableObjects> cardsToSpawn)
    {
        foreach (var item in cardsToSpawn)
        {
            GameObject thisCard = Instantiate(cardPrefab, _WhereToSpawnCard.transform);
            thisCard.GetComponent<SpawnUnitCard>().stats = item;
            //CardInHand.Insert(0, thisCard);
            CardInHand.Add(thisCard);
        }
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

    public void WyjebReke()
    {
        foreach (var item in CardInHand)
        {
            Destroy(item);

        }
        CardInHand = new List<GameObject>();
    }

    public void GetNewRenka()
    {
        CardsToSpawnInHand = new List<UnitScriptableObjects>();
        for (int i = 0; i < MaxCardInHand; i++)
        {
            AddCard();
        }
        SpawnCards(CardsToSpawnInHand);

    }

    public void AddCard()
    {
        int randomNumber = Random.Range(0, CardToDraw.Count);
        CardsToSpawnInHand.Add(CardToDraw[randomNumber]);
    }




}
