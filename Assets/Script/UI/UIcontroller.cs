using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public GameObject miniMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //打开地图
        miniMap.gameObject.SetActive(Input.GetKey(KeyCode.M));
        
    }
}
