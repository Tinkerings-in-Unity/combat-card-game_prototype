using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckController : MonoBehaviour
{
    public static DeckController Instance = null;
    
    [SerializeField] private List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
    [SerializeField] private Card cardToSpawn;
    [SerializeField] private int drawCardCost = 2;
    [SerializeField] private float delayBetweenCardDraws = 0.25f;
    private List<CardScriptableObject> _activeCards = new List<CardScriptableObject>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupDeck();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.T))
        // {
        //     DrawCardToHand();
        // }
    }

    public void SetupDeck()
    {
        _activeCards.Clear();

        var tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        while (tempDeck.Count > 0)
        {
            var selected = Random.Range(0, tempDeck.Count);
            _activeCards.Add(tempDeck[selected]);
            
            tempDeck.RemoveAt(selected);
        }
    }

    public void DrawCardToHand()
    {
        if (_activeCards.Count == 0)
        {
            SetupDeck();
        }

        var newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);

        newCard.cardSO = _activeCards[0];
        newCard.SetupCard();
        
        _activeCards.RemoveAt(0);
        
        HandController.Instance.AddCardToHand(newCard);
    }
    
    public bool DrawCardForMana()
    {
        var canDrawCard = false;
        
        if (BattleController.Instance.playerMana >= drawCardCost)
        {
            DrawCardToHand();
            BattleController.Instance.SpendPlayerMana(drawCardCost);
            canDrawCard = true;
        }
        else
        {
            UIController.Instance.ShowLowManaWarning();
        }

        return canDrawCard;
    }

    public void DrawMultipleCards(int amountOfCardsToDraw)
    {
        StartCoroutine(DrawMultipleCardsCoroutine(amountOfCardsToDraw));
    }

    private IEnumerator DrawMultipleCardsCoroutine(int amountToDraw)
    {
        for (var i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();

            yield return new WaitForSeconds(delayBetweenCardDraws);
        }
    }
}
