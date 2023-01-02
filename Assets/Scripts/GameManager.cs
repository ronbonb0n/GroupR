using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GAME_STATE State;
    public static LEVELS Level;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }

    }
    public void Start()
    {
        State = GAME_STATE.WORLD_MAP;
        Level = LEVELS.WORLD_MAP;
    }
    public static void SwitchLevel(LEVELS newlevel)
    {
        SceneManager.LoadScene((int)newlevel);
        Level = newlevel;
    }

    
    public static void UpdateGameState(GAME_STATE newState)
    {
        State = newState;
    }
}

    
public enum GAME_STATE
{
    WORLD_MAP,
    DUNGEON_1_COMPLETE,
    DUNGEON_2_COMPLETE,
    DUNGEON_3_COMPLETE,
    DUNGEON_4_COMPLETE
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
