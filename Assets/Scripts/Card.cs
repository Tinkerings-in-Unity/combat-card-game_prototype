using System;
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

    public float moveSpeed = 5f, rotateSpeed = 540f;

    public bool inHand = false;
    
    public int handPosition;

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    private HandController _handController = null;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupCard();

        _handController = FindObjectOfType<HandController>();
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
        transform.position = Vector3.Lerp(transform.position, _targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, _targetRotation, rotateSpeed * Time.deltaTime); 
    }

    public void MoveToPosition(Vector3 positionToMoveTo, Quaternion targetRotation)
    {
        _targetPosition = positionToMoveTo;
        _targetRotation = targetRotation;
    }

    private void OnMouseOver()
    {
        if (inHand)
        {
            MoveToPosition(_handController.cardPositions[handPosition] + new Vector3(0f,0.5f,0.7f), _targetRotation);
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPosition(_handController.cardPositions[handPosition], _targetRotation);
        }
    }
}
