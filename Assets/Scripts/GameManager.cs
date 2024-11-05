using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;
    public PoolManager poolManager;
    public Animator levelUpEffect;
    public StateDisplayer statePanel;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public int hp;
    int exp;
    public int level;
    const int MAX_HP = 3;
    const int MAX_LEVEL = 6;
    public bool isDead;
    public bool isCleared;


    public int maxLevel
    {
        get { return MAX_LEVEL; }
    }
    int[] requiredExp;
    float[] scales;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        else
        {
            instance = this;
            Application.targetFrameRate = 60;
        }
    }

    private void Start()
    {
        level = 0;
        hp = MAX_HP;
        exp = 0;
        requiredExp = new int[MAX_LEVEL + 1] { 10, 30, 50, 90, 150, 220, 220 };
        scales = new float[MAX_LEVEL + 1] { 1f, 1.2f, 1.5f, 1.8f, 2.2f, 2.5f, 2.5f };
    }

    public void DecreaseHp(int health)
    {
        hp -= health;
        statePanel.SetLives(hp);
        if (hp <= 0)
        {
            GameOver();
        }
    }

    public void IncreaseHp(int health)
    {
        AudioManager.instance.SfxPlay(AudioManager.Sfx.Heal);
        hp = Mathf.Min(hp + health, MAX_HP);
        player.heartParticle.Play();
        statePanel.SetLives(hp);
    }

    public void IncreaseExp(int exp)
    {
        AudioManager.instance.SfxPlay(AudioManager.Sfx.Eat);
        this.exp += exp;
        if (this.exp >= requiredExp[level])
        {
            if (level < MAX_LEVEL)
            {
                LevelUp();
            }
            else
            {
                GameClear();
            }
        }
    }

    public void LevelUp()
    {
        if (level + 1 > MAX_LEVEL) return;

        AudioManager.instance.SfxPlay(AudioManager.Sfx.LevelUp);
        levelUpEffect.SetTrigger("LevelUp");
        level++;
        player.speed += 2;
        player.oldSpeed += 2;
        player.gameObject.transform.localScale = new Vector2(scales[level], scales[level]);
    }

    void GameOver()
    {
        AudioManager.instance.SfxPlay(AudioManager.Sfx.GameOver);
        gameOverPanel.SetActive(true);
        isDead = true;
        player.Die();
    }

    void GameClear()
    {
        SaveHighScore();
        AudioManager.instance.SfxPlay(AudioManager.Sfx.GameClear);
        gameClearPanel.SetActive(true);
        isCleared = true;
    }

    void SaveHighScore()
    {
        int score = exp;

        string currentScoreString = score.ToString();
        string savedScoreString = PlayerPrefs.GetString("Highscores", "");

        if (savedScoreString == "")
        {
            PlayerPrefs.SetString("Highscores", currentScoreString);
        }

        else
        {
            string[] scoreArray = savedScoreString.Split(',');
            List<string> scoreList = new List<string>(scoreArray);

            for (int i = 0; i < scoreList.Count; i++)
            {
                float savedScore = float.Parse(scoreList[i]);
                if (savedScore < score)
                {
                    scoreList.Insert(i, currentScoreString);
                    break;
                }
            }

            if (scoreArray.Length == scoreList.Count)
            {
                scoreList.Add(currentScoreString);
            }

            if (scoreList.Count > 10)
            {
                scoreList.RemoveAt(10);
            }

            string result = string.Join(",", scoreList.ToArray());
            PlayerPrefs.SetString("HighScores", result);
        }
        PlayerPrefs.Save();
    }
}
