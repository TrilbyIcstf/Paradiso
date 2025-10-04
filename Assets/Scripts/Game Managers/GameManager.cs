using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get { return GameManager._instance != null ? GameManager._instance : (GameManager)FindObjectOfType(typeof(GameManager)); }
        set { GameManager._instance = value; }
    }
    private static GameManager _instance;

    public Player_Manager PM;

    public Combat_UI_Manager CUI;
    public Combat_Field_Manager CF;
    public Combat_Stats_Manager CS;
    public Combat_Enemy_Stats_Manager CES;
    public Combat_Player_Hand_Manager CPH;
    public Combat_Enemy_Hand_Manager CEH;
    public Combat_Player_Deck_Manager CPD;
    public Combat_Player_Item_Manager CPI;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void InitCombat()
    {
        this.CS.InitializeHealth(this.PM.GetMaxHealth(), this.PM.GetCurrentHealth());
        this.CPI.SetPlayersItems();

        this.CPD.TestRandomDeck(30);

        this.CES.AddFreeCards(3);
        this.CS.AddFreeCards(3);
    }
}
