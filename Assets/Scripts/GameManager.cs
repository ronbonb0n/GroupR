using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GAME_STATE State;
    public static event Action<GAME_STATE> onGameStateChanged;
    public void Awake()
    {
        Instance = this;
    }
    public static void SwitchLevel(LEVELS newlevel)
    {
        
        SceneManager.LoadScene((int)newlevel);
    }

    public void UpdateGameState(GAME_STATE newState)
    {
        State = newState;
        switch (newState) //Checks for new state change and calls appropriate function
        {
            case GAME_STATE.WORLD_MAP:
                break;
            case GAME_STATE.LEVEL_1_START:
                break;
            case GAME_STATE.LEVEL_1_END:
                break;
            case GAME_STATE.LEVEL_2_START:
                break;
            case GAME_STATE.LEVEL_2_END:
                break;
            default:
                throw new System.Exception(newState.ToString()+" not found as a Game State");
        }
        onGameStateChanged?.Invoke(newState); // Invokes the Event whenever the Gamestate is changed
    
    }
}

public enum GAME_STATE
{
    WORLD_MAP,
    LEVEL_1_START,
    LEVEL_2_START,
    LEVEL_1_END,
    LEVEL_2_END
}
public enum LEVELS
{
    WORLD_MAP,
    LEVEL1,
    LEVEL2,
    MENU,
}
