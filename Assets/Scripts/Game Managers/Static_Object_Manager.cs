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
        get { return Static_Object_Manager._instance != null ? Static_Object_Manager._instance : (Static_Object_Manager)FindFirstObjectByType(typeof(Static_Object_Manager)); }
        set { Static_Object_Manager._instance = value; }
    }
    private static Static_Object_Manager _instance;

    // The big ol' list of items in the game
    [Serializable]
    public class ItemDictionary : SerializableDictionary<Item, Item_Base> { }
    [SerializeField]
    private ItemDictionary items;
    public Item_Base GetItem(Item item) { return this.items[item]; }

    // List of enemies in the game
    [Serializable]
    public class EnemyDictionary : SerializableDictionary<Enemy, Enemy_Stats> { }
    public EnemyDictionary enemies;
    public Enemy_Stats GetEnemy(Enemy enemy) { return Instantiate(this.enemies[enemy]); }

    // The quill card stats used for the quill effect
    [SerializeField]
    private Card_Base quillCard;

    public Card_Base GetQuillCard() { return Instantiate(this.quillCard); }

    // Wall tile sprites used for exploration. May vary by floor
    [Serializable]
    public class WallTiles : SerializableDictionary<Floor, TileBase> { }
    [SerializeField]
    private WallTiles wallTiles;
    public TileBase GetWallTile(Floor floor) { return this.wallTiles[floor]; }

    // Floor tile sprites used for exploration. May vary by floor
    [Serializable]
    public class FloorTiles : SerializableDictionary<Floor, TileBase> { }
    [SerializeField]
    private FloorTiles floorTiles;
    public TileBase GetFloorTile(Floor floor) { return this.floorTiles[floor]; }

    // Lock tile sprites used for exploration. May vary by floor
    [Serializable]
    public class LockTiles : SerializableDictionary<Floor, TileBase> { }
    [SerializeField]
    private LockTiles lockTiles;
    public TileBase GetLockTile(Floor floor) { return this.lockTiles[floor]; }

    // Elemental icons
    [Serializable]
    public class ElementIcons : SerializableDictionary<CardElement, Sprite> { }
    [SerializeField]
    private ElementIcons elementIcons;
    public Sprite GetElementIcon(CardElement element) { return this.elementIcons[element]; }
}
