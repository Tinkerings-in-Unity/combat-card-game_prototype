using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnOrder
{
    PlayerActive, PlayerCardAttacks, EnemyActive, EnemyCardAttacks
}

public class BattleController : MonoBehaviour
{
    public static BattleController Instance = null;
    
    public int startingMana = 4, maxMana = 12;

    public int playerMana;

    public TurnOrder currentPhase;

    [SerializeField] private int startingCardsAmount = 5;
    [SerializeField] private int cardsToDrawPerTurn = 2;
    private int _currentPlayerMaxMana;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentPlayerMaxMana = startingMana;
        
        FillPlayerMana();
        
        DeckController.Instance.DrawMultipleCards(startingCardsAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana -= amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }
        
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    public void FillPlayerMana()
    {
        playerMana = _currentPlayerMaxMana;
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn()
    {
        currentPhase = (int) currentPhase + 1 >= Enum.GetValues(typeof(TurnOrder)).Length? 0 : currentPhase + 1;

        switch (currentPhase)
        {
            case TurnOrder.PlayerActive:
                
                UIController.Instance.endTurnButton.SetActive(true);
                UIController.Instance.drawCardButton.SetActive(true);

                if (_currentPlayerMaxMana < maxMana)
                {
                    _currentPlayerMaxMana++;
                }
                
                FillPlayerMana();
                
                DeckController.Instance.DrawMultipleCards(cardsToDrawPerTurn);

                break;
            case TurnOrder.PlayerCardAttacks:
                
                Debug.Log("Skipping player card attacks");
                AdvanceTurn();
                
                break;
            case TurnOrder.EnemyActive:
                
                Debug.Log("Skipping enemy actions");
                AdvanceTurn();
                
                break; 
            case TurnOrder.EnemyCardAttacks:
                
                Debug.Log("Skipping enemy card attacks");
                AdvanceTurn();
                
                break;
        }
    }

    public void EndPlayerTurn()
    {
        UIController.Instance.endTurnButton.SetActive(false);
        UIController.Instance.drawCardButton.SetActive(false);
        AdvanceTurn();
    }
}
