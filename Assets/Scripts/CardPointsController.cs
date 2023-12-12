 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{
    public static CardPointsController Instance;

    public CardPlacePoint[] playerCardPoints, enemyCardPoints;

    [SerializeField] private float timeBetweenAttacks = 0.25f;
    
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
        
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerAttackCoroutine());
    }

    private IEnumerator PlayerAttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (var i = 0; i < playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                if (enemyCardPoints[i].activeCard != null)
                {
                    enemyCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attackPower);
                    
                }
                else
                {
                    BattleController.Instance.DamageEnemy(playerCardPoints[i].activeCard.attackPower);
                }
                
                playerCardPoints[i].activeCard.Animator.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }
        
        CheckAssignedCards();
        
        BattleController.Instance.AdvanceTurn();
    }
    
    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCoroutine());
    }

    private IEnumerator EnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (var i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                if (playerCardPoints[i].activeCard != null)
                {
                    playerCardPoints[i].activeCard.DamageCard(enemyCardPoints[i].activeCard.attackPower);
                    
                }
                else
                {
                    BattleController.Instance.DamagePlayer(enemyCardPoints[i].activeCard.attackPower);
                }
                
                enemyCardPoints[i].activeCard.Animator.SetTrigger("Attack");
                
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }
        
        CheckAssignedCards();
        
        BattleController.Instance.AdvanceTurn();
    }

    public void CheckAssignedCards()
    {
        foreach (var playerCardPoint in playerCardPoints)
        {
            if (playerCardPoint.activeCard != null && playerCardPoint.activeCard.currentHealth <= 0)
            {
                playerCardPoint.activeCard = null;
            }
        }

        foreach (var enemyCardPoint in enemyCardPoints)
        {
            if (enemyCardPoint.activeCard != null && enemyCardPoint.activeCard.currentHealth <= 0)
            {
                enemyCardPoint.activeCard = null;
            }
        }
    }
}
