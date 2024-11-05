using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    enum Items
    {
        Star,
        Heart,
        Bubble,
        SpeedUp
    }

    SpriteRenderer renderer;
    bool isPlayerEnter;
    bool isUsed;

    public Sprite closed;
    public Sprite opened;
    public GameObject itemText;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        isPlayerEnter = false;
        isUsed = false;
    }

    private void OnEnable()
    {
        renderer.sprite = closed;
        isPlayerEnter = false;
        isUsed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerEnter = false;
        }
    }

    void Update()
    {
        if (!isUsed && isPlayerEnter && Input.GetButtonDown("Jump"))
        {
            UseItem();
        }
    }

    void UseItem()
    {
        isUsed = true;
        renderer.sprite = opened;
        GetComponent<ObjTimer>().time = 2f;
        GetComponent<ObjTimer>().timer = 0;

        Items item = (Items)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Items)).Length));

        switch (item)
        {
            case Items.Star:
                UseStar();
                break;
            case Items.Heart:
                UseHeart();
                CreateText("hp+", new Color32(255, 194, 200, 255));
                break;
            case Items.Bubble:
                UseBubble();
                break;
            case Items.SpeedUp:
                UseSpeedUp();
                CreateText("speed up!", new Color32(194, 230, 255, 255));
                break;
        }
    }

    void CreateText(string str, Color32 color)
    {
        itemText.GetComponent<TextMeshPro>().text = str;
        itemText.GetComponent<TextMeshPro>().color = color;
        GameObject t = Instantiate(itemText);
        t.transform.position = GameManager.instance.player.transform.position + new Vector3(0, 1, 0);
    }

    void UseStar()
    {
        GameManager.instance.player.InvincibleOn();
    }

    void UseHeart()
    {
        GameManager.instance.IncreaseHp(1);
    }

    void UseBubble()
    {
        GameManager.instance.player.BubbleOn();
    }

    void UseSpeedUp()
    {
        GameManager.instance.player.SpeedUpOn();
    }
}
