using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float health;
    [SerializeField] private float damage;
    
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
    [SerializeField] private bool isHit;
    [SerializeField] private bool isDead;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool onAttackReset;

    [Header("Debugging")]
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject target;
    [SerializeField] private float hitDuration;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isHit && !isDead)
        {
            CheckForTarget();

            if (target == null)
            {
                if (!isPatroling && !isProtecting)
                {
                    StartCoroutine(Patrol());
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
        direction = new Vector3(transform.position.x + Random.Range(-patrolingDistance, patrolingDistance), transform.position.y + Random.Range(-patrolingDistance, patrolingDistance), 0f);

        if(direction.y > 6.4f)
        {
            direction = new Vector3(direction.x, 6.4f, 0f);
        }

        else if(direction.y < -22f)
        {
            direction = new Vector3(direction.x, -22f, 0f);
        }

        if(direction.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        isPatroling = true;
        animator.SetInteger("movingState", 1);

        while (!isProtecting && target == null && !isHit)
        {
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
        target.GetComponent<fighterScript>().TakeDamage(damage);
        animator.SetInteger("fightState", 1);

        canAttack = false;
        onAttackReset = true;

        yield return new WaitForSeconds(attackResetTime);

        onAttackReset = false;
    }

    public void GetHit(float damage)
    {
        Debug.Log("Enemy takes damage!");

        health -= damage;

        if(health <= 0)
        {
            Die();
        }

        else
        {
            animator.SetInteger("getAttackedState", 1);
            StartCoroutine(HitReset());
        }
    }

    private IEnumerator HitReset()
    {
        canAttack = false;
        isHit = true;

        yield return new WaitForSeconds(hitDuration);

        animator.SetInteger("getAttackedState", 0);
        isHit = false;
    }

    private void Die()
    {
        isDead = true;

        // animation

        GetComponent<Collider2D>().enabled = false;
    }
}
