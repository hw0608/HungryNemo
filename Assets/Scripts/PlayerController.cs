using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Rendering.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public ParticleSystem heartParticle;
    ParticleSystem starParticle;

    public Material defaultMaterial;
    public Material invincibleMaterial;

    float vx = 0;
    float vy = 0;
    float timer = 0;

    public bool isInvincible;
    float invincibleTimer = 0;
    float invincibleTime = 5f;

    public GameObject bubble;

    bool speedUp;
    public float oldSpeed;
    float speedUpTimer = 0;
    float speedUpTime = 5f;

    Coroutine co_changingColor = null;
    bool isChangingColor => co_changingColor != null;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        heartParticle = GetComponentsInChildren<ParticleSystem>()[0];
        starParticle = GetComponentsInChildren<ParticleSystem>()[1];
        isInvincible = false;
    }

    public void InvincibleOn()
    {
        starParticle.Play();
        AudioManager.instance.SfxPlay(AudioManager.Sfx.Star);
        if (isInvincible)
        {
            invincibleTimer = 0;
            return;
        }
        isInvincible = true;
        spriteRenderer.material = invincibleMaterial;
    }

    public void InvincibleOff()
    {
        isInvincible = false;
        invincibleTimer = 0;
        spriteRenderer.material = defaultMaterial;
    }

    public void SpeedUpOn()
    {
        AudioManager.instance.SfxPlay(AudioManager.Sfx.SpeedUp);
        if (speedUp)
        {
            speedUpTimer = 0;
            return;
        }
        oldSpeed = speed;
        speedUp = true;
        speed *= 1.5f;
    }

    public void SpeedUpOff()
    {
        speedUpTimer = 0;
        speed = oldSpeed;
        speedUp = false;
    }

    public void BubbleOn()
    {
        AudioManager.instance.SfxPlay(AudioManager.Sfx.Bubble);
        bubble.GetComponent<Animator>().SetBool("Pop", false);
        bubble.SetActive(true);
    }

    public void BubbleOff()
    {
        AudioManager.instance.SfxPlay(AudioManager.Sfx.Pop);
        bubble.GetComponent<Animator>().SetBool("Pop", true);
        bubble.GetComponent<Animator>().keepAnimatorStateOnDisable = false;
        bubble.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.isDead || GameManager.instance.isCleared)
        {
            gameObject.layer = 8;
            return;
        }

        if (GameManager.instance.statePanel.hungerGauge.fillAmount <= 0)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                timer = 0;
                Hit("Hunger");
            }
        }

        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= invincibleTime)
            {
                InvincibleOff();
            }
        }

        if (speedUp)
        {
            speedUpTimer += Time.deltaTime;
            if (speedUpTimer >= speedUpTime)
            {
                SpeedUpOff();
            }
        }

        vx = Input.GetAxisRaw("Horizontal") * speed;
        vy = Input.GetAxisRaw("Vertical") * speed;

        if ((transform.localScale.x > 0 && vx < 0) || transform.localScale.x < 0 && vx > 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        if (vx != 0 || vy != 0)
        {
            animator.SetTrigger("Swim");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isDead)
        {
            rigid.linearVelocity = Vector2.zero;
            return;
        }

        rigid.linearVelocity = (new Vector3(vx, vy, 0));
    }

    public void Hit(string str = "")
    {
        if (isInvincible)
        {
            return;
        }

        AudioManager.instance.SfxPlay(AudioManager.Sfx.Hit);

        if (bubble.activeSelf && str != "Hunger")
        {
            BubbleOff();
            return;
        }

        GameManager.instance.DecreaseHp(1);
        TransitionColor(Color.red, 2f);
        gameObject.layer = 7;
        Invoke("OffHit", 0.5f);
    }

    void OffHit()
    {
        gameObject.layer = 6;
    }

    Coroutine TransitionColor(Color color, float speed = 1f)
    {
        if (isChangingColor)
            StopCoroutine(co_changingColor);
        co_changingColor = StartCoroutine(ChangingColor(color, speed));
        return co_changingColor;
    }

    IEnumerator ChangingColor(Color color, float speedMultiplier = 1f)
    {
        Color oldColor = Color.white;
        float colorPercent = 0;
        while (colorPercent < 1)
        {
            colorPercent += 2f * speedMultiplier * Time.deltaTime;
            spriteRenderer.color = Color.Lerp(oldColor, color, colorPercent);
            yield return null;
        }
        colorPercent = 0;

        while (colorPercent < 1)
        {
            colorPercent += 2f * speedMultiplier * Time.deltaTime;
            spriteRenderer.color = Color.Lerp(color, oldColor, colorPercent);
            yield return null;
        }

        co_changingColor = null;
    }

    public void Die()
    {
        rigid.gravityScale = 1f;
        animator.SetTrigger("Dead");
        spriteRenderer.flipY = true;
        Debug.Log("Die");
    }
}
