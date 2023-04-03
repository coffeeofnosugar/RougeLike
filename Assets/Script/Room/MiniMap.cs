using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private GameObject miniMap;
    public GameObject LeftUp;
    public GameObject RightDown;
    public GameObject monster;
    private Vector3 generatorPoint;
    public int monsterNumber = 2;
    private void OnEnable()
    {
        miniMap = transform.parent.GetChild(0).gameObject;
        miniMap.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            miniMap.SetActive(true);
            CreateMonster();
            CameraController.instance.ChangeTarget(transform);
        }
    }
    public void CreateMonster()
    {
        if (miniMap.activeSelf)
        {
            // 创建怪物
            for (int i = 0; i < monsterNumber; i++)
            {
                generatorPoint = new Vector3(Random.Range(LeftUp.transform.position.x, RightDown.transform.position.x), Random.Range(LeftUp.transform.position.y, RightDown.transform.position.y));
                Instantiate(monster, generatorPoint, Quaternion.identity);
            }
            // 创建门
            Room.CreateDoor();
        }
    }
}
