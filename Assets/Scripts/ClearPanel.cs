using UnityEngine;

public class ClearPanel : MonoBehaviour
{
    public GameObject title;
    public GameObject highscores;

    public void HighScoresPressed()
    {
        title.SetActive(false);
        highscores.SetActive(true);
    }

    public void BackPressed()
    {
        title.SetActive(true);
        highscores.SetActive(false);
    }
}
