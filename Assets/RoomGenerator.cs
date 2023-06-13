using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public short roomID = 0;
    public bool isBorderRoom;
    [Space]
    public bool isTopOpen;
    public RoomGenerator topNeighborRoomGenerator;
    public bool isBottomOpen;
    public RoomGenerator bottomNeighborRoomGenerator;
    public bool isLeftOpen;
    public RoomGenerator leftNeighborRoomGenerator;
    public bool isRightOpen;
    public RoomGenerator rightNeighborRoomGenerator;

    private void Start()
    {
    }

    public enum roomType
    {
        Default,
        Start,
        End,
        EnemySpawn
    }
    public roomType thisRoomType = roomType.Default;

    public void CheckNeighbours()
    {
        if (topNeighborRoomGenerator != null && !topNeighborRoomGenerator.isBottomOpen)
        {
            isTopOpen = false;
        }

        if (leftNeighborRoomGenerator != null && !leftNeighborRoomGenerator.isRightOpen)
        {
            isLeftOpen = false;
        }
        if (bottomNeighborRoomGenerator != null && !bottomNeighborRoomGenerator.isTopOpen)
        {
            isBottomOpen = false;
        }

        if (rightNeighborRoomGenerator != null && !rightNeighborRoomGenerator.isLeftOpen)
        {
            isRightOpen = false;
        }
    }

    public void GenerateOuterBorders()
    {
        
        for (int x = 0; x < 20; x++)
        {            
            for (int y = 0; y < 20; y++)
            {
                if (y > 0 && y < 19 && x > 0 && x < 19)
                {
                    continue;
                }
                if (isTopOpen && y == 19 && x > 6 && x < 13)
                {
                    continue;
                }
                if (isBottomOpen && y == 0 && x > 6 && x < 13)
                {
                    continue;
                }
                if (isLeftOpen && x == 0 && y > 6 && y < 13)
                {
                    continue;
                }
                if (isRightOpen && x == 19 && y > 6 && y < 13)
                {
                    continue;
                }
                Vector2 place = new Vector2(transform.position.x + x - 9.5f, transform.position.y + y - 9.5f);
                Instantiate(wallPrefab, place, Quaternion.identity, transform);
            }
        }
    }
}
