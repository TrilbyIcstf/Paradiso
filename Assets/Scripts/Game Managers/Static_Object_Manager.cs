using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Holds static references to objects to be accessible at all times
/// </summary>
public class Static_Object_Manager : MonoBehaviour
{
    public static Static_Object_Manager instance
    {
        get { return Static_Object_Manager._instance != null ? Static_Object_Manager._instance : (Static_Object_Manager)FindObjectOfType(typeof(Static_Object_Manager)); }
        set { Static_Object_Manager._instance = value; }
    }
    private static Static_Object_Manager _instance;

    // The quill card stats used for the quill effect
    public Card_Base QuillCard;

    // Wall tile sprites used for exploration. May vary by floor
    [Serializable]
    public class WallTiles : SerializableDictionary<Floors, TileBase> { }
    [SerializeField]
    private WallTiles wallTiles;
    public TileBase GetWallTile(Floors floor) { return this.wallTiles[floor]; }

    // Floor tile sprites used for exploration. May vary by floor
    [Serializable]
    public class FloorTiles : SerializableDictionary<Floors, TileBase> { }
    [SerializeField]
    private FloorTiles floorTiles;
    public TileBase GetFloorTile(Floors floor) { return this.floorTiles[floor]; }

    // Lock tile sprites used for exploration. May vary by floor
    [Serializable]
    public class LockTiles : SerializableDictionary<Floors, TileBase> { }
    [SerializeField]
    private LockTiles lockTiles;
    public TileBase GetLockTile(Floors floor) { return this.lockTiles[floor]; }
}
