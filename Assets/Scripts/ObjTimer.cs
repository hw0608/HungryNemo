using UnityEngine;

public class ObjTimer : MonoBehaviour
{
    public float time;
    public float timer = 0;

    private void OnEnable()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > time)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }
}
