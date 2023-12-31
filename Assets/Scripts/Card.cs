using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public bool isPlayer;
     
    public int currentHealth, attackPower, manaCost = 0;

    public TMP_Text healthText, attackText, costText, nameText, actionText, loreText;

    public Image characterImage, backgroundImage = null;

    public float moveSpeed = 5f, rotateSpeed = 540f;

    public bool inHand = false;
    
    public int handPosition;

    public LayerMask desktopLayer, placementLayer;

    public Animator Animator => animator;

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    private HandController _handController = null;
    private bool _isSelected = false;
    private Collider _cardCollider;
    private bool _justPressed = false;
    [SerializeField] private CardPlacePoint assignedPlacePoint = null;
    [SerializeField] private Animator animator = null;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_targetPosition == Vector3.zero)
        {
            _targetPosition = transform.position;
            _targetRotation = transform.rotation;
        }
        
        SetupCard();

        _handController = FindObjectOfType<HandController>();
        _cardCollider = GetComponent<Collider>();
    }

    public void SetupCard()
    {
        currentHealth = cardSO.currentHealth;
        attackPower = cardSO.attackPower;
        manaCost = cardSO.manaCost;
        
        UpdateCardDisplay();

        nameText.text = cardSO.cardName;
        actionText.text = cardSO.actionDescription;
        loreText.text = cardSO.cardLore;

        characterImage.sprite = cardSO.characterSprite;
        backgroundImage.sprite = cardSO.backgroundSprite;
    }

    public void UpdateCardDisplay()
    {
        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        costText.text = manaCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, _targetRotation, rotateSpeed * Time.deltaTime);


        if (_isSelected)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 100f, desktopLayer))
            {
                MoveToPosition(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
            
            if (Input.GetMouseButtonDown(0) && !_justPressed )
            {
                if (Physics.Raycast(ray, out hit, 100f, placementLayer) && BattleController.Instance.currentPhase == TurnOrder.PlayerActive)
                {
                    var selectedCardPlacementPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if (selectedCardPlacementPoint.activeCard == null && selectedCardPlacementPoint.isPlayerPoint)
                    {
                        if (BattleController.Instance.playerMana >= manaCost)
                        {
                            selectedCardPlacementPoint.activeCard = this;
                            assignedPlacePoint = selectedCardPlacementPoint;

                            MoveToPosition(selectedCardPlacementPoint.transform.position, Quaternion.identity);

                            inHand = false;
                            _isSelected = false;

                            _handController.RemoveCardFromHand(this);
                            
                            BattleController.Instance.SpendPlayerMana(manaCost);
                        }
                        else
                        {
                            UIController.Instance.ShowLowManaWarning();
                            ReturnToHand();
                        }
                    }
                    else
                    {
                        ReturnToHand();
                    }
                }
                else
                {
                    ReturnToHand();
                }
            }
            
        }

        _justPressed = false;
    }

    public void MoveToPosition(Vector3 positionToMoveTo, Quaternion targetRotation)
    {
        _targetPosition = positionToMoveTo;
        _targetRotation = targetRotation;
    }

    private void OnMouseOver()
    {
        if (inHand && isPlayer)
        {
            MoveToPosition(_handController.cardPositions[handPosition] + new Vector3(0f,0.2f,0.7f), _targetRotation);
        }
    }

    private void OnMouseExit()
    {
        if (inHand && ! _isSelected && isPlayer)
        {
            MoveToPosition(_handController.cardPositions[handPosition], _targetRotation);
        }
    }

    private void OnMouseDown()
    {
        if (!inHand || BattleController.Instance.currentPhase != TurnOrder.PlayerActive || !isPlayer) return;
        
        _isSelected = true;
        _cardCollider.enabled = false;

        _justPressed = true;
            
        MoveToPosition(_handController.cardPositions[handPosition] + new Vector3(0f,0.1f,0.7f), _targetRotation);
    }

    public void ReturnToHand()
    {
        _isSelected = false;
        _cardCollider.enabled = true;
        
        MoveToPosition(_handController.cardPositions[handPosition], _targetRotation);
    }

    public void DamageCard(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (inHand)
            {
                assignedPlacePoint.activeCard = null;
            }

            MoveToPosition(BattleController.Instance.discardPoint.position, BattleController.Instance.discardPoint.rotation);
            
            animator.SetTrigger("Jump");
            
            Destroy(gameObject, 5f);
        }
        
        animator.SetTrigger("Hurt");

        UpdateCardDisplay();
    }
}
