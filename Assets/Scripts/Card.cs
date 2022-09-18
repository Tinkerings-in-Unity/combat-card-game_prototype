using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;
     
    public int currentHealth, attackPower, manaCost = 0;

    public TMP_Text healthText, attackText, costText, nameText, actionText, loreText;

    public Image characterImage, backgroundImage = null;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        currentHealth = cardSO.currentHealth;
        attackPower = cardSO.attackPower;
        manaCost = cardSO.manaCost;
        
        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        costText.text = manaCost.ToString();

        nameText.text = cardSO.cardName;
        actionText.text = cardSO.actionDescription;
        loreText.text = cardSO.cardLore;

        characterImage.sprite = cardSO.characterSprite;
        backgroundImage.sprite = cardSO.backgroundSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
