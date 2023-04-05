using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public GameObject miniMap;
    private bool isOpenMiniMap = false;

    // Update is called once per frame
    void Update()
    {
        if (!isOpenMiniMap)
            //打开地图
            miniMap.gameObject.SetActive(Input.GetKey(KeyCode.M));
        
    }

    public void OpenMiniMap()
    {
        if (!isOpenMiniMap)
        {
            miniMap.gameObject.SetActive(true);
            Debug.Log(isOpenMiniMap);
            isOpenMiniMap = true;
        }
        else
        {
            miniMap.gameObject.SetActive(false);
            Debug.Log(isOpenMiniMap);
            isOpenMiniMap = false;
        }
    }
}
