using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject doorLeft, doorRight, doorUp, doorDown;

    public bool roomLeft = false, roomRight = false, roomUp = false, roomDown = false;

    public int doorNumber;
    private Transform doors;
    public static bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        doorUp.SetActive(roomUp);
        doorDown.SetActive(roomDown);
    }

    private void Update()
    {
        if (done)
        {
            for (int i = 0; i < 4; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateRoom()
    {
        if (roomUp) doorNumber++;
        if (roomDown) doorNumber++;
        if (roomLeft) doorNumber++;
        if (roomRight) doorNumber++;
    }
    public static void JudgmenDone()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            done = true;
            bool enemyAlive = enemy.GetComponent<Ghoul>().MA.isAlive;
            done = done && !enemyAlive;
        }
    }
}
