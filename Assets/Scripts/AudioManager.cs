using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Sfx
    {
        Bubble,
        Pop,
        Button,
        LevelUp,
        Hit,
        Star,
        SpeedUp,
        Heal,
        Eat,
        GameOver,
        GameClear
    }

    public static AudioManager instance;
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    int sfxCursor;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        bgmPlayer.Play();
    }


    public void SfxPlay(Sfx type)
    {
        switch (type)
        {
            case Sfx.Bubble:
                sfxPlayer[sfxCursor].clip = sfxClip[0];
                break;
            case Sfx.Pop:
                sfxPlayer[sfxCursor].clip = sfxClip[1];
                break;
            case Sfx.Button:
                sfxPlayer[sfxCursor].clip = sfxClip[2];
                break;
            case Sfx.LevelUp:
                sfxPlayer[sfxCursor].clip = sfxClip[3];
                break;
            case Sfx.Hit:
                sfxPlayer[sfxCursor].clip = sfxClip[4];
                break;
            case Sfx.Star:
                sfxPlayer[sfxCursor].clip = sfxClip[5];
                break;
            case Sfx.SpeedUp:
                sfxPlayer[sfxCursor].clip = sfxClip[6];
                break;
            case Sfx.Heal:
                sfxPlayer[sfxCursor].clip = sfxClip[7];
                break;
            case Sfx.Eat:
                sfxPlayer[sfxCursor].clip = sfxClip[8];
                break;
            case Sfx.GameOver:
                sfxPlayer[sfxCursor].clip = sfxClip[9];
                break;
            case Sfx.GameClear:
                sfxPlayer[sfxCursor].clip = sfxClip[10];
                break;
        }
        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }
}
