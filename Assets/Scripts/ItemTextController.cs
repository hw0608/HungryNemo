using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTextController : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
    }

    void DestoryEvent()
    {
        Destroy(gameObject);
    }
}
