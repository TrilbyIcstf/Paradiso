using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get { return GameManager._instance != null ? GameManager._instance : (GameManager)FindObjectOfType(typeof(GameManager)); }
        set { GameManager._instance = value; }
    }
    private static GameManager _instance;

    public Combat_UI_Manager CUI;
    public Combat_Stats_Manager CS;
    public Combat_Field_Manager CF;

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
}
