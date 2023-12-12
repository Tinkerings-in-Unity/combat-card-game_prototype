using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{

    public static EnemyController Instance;
    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();

    public enum AIType
    {
        PlaceFromDeck, HandRandomPlace, HandDefensive, HandAttacking
    }

    public AIType EnemyEnemyAiType => enemyAiType;
    

    [SerializeField] private Card cardToSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private AIType enemyAiType;
    [SerializeField] private int startHandSize;
    
    private List<CardScriptableObject> _activeCards = new List<CardScriptableObject>();
    private List<CardScriptableObject> _cardsInHand = new List<CardScriptableObject>();

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetupDeck();

        if (enemyAiType != AIType.PlaceFromDeck)
        {
            SetupHand();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void StartAction()
    {
        StartCoroutine(EnemyActionCoroutine());
    }

    private IEnumerator EnemyActionCoroutine()
    {
        if (_activeCards.Count == 0)
        {
            SetupDeck();
        }
        
        yield return new WaitForSeconds(0.5f);

        var cardPlacePoints = new List<CardPlacePoint>();
        cardPlacePoints.AddRange(CardPointsController.Instance.enemyCardPoints);

        var randomPoint = Random.Range(0, cardPlacePoints.Count);
        var selectedPoint = cardPlacePoints[randomPoint];

        if (enemyAiType == AIType.PlaceFromDeck || enemyAiType == AIType.HandRandomPlace)
        {
            cardPlacePoints.Remove(selectedPoint);
            
            while (selectedPoint.activeCard != null && cardPlacePoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPlacePoints.Count);
                selectedPoint = cardPlacePoints[randomPoint];
            
                cardPlacePoints.RemoveAt(randomPoint);
            }
        }

        switch (enemyAiType)
        {
            case AIType.PlaceFromDeck:
                if(selectedPoint.activeCard == null)
                {
                    var newCard = Instantiate(cardToSpawn, spawnPoint.position, spawnPoint.rotation);
                    newCard.cardSO = _activeCards[0];
                    _activeCards.RemoveAt(0);
            
                    newCard.SetupCard();
                    var selectedPointTransform = selectedPoint.transform;
                    newCard.MoveToPosition(selectedPointTransform .position, selectedPointTransform .rotation);

                    selectedPoint.activeCard = newCard;
                }
                break;
            case AIType.HandRandomPlace:
                break;
            case AIType.HandDefensive:
                break;
            case AIType.HandAttacking:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        yield return new WaitForSeconds(0.5f);
        
        BattleController.Instance.AdvanceTurn();
    }

    private void SetupHand()
    {
        for (var i = 0; i < startHandSize; i++)
        {
            if (_activeCards.Count == 0)
            {
                SetupDeck();
            }
            
            _cardsInHand.Add(_activeCards[0]);
            _activeCards.RemoveAt(0);
        }
    }
}
