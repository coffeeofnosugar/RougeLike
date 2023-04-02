using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private GameObject miniMap;
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
            CameraController.instance.ChangeTarget(transform);
        }
    }
}
