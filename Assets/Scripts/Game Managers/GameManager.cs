using UnityEngine;

/// <summary>
/// ALL HAIL THE MIGHTY GAMEMANAGER, ORGANIZER OF THE MANAGERS
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance
    {
        get { return GameManager._instance != null ? GameManager._instance : (GameManager)FindObjectOfType(typeof(GameManager)); }
        set { GameManager._instance = value; }
    }
    private static GameManager _instance;

    public Static_Object_Manager SOM;
    public Transition_Manager TR;

    public Player_Manager PM;
    public Item_Behavior_Manager IBM;

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

    public Screen_Fade SF;

    public String_List STR;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.instance = this;
        this.PM.TestRandomDeck(30);
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

    public void InitCombat()
    {
        ToggleCombatUpdates(true);
        this.CPS.InitializeHealth(this.PM.GetMaxHealth(), this.PM.GetCurrentHealth());
        this.CPS.InitializeEnergy(this.PM.GetMaxEnergy());
        this.CPD.SetDeck(this.PM.GetDeck());
        this.CPD.ShuffleDeck();

        this.CES.AddFreeCards(3);
        this.CPS.AddFreeCards(3);
    }

    public void EndCombat()
    {
        this.PM.SetCurrentHealth((int)this.CPS.GetCurrentHealth());
        this.PM.ActivatePassiveItems(EffectTiming.CombatEnd, null);
        ToggleCombatUpdates(false);
        this.TR.UnloadScene("TestCombat", () => {
            this.CPH.ResetHand();
            this.CEH.ResetHand();
            this.ER.SetRoomActive(true);
        });
    }
}