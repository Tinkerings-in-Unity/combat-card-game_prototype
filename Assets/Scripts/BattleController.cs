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

    public int playerMana, enemyMana;

    public TurnOrder currentPhase;

    public Transform discardPoint;

    public int playerHealth;
    public int enemyHealth;

    [SerializeField] private int startingCardsAmount = 5;
    [SerializeField] private int cardsToDrawPerTurn = 2;
    private int _currentPlayerMaxMana, _currentEnemyMaxMana;

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
        
        UIController.Instance.SetPlayerHealthText(playerHealth);
        UIController.Instance.SetEnemyHealthText(enemyHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     AdvanceTurn();
        // }
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
                
                CardPointsController.Instance.PlayerAttack();
                
                break;
            case TurnOrder.EnemyActive:
                
                EnemyController.Instance.StartAction();
                
                break; 
            case TurnOrder.EnemyCardAttacks:
                
                CardPointsController.Instance.EnemyAttack();
                
                break;
        }
    }

    public void EndPlayerTurn()
    {
        UIController.Instance.endTurnButton.SetActive(false);
        UIController.Instance.drawCardButton.SetActive(false);
        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damageAmount;

            if (playerHealth <= 0)
            {
                playerHealth = 0;
                
                //End battle
            }
            
            UIController.Instance.SetPlayerHealthText(playerHealth);
            
            var damageText = Instantiate(UIController.Instance.PlayerDamageText, UIController.Instance.transform);
            damageText.SetDamage(damageAmount);
            damageText.gameObject.SetActive(true);
        }
    }
    
    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damageAmount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                
                //End battle
            }
            
            UIController.Instance.SetEnemyHealthText(enemyHealth);
            
            var damageText = Instantiate(UIController.Instance.EnemyDamageText, UIController.Instance.transform);
            damageText.SetDamage(damageAmount);
            damageText.gameObject.SetActive(true);
        }
    }
}
