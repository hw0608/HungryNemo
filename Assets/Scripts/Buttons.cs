using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void LoadGameScene()
    {
        AudioManager.instance.SfxPlay(AudioManager.Sfx.Button);
        SceneManager.LoadScene("GameScene");
    }
}
