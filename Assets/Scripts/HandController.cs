using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController Instance;
    
    public List<Card> heldCards = new List<Card>();

    public Transform minPos, maxPos;
    public List<Vector3> cardPositions = new List<Vector3>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionsInHand();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardPositionsInHand()
    {
        cardPositions.Clear();

        var distanceBetweenCards = Vector3.zero;

        if (heldCards.Count > 1)
        {
            distanceBetweenCards = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }
        
        for (var i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenCards * i));

            heldCards[i].MoveToPosition(cardPositions[i], minPos.rotation);

            heldCards[i].inHand = true;
            
            heldCards[i].handPosition = i;
        }
    }

    public void RemoveCardFromHand(Card cardToRemove)
    {
        if (heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.Remove(cardToRemove);
        }
        else
        {
            Debug.LogError("Card at position " + cardToRemove.handPosition + "is not the card being removed");
        }
        
        SetCardPositionsInHand();
    }

    public void AddCardToHand(Card cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPositionsInHand();
    }
}
