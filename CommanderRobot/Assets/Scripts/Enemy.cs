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
    [SerializeField] private float attackRadius;
    [SerializeField] private float moveToEnemySpeed;
    [SerializeField] private float moveAnimationSpeed = 1;
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
    //[SerializeField] private bool foundAlly;
    [SerializeField] private bool foundEnemy;
    [SerializeField] private bool moveTowardsEnemy;

    [Header("Debugging")]
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject target;
    [SerializeField] private float hitDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private Animator animator;
    //[SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject player;

    [SerializeField] ParticleSystem DeathExplosio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
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

                else if (!moveTowardsEnemy)
                {
                    moveTowardsEnemy = true;
                    StartCoroutine(MoveTowardsEnemy());
                }
            }
        }
    }

    private void CheckForTarget()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < enemyDetectionRadius)
        {
            target = player;
            foundEnemy = true;
        }

        else
        {
            target = null;
            foundEnemy = false;
        }

        /*Collider2D[] enemies = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 10.22136f), enemyDetectionRadius, enemyMask);

        if(enemies.Length > 0)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].CompareTag("Player"))
                {
                    target = enemies[i].gameObject;
                    foundEnemy = true;
                }
            }
        }

        else
        {
            target = null;
            foundEnemy = false;
            //animator.SetInteger("movingState", 0);

            if (!isProtecting)
            {
                Collider2D[] cops = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 10.22136f), enemyDetectionRadius, LayerMask.GetMask("enemy"));

                if (cops.Length > 0)
                {
                    for (int i = 0; i < cops.Length; i++)
                    {
                        if (cops[i].gameObject != gameObject)
                        {
                            foundAlly = true;
                            return;
                        }

                        else
                        {
                            foundAlly = false;
                        }
                    }
                }
            }*/
    }

    private IEnumerator Patrol()
    {
        direction = new Vector3(transform.position.x + Random.Range(-patrolingDistance, patrolingDistance), transform.position.y + Random.Range(-patrolingDistance, patrolingDistance));

        if(direction.y > 22.4f)
        {
            direction = new Vector3(direction.x, 22.4f);
        }

        else if(direction.y < -25f)
        {
            direction = new Vector3(direction.x, -25f);
        }

        if(direction.x > 250f)
        {
            direction = new Vector3(250f, direction.y);
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

        while (!isProtecting && target == null && !isHit && !foundEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);

            //rb.velocity = direction * moveSpeed;

            renderer.sortingOrder = (int)transform.position.y;

            if (Vector2.Distance(transform.position, direction) < 2f/* || foundAlly*/)
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

        /*if (foundAlly)
        {
            foundAlly = false;

            Collider2D ally = Physics2D.OverlapCircle(transform.position, enemyDetectionRadius, LayerMask.GetMask("enemy"));

            Vector3 newDirection = new Vector3(-ally.transform.position.x, -ally.transform.position.y);

            if (newDirection.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            float moveTime = 0.5f;
            
            while (moveTime > 0f)
            {
                Debug.Log("move away");
                transform.position = Vector3.MoveTowards(transform.position, newDirection, moveSpeed * Time.deltaTime);
                moveTime -= 0.01f;

                yield return new WaitForSeconds(0.01f);
            }
        }*/

        animator.SetInteger("movingState", 0);
        yield return new WaitForSeconds(protectingTime + Random.Range(-1.5f, 1.5f));
        isProtecting = false;
    }

    private IEnumerator MoveTowardsEnemy()
    {
        animator.SetInteger("movingState", 1);
        animator.SetFloat("moveSpeed", moveAnimationSpeed);

        while (!canAttack)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveToEnemySpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, player.transform.position) < attackRadius)
            {
                canAttack = true;
            }

            yield return new WaitForSeconds(patrolUpdateTime);
        }
    }

    private IEnumerator Attack()
    {
        if(target.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        Debug.Log("attacked");
        target.GetComponent<fighterScript>().TakeDamage(damage);
        animator.SetInteger("movingState", 4);
        animator.SetInteger("fightState", 1);

        canAttack = false;
        onAttackReset = true;

        StartCoroutine(AttackReset());

        yield return new WaitForSeconds(attackResetTime);

        onAttackReset = false;
    }

    private IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(attackDuration);

        animator.SetInteger("movingState", 0);
        animator.SetInteger("fightState", 0);
    }

    public void GetHit(float damage)
    {
        Debug.Log("Enemy takes damage!");

        //DeathExplosio.Play();

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
        canAttack = false;
        isPatroling = false;
        isProtecting = false;
        isHit = false;

        //transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        animator.SetBool("dead", true);

        GetComponent<Collider2D>().enabled = false;
    }
}
