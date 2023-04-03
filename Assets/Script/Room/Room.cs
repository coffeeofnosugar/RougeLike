using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public GameObject doorLeft, doorRight, doorUp, doorDown;

    public bool roomLeft = false, roomRight = false, roomUp = false, roomDown = false;

    public int doorNumber;
    private Transform doors;
    public static bool done = false;

    public static Room roomInstance;

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

    }
    public static void SetDoor(Room room)
    {
        room.doorUp.SetActive(room.roomUp);
        room.doorDown.SetActive(room.roomDown);
        room.doorLeft.SetActive(room.roomLeft);
        room.doorRight.SetActive(room.roomRight);
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
        // 获取
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            done = true;
            bool enemyAlive = enemy.GetComponent<Ghoul>().MA.isAlive;
            done = done && !enemyAlive;
        }
        if (done)
        {
            roomInstance.roomUp = false;
            roomInstance.roomDown = false;
            roomInstance.roomLeft = false;
            roomInstance.roomRight = false;
            SetDoor(roomInstance);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomInstance = this;
        }
    }
    public static void CreateDoor()
    {
        done = false;
        RoomGenerator.SetupDoor(roomInstance);
        SetDoor(roomInstance);
    }
}
