using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    public float startToDestory = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(transform.gameObject, startToDestory);
    }
}
