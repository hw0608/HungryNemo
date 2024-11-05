using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateDisplayer : MonoBehaviour
{
    public List<GameObject> LifeImages;
    public Sprite filledSprite;
    public Sprite unfilledSprite;
    public Sprite happy;
    public Sprite soso;
    public Sprite bad;
    public Sprite dead;
    public Sprite invincible;

    public Image emote;
    public Image hungerGauge;

    float endureTime;
    public float timer = 0;

    public void SetLives(int life)
    {
        for (int i = 0; i < LifeImages.Count; i++)
        {
            if (i < life)
            {
                LifeImages[i].GetComponent<Image>().sprite = filledSprite;
            }
            else
            {
                LifeImages[i].GetComponent<Image>().sprite = unfilledSprite;
            }
        }
    }

    private void Start()
    {
        endureTime = 10f;
    }

    private void Update()
    {
        if (GameManager.instance.isDead)
        {
            emote.sprite = dead;
            return;
        }

        timer += Time.deltaTime;
        hungerGauge.fillAmount = 1 - timer / endureTime;

        if (GameManager.instance.player.isInvincible)
        {
            hungerGauge.fillAmount = 1;
        }

        if (hungerGauge.fillAmount >= 2 / 3f)
        {
            emote.sprite = happy;
        }
        else if (hungerGauge.fillAmount >= 1 / 3f)
        {
            emote.sprite = soso;
        }
        else
        {
            emote.sprite = bad;
        }

        if (GameManager.instance.player.isInvincible)
        {
            emote.sprite = invincible;
        }
    }
}
