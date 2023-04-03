using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public static List<GameObject> healthIcon = new List<GameObject>();
    private static int health;
    // Start is called before the first frame update
    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health;
        for (int i = 0; i < health; i++)
        {
            healthIcon.Add(transform.GetChild(i).gameObject);
        }
    }
    public static void GetHit()
    {
        health--;
        healthIcon[health].SetActive(false);
    }
}
