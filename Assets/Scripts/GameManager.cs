using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GAME_STATE State = GAME_STATE.NARRATIVE;
    public static LEVELS Level = LEVELS.WORLD_MAP;
    public static Vector3 playerInWorldMap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else { Destroy(this.gameObject); }
        if (PlayerPrefs.HasKey("State"))
        {
            State = (GAME_STATE)PlayerPrefs.GetInt("State");
        }


    }
    private void Start()
    {
        if (State == GAME_STATE.NARRATIVE && Level == LEVELS.WORLD_MAP)
        {
            NarrativeScript narrativeScript = GameObject.Find("Player").GetComponent<NarrativeScript>();
            narrativeScript.onNarrativeStart();
        }
    }

    public static void SwitchLevel(LEVELS newlevel)
    {
        SceneManager.LoadScene((int)newlevel);
        Level = newlevel;
    }

    
    public static void UpdateGameState(GAME_STATE newState)
    {
        if (newState > State) State = newState;
        PlayerPrefs.SetInt("State", (int)State);
        
    }
    public static bool CheckNextLevel(LEVELS level)
    {
        if (level == LEVELS.VENDOR)
        {
            return true;
        }
        if (((int)State >= (int)level - 1))
        {
            return true;
        }
        return false;
    }
    public void OnDestroy()
    {
        PlayerPrefs.SetInt("State", (int)State);
        PlayerPrefs.Save();
    }


}

    
public enum GAME_STATE
{
    WORLD_MAP,
    DUNGEON_1_COMPLETE,
    DUNGEON_2_COMPLETE,
    DUNGEON_3_COMPLETE,
    DUNGEON_4_COMPLETE,
    NARRATIVE
}
public enum LEVELS
{
    WORLD_MAP,
    DUNGEON_1,
    DUNGEON_2,
    DUNGEON_3,
    DUNGEON_4,
    VENDOR
}
