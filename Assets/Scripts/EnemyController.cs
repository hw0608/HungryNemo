using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        Idle,
        Swim,
        Follow,
        Escape
    }

    public float speed = 1f;
    public int level;
    float transitionStateTime;

    State state;

    Rigidbody2D rigid;
    Animator animator;

    public Vector2 spawnPosition;
    Vector3 target;
    Vector2 oldDir;
    Vector2 dirVec;

    float timer;

    void Awake()
    {
        spawnPosition = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        timer = 0;
        StartSwim();
    }

    private void OnEnable()
    {
        StartSwim();
    }

    private void Update()
    {
        if (GameManager.instance.isCleared)
        {
            return;
        }

        if (level == 0)
        {
            timer += Time.deltaTime;

            if (timer >= 8f)
            {
                timer = 0;
                GameManager.instance.poolManager.activeCount--;
                gameObject.SetActive(false);
            }
        }
        if (transform.position.x < -12 || transform.position.x > 12 || transform.position.y > 10 || transform.position.y < -8)
        {
            GameManager.instance.poolManager.activeCount--;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance.isDead || GameManager.instance.isCleared)
            {
                return;
            }
            target = collision.gameObject.transform.position;
            if (GameManager.instance.level >= level)
            {
                state = State.Escape;
            }
            else
            {
                state = State.Follow;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            state = State.Swim;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.instance.level < level)
            {
                collision.gameObject.GetComponent<PlayerController>().Hit();
            }
            else
            {
                GameManager.instance.statePanel.timer = 0;
                GameManager.instance.poolManager.activeCount--;
                GameManager.instance.IncreaseExp(level + 1);
                gameObject.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (level == 0) return;
        Vector2 moveVec = Vector2.zero;
        switch (state)
        {
            case State.Follow:
                animator.SetTrigger("Swim");
                dirVec = target - transform.position;
                moveVec = dirVec.normalized * speed * 1.5f * Time.fixedDeltaTime;
                if ((moveVec.x < 0 && oldDir.x > 0) || (moveVec.x > 0 && oldDir.x < 0))
                {
                    transform.localScale = new Vector2(-transform.localScale.x, 1);
                }
                rigid.MovePosition(rigid.position + moveVec);
                if (dirVec.magnitude < 0.1f)
                {
                    state = State.Swim;
                }
                break;
            case State.Escape:
                animator.SetTrigger("Swim");
                dirVec = transform.position - target;
                if (dirVec.x<0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                else if (dirVec.x > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector2(1, 1);
                }
                moveVec = dirVec.normalized * speed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + moveVec);
                break;
            case State.Idle:
                transitionStateTime -= Time.deltaTime;
                if (transitionStateTime < 0)
                {
                    StartSwim();
                }
                break;
            case State.Swim:
                transitionStateTime -= Time.deltaTime;
                moveVec = dirVec * speed * Time.fixedDeltaTime;

                if (transform.position.y > 5 || transform.position.y < -3)
                {
                    StartSwim(false);
                }

                rigid.MovePosition(rigid.position + moveVec);

                if (transitionStateTime < 0)
                {
                    StartIdle();
                }
                break;
        }

        oldDir = dirVec.normalized;
    }

    void StartIdle()
    {
        state = State.Idle;
        transitionStateTime = UnityEngine.Random.Range(1.0f, 1.5f);
        animator.SetTrigger("Idle");
    }

    void StartSwim(bool turn = true)
    {
        if (turn)
        {
            transform.localScale = new Vector2(UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1, 1);
        }
        
        if (transform.position.x > 10)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (transform.position.x < -10)
        {
            transform.localScale = new Vector2(1, 1);
        }

        if (transform.position.y >= 5)
        {
            dirVec = new Vector2(transform.localScale.x, UnityEngine.Random.Range(-1f, -0f));
        }
        else if (transform.position.y <= -3)
        {
            dirVec = new Vector2(transform.localScale.x, UnityEngine.Random.Range(0f, 1f));
        }
        else
        {
            dirVec = new Vector2(transform.localScale.x, UnityEngine.Random.Range(-2f, 2f));
        }
        state = State.Swim;
        transitionStateTime = UnityEngine.Random.Range(4.0f, 7.0f);
        animator.SetTrigger("Swim");
    }

}
