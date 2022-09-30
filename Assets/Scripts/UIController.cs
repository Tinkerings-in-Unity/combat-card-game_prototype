using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance = null;
    
    [SerializeField] private TMP_Text playerManaText;
    [SerializeField] private GameObject lowManaWarningLabel;
    [SerializeField] private float lowManaWarningTime;
    public GameObject drawCardButton;
    public GameObject endTurnButton;
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
