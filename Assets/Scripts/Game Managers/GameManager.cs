using UnityEngine;

/// <summary>
/// ALL HAIL THE MIGHTY GAMEMANAGER, ORGANIZER OF THE MANAGERS
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance
    {
        get { return GameManager._instance != null ? GameManager._instance : (GameManager)FindFirstObjectByType(typeof(GameManager)); }
        set { GameManager._instance = value; }
    }
    private static GameManager _instance;

    public Static_Object_Manager SO;
    public Transition_Manager TR;

    public Player_Manager PL;
    public Item_Behavior_Manager IB;

    public Combat_UI_Manager CUI;
    public Combat_Field_Manager CF;
    public Combat_Player_Stats_Manager CPS;
    public Combat_Enemy_Stats_Manager CES;
    public Combat_Player_Hand_Manager CPH;
    public Combat_Enemy_Hand_Manager CEH;
    public Combat_Player_Deck_Manager CPD;
    public Combat_Player_Item_Manager CPI;
    public Combat_Card_Effect_Manager CCE;

    public Exploration_Layout_Manager EL;
    public Exploration_Room_Manager ER;
    public Exploration_Player_Manager EP;
    public Exploration_Item_Manager EI;
    public Exploration_Upgrade_Manager EU;

    public Screen_Fade SF;

    public String_List STR;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.instance = this;
        this.PL.SetBasicStartingDeck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ToggleCombatUpdates(bool val)
    {
        this.CPS.enabled = val;
        this.CPH.enabled = val;
        this.CES.enabled = val;
        this.CEH.enabled = val;
    }
}