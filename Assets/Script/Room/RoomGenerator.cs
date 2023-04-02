using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    private enum Direction {up, down, left, right};
    private Direction direction;
    private GameObject endRoom;

    [Header("房间信息")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] int rommNumber;
    [SerializeField] Color starColor,endColor;

    [Header("位置控制")]
    [SerializeField] private Transform generatorPoint;
    [SerializeField] private static float xOffsetRoom = 18;
    [SerializeField] private static float yOffsetRoom = 10;

    private List<Room> rooms = new List<Room>();
    private static List<Vector3> positionRoom = new List<Vector3>();
    
    private List<Vector3> endInterlinkageRoom = new List<Vector3>();
    private Room lastRoom;
    [Header("墙壁")]
    [SerializeField] WallType WallType;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rommNumber - 1; i++)
        {
            // 添加到列表中，方便后续管理
            // 创建roomPrefab对象，位置：generatorPoint,无旋转
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>());
            // 保存创建房间的位置
            positionRoom.Add(generatorPoint.position);
            // 改变generatorPoint位置
            ChangePointPos();
        }
        // 设置初始房间的颜色
        rooms[0].GetComponent<SpriteRenderer>().color = starColor;

        endRoom = rooms[0].gameObject;
        foreach (var room in rooms)
        {
            SetupDoor(room);
            // 获取距离初始房间最远的房间
            if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)
            {
                endRoom = room.gameObject;
            }
        }
        endInterlinkageRoom.Add(endRoom.transform.position + new Vector3(0, yOffsetRoom, 0));
        endInterlinkageRoom.Add(endRoom.transform.position + new Vector3(0, -yOffsetRoom, 0));
        endInterlinkageRoom.Add(endRoom.transform.position + new Vector3(xOffsetRoom, 0, 0));
        endInterlinkageRoom.Add(endRoom.transform.position + new Vector3(-xOffsetRoom, 0, 0));
        foreach (var roomPosition in endInterlinkageRoom)
        {
            if (!IsAroundHaveRoom(roomPosition))
            {
                lastRoom = Instantiate(roomPrefab, roomPosition, Quaternion.identity).GetComponent<Room>();
                lastRoom.GetComponent<SpriteRenderer>().color = endColor;
                // 补全数据
                rooms.Add(lastRoom);
                positionRoom.Add(endRoom.transform.position);
                positionRoom.Add(lastRoom.transform.position);
                SetupDoor(lastRoom);
                SetupDoor(endRoom.GetComponent<Room>());
                break;
            }
        }
        foreach (var room in rooms)
        {
            SetUpWall(room);
        }
    }
    // 向上、下、左、右移动标记点
    public void ChangePointPos()
    {
        do
        {
            direction = (Direction)Random.Range(0,4);

            switch(direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffsetRoom, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -yOffsetRoom, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(xOffsetRoom, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(-xOffsetRoom, 0, 0);
                    break;
            }
        }while(IsHaveRoom(generatorPoint));
    }
    // 判断新坐标上是否已经创建过房间
    public bool IsHaveRoom(Transform roomPoint)
    {
        foreach (var room in rooms)
        {
            if(positionRoom.Contains(roomPoint.position))
            {
                return true;
            }
        }
        return false;
    }
    // 判断四个方向上是否其他房间，参数是Room类
    public static void SetupDoor(Room newRoom)
    {
        // 通过将roomUp的参数设置为true来创建门
        if (positionRoom.Contains(newRoom.transform.position + new Vector3(0, yOffsetRoom, 0))) { newRoom.roomUp = true; Debug.Log("Up"); }
        if (positionRoom.Contains(newRoom.transform.position + new Vector3(0, -yOffsetRoom, 0))) { newRoom.roomDown = true; Debug.Log("Down"); }
        if (positionRoom.Contains(newRoom.transform.position + new Vector3(xOffsetRoom, 0, 0))) { newRoom.roomRight = true; Debug.Log("Right"); }
        if (positionRoom.Contains(newRoom.transform.position + new Vector3(-xOffsetRoom, 0, 0))) { newRoom.roomLeft = true; Debug.Log("Left"); }
    }
    public void SetUpWall(Room newRoom)
    {
        // 计算门的数量
        newRoom.UpdateRoom();
        // 依据门的数量来创建墙边框
        switch(newRoom.doorNumber)
        {
            case 1:
                if(newRoom.roomUp) Instantiate(WallType.singleUp, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomDown) Instantiate(WallType.singleDown, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomLeft) Instantiate(WallType.singleLeft, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomRight) Instantiate(WallType.singleRight, newRoom.transform.position, Quaternion.identity);
                break;
            case 2:
                if(newRoom.roomUp && newRoom.roomDown) Instantiate(WallType.doubleUD, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomUp && newRoom.roomLeft) Instantiate(WallType.doubleUL, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomUp && newRoom.roomRight) Instantiate(WallType.doubleUR, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomDown && newRoom.roomLeft) Instantiate(WallType.doubleDL, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomDown && newRoom.roomRight) Instantiate(WallType.doubleDR, newRoom.transform.position, Quaternion.identity);
                if(newRoom.roomLeft && newRoom.roomRight) Instantiate(WallType.doubleLR, newRoom.transform.position, Quaternion.identity);
                break;
            case 3:
                if(!newRoom.roomUp) Instantiate(WallType.tripleDLR, newRoom.transform.position, Quaternion.identity);
                if(!newRoom.roomDown) Instantiate(WallType.tripleULR, newRoom.transform.position, Quaternion.identity);
                if(!newRoom.roomLeft) Instantiate(WallType.tripleUDR, newRoom.transform.position, Quaternion.identity);
                if(!newRoom.roomRight) Instantiate(WallType.tripleUDL, newRoom.transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(WallType.fourDoors, newRoom.transform.position, Quaternion.identity);
                break;
        }
    }
    // 判断四个方向上是否有其他房间，参数是三维位置坐标
    public bool IsAroundHaveRoom(Vector3 roomPosition)
    {
        positionRoom.Remove(endRoom.transform.position);
        if (positionRoom.Contains(roomPosition + new Vector3(0, yOffsetRoom, 0))) { return true; }
        if (positionRoom.Contains(roomPosition + new Vector3(0, -yOffsetRoom, 0))) { return true; }
        if (positionRoom.Contains(roomPosition + new Vector3(xOffsetRoom, 0, 0))) { return true; }
        if (positionRoom.Contains(roomPosition + new Vector3(-xOffsetRoom, 0, 0))) { return true; }
        return false;
    }
}

[System.Serializable]
public class WallType
{
    public GameObject singleLeft, singleRight, singleUp, singleDown,
                      doubleUD, doubleUL, doubleUR, doubleDL, doubleDR, doubleLR,
                      tripleUDL, tripleUDR, tripleULR, tripleDLR,
                      fourDoors;

}