using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject roomGeneratorPrefab;
    public GameObject levelEndPrefab;
    public GameObject enemyPrefab;
    public Vector2 levelSize = new Vector2(8,8);
    public List<GameObject> rooms = new List<GameObject>();
    Vector2 pathDirection;

    private void Start()
    {
        short roomIDcounter = 0;
        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                GameObject room = Instantiate(roomGeneratorPrefab, new Vector2(x * 20, y * 20), Quaternion.identity, transform);
                room.GetComponent<RoomGenerator>().roomID = roomIDcounter;
                roomIDcounter++;
                rooms.Add(room);
            }            
        }

        foreach (GameObject room in rooms)
        {
            RoomGenerator rg = room.GetComponent<RoomGenerator>();
            float x = room.transform.position.x / 20;
            float y = room.transform.position.y / 20;

            rg.isBottomOpen = y % levelSize.y != 0;
            rg.isTopOpen = y % (levelSize.y - 1) != 0 || y == 0;
            rg.isLeftOpen = x % levelSize.x != 0;
            rg.isRightOpen = x % (levelSize.x - 1) != 0 || x == 0;

            if (y != levelSize.y && y % (levelSize.y - 1) != 0 || y == 0)
            {
                rg.topNeighborRoomGenerator = rooms.ElementAt((int)y + 1).GetComponent<RoomGenerator>();
            }
            if (y != 0 && y % levelSize.x != 0)
            {
                rg.bottomNeighborRoomGenerator = rooms.ElementAt((int)y - 1).GetComponent<RoomGenerator>();
            }
            if (x > 0)
            {
                rg.leftNeighborRoomGenerator = rooms.ElementAt(rg.roomID - (int)levelSize.x).GetComponent<RoomGenerator>();
            }
            if (x < levelSize.y - 1)
            {
                rg.rightNeighborRoomGenerator = rooms.ElementAt(rg.roomID + (int)levelSize.x).GetComponent<RoomGenerator>();
            }
            rg.CheckNeighbours();
            bool condition = !(!(y % levelSize.x != 0) || !(y % (levelSize.x - 1) != 0 || y == 0) || !(x % levelSize.y != 0) || !(x % (levelSize.y - 1) != 0 || x == 0));
            Debug.Log(condition);
            if (condition)
            {
                rg.isBottomOpen = true;
                rg.isTopOpen = true;
                rg.isLeftOpen = true;
                rg.isRightOpen = true;
            }
            rg.GenerateOuterBorders();

        }
        Instantiate(levelEndPrefab, rooms.ElementAt(Random.Range(1, (int)(levelSize.y + levelSize.x - 1))).transform.position, Quaternion.identity,transform);
        for (int i = 0; i < PlayerPrefs.GetInt("level"); i++)
        Instantiate(enemyPrefab, rooms.ElementAt(Random.Range(1, (int)(levelSize.y + levelSize.x - 1))).transform.position, Quaternion.identity, transform);
    }

    bool RandomFlag()
    {
        bool[] boolLMAO = new bool[] { true, false };
        return boolLMAO[Random.Range(0,1)];
    }
}
