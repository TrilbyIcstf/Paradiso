using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates the minimap object while exploring
/// </summary>
public class Minimap_Maker : ManagerBehavior
{
    private float roomDistance = 2.5f;

    private RoomExpLevels[,] floorLayout;

    [SerializeField]
    GameObject minimapCamera;

    [SerializeField]
    private Sprite enteredRoom;
    [SerializeField]
    private Sprite hiddenRoom;
    [SerializeField]
    private Sprite roomConnection;
    [SerializeField]
    private GameObject dummyObject;

    private void Start()
    {
        int width = GM.EL.GetFloorWidth();
        int height = GM.EL.GetFloorHeight();

        this.floorLayout = new RoomExpLevels[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector2Int pos = new Vector2Int(j, i);

                Room_Object room = GM.EL.GetRoom(pos);

                if (room == null) { continue; }

                if (room.GetEntered())
                {
                    this.floorLayout[pos.x, pos.y] = RoomExpLevels.Entered;

                    MakeRoom(pos, this.enteredRoom);
                    foreach (Direction dir in GM.EL.GetRoom(pos).GetConnections().Keys)
                    {
                        Vector2Int connPos = pos + dir.NumericalDirection();
                        if (this.floorLayout[connPos.x, connPos.y] != RoomExpLevels.Entered) {
                            AddConnection(pos, dir);
                        }
                    }

                } else if (CheckSeen(room))
                {
                    this.floorLayout[pos.x, pos.y] = RoomExpLevels.Seen;

                    MakeRoom(pos, this.hiddenRoom);
                }
            }
        }
    }

    private void MakeRoom(Vector2Int pos, Sprite sprite)
    {
        GameObject tempRoom = Instantiate(this.dummyObject, this.transform);
        Vector3 mapPos = tempRoom.transform.position;
        mapPos.x += (pos.x * this.roomDistance);
        mapPos.y += (pos.y * this.roomDistance);
        tempRoom.transform.position = mapPos;

        tempRoom.GetComponent<Image>().sprite = sprite;

        if (pos == GM.EL.GetPos())
        {
            Vector3 cameraPos = this.minimapCamera.transform.position;
            cameraPos.x = tempRoom.transform.position.x;
            cameraPos.y = tempRoom.transform.position.y;
            this.minimapCamera.transform.position = cameraPos;
        }
    }

    private void AddConnection(Vector2Int pos, Direction direction)
    {
        GameObject tempConnection = Instantiate(this.dummyObject, this.transform);
        Vector3 mapPos = tempConnection.transform.position;
        Vector2 posShift = new Vector2(0.5f, 0.5f) * direction.NumericalDirection() * this.roomDistance;
        mapPos.x += (pos.x * this.roomDistance) + posShift.x;
        mapPos.y += (pos.y * this.roomDistance) + posShift.y;
        tempConnection.transform.position = mapPos;

        tempConnection.GetComponent<Image>().sprite = this.roomConnection;

        if (direction == Direction.Down || direction == Direction.Up)
        {
            tempConnection.transform.Rotate(0, 0, 90);
        }
    }

    private bool CheckSeen(Room_Object room)
    {
        foreach (Vector2Int pos in room.GetConnections().Values)
        {
            Room_Object connRoom = GM.EL.GetRoom(pos);
            if (connRoom.GetEntered()) {
                return true;
            }
        }

        return false;
    }
}

enum RoomExpLevels
{
    None,
    Seen,
    Entered
}