using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Detection")]
    [SerializeField] private float enemyDetectionRadius;
    [SerializeField] private LayerMask enemyMask;

    [Header("Patroling")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float patrolingDistance;
    [SerializeField] private float patrolUpdateTime;
    [SerializeField] private float protectingTime;

    [Header("Attacking")]
    [SerializeField] private float attackResetTime;

    [Header("Enemy States")]
    [SerializeField] private bool isPatroling;
    [SerializeField] private bool isProtecting;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool onAttackReset;

    [Header("Debugging")]
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject target;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckForTarget();
        
        if(target == null)
        {
            if (!isPatroling && !isProtecting)
            {
                StartCoroutine(Patrol());
                Debug.Log("started patroling");
            }
        }

        else
        {
            if (canAttack && !onAttackReset)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private void CheckForTarget()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius, enemyMask);

        if(enemies.Length > 0)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].CompareTag("Player"))
                {
                    target = enemies[i].gameObject;
                    canAttack = true;
                }
            }
        }

        else
        {
            target = null;
            canAttack = false;
        }
    }

    private IEnumerator Patrol()
    {
        direction = new Vector3(transform.position.x + Random.Range(-patrolingDistance, patrolingDistance), transform.position.y + Random.Range(-patrolingDistance, patrolingDistance));

        if(direction.y > 23f)
        {
            direction = new Vector3(direction.x, 23f);
        }

        else if(direction.y < -7.7f)
        {
            direction = new Vector3(direction.x, -7.7f);
        }

        isPatroling = true;
        animator.SetInteger("movingState", 1);

        while (!isProtecting && target == null)
        {
            Debug.Log("patroling");
            transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);

            if (transform.position == direction)
            {
                StartCoroutine(ProtectPosition());
            }

            yield return new WaitForSeconds(patrolUpdateTime);
        }

        isPatroling = false;
    }

    private IEnumerator ProtectPosition()
    {
        isProtecting = true;
        animator.SetInteger("movingState", 0);
        yield return new WaitForSeconds(protectingTime + Random.Range(-0.5f, 0.5f));
        isProtecting = false;
    }

    private IEnumerator Attack()
    {
        Debug.Log("Attacking towards player!");
        canAttack = false;
        onAttackReset = true;

        yield return new WaitForSeconds(attackResetTime);

        onAttackReset = false;
    }
}
