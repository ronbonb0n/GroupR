using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GAME_STATE State;
    public static LEVELS Level;
    public void Start()
    {
    }
    public static void SwitchLevel(LEVELS newlevel)
    {
        SceneManager.LoadScene((int)newlevel);
        Level = newlevel;
    }

    
    public static void UpdateGameState(GAME_STATE newState)
    {
        switch (newState) //Checks for new state change and calls appropriate function
        {
            case GAME_STATE.WORLD_MAP:
                break;
            case GAME_STATE.DUNGEON_1_COMPLETE:
                break;
            case GAME_STATE.DUNGEON_2_COMPLETE:
                break;
            case GAME_STATE.DUNGEON_3_COMPLETE:
                break;
            case GAME_STATE.DUNGEON_4_COMPLETE:
                break;

            default:
                throw new System.Exception(newState.ToString()+" not found as a Game State");
        } // Invokes the Event whenever the Gamestate is changed
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
