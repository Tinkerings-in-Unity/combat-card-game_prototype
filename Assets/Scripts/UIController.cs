using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance = null;
    public GameObject drawCardButton;
    public GameObject endTurnButton;
    public UIDamageIndicator PlayerDamageText => playerDamageText;
    public UIDamageIndicator EnemyDamageText => enemyDamageText;

    
    [SerializeField] private TMP_Text playerManaText, playerHealthText, enemyHealthText;
    [SerializeField] private UIDamageIndicator playerDamageText, enemyDamageText;
    [SerializeField] private GameObject lowManaWarningLabel;
    [SerializeField] private float lowManaWarningTime;
    private float _lowManaWarningCounter;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_lowManaWarningCounter > 0f)
        {
            _lowManaWarningCounter -= Time.deltaTime;

            if (_lowManaWarningCounter <= 0)
            {
                lowManaWarningLabel.SetActive(false);
            }
        }
    }

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void SetPlayerHealthText(int healthAmount)
    {
        playerHealthText.text = "Player Health: " + healthAmount;
    }
    
    public void SetEnemyHealthText(int healthAmount)
    {
        enemyHealthText.text = "Enemy Health: " + healthAmount;
    }

    public void ShowLowManaWarning()
    {
        lowManaWarningLabel.SetActive(true);
        _lowManaWarningCounter = lowManaWarningTime;
    }
    
    public void DrawCard()
    {
        if (!DeckController.Instance.DrawCardForMana())
        {
            drawCardButton.SetActive(false);
        }
    }

    public void EndPlayerTurn()
    {
        BattleController.Instance.EndPlayerTurn();
    }
}
