using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GAME_STATE State;
    public static LEVELS Level;
    public static event Action<GAME_STATE> onGameStateChanged;
    public void Start()
    {
        if (Level == LEVELS.WORLD_MAP)
        {
            HighlightNextLevel();
        }
    }
    public static void SwitchLevel(LEVELS newlevel)
    {
        SceneManager.LoadScene((int)newlevel);
    }

    
    public static void UpdateGameState(GAME_STATE newState)
    {
        switch (newState) //Checks for new state change and calls appropriate function
        {
            case GAME_STATE.WORLD_MAP:
                GameManager.Level = LEVELS.WORLD_MAP;
                SwitchLevel(LEVELS.WORLD_MAP);
                break;
            case GAME_STATE.LEVEL_1_START:
                GameManager.State = newState;
                GameManager.Level = LEVELS.LEVEL1; 
                SwitchLevel(LEVELS.LEVEL1);
                break;
            case GAME_STATE.LEVEL_1_END_LOST:
                GameManager.State = newState;
                SwitchLevel(LEVELS.LEVEL1);
                break;
            case GAME_STATE.LEVEL_1_END_WON:
                GameManager.State = newState;
                GameManager.Level = LEVELS.WORLD_MAP; 
                SwitchLevel(LEVELS.WORLD_MAP);
                break;
            case GAME_STATE.LEVEL_2_START:
                GameManager.State = newState;
                GameManager.Level = LEVELS.LEVEL2; 
                SwitchLevel(LEVELS.LEVEL2);
                break;
            case GAME_STATE.LEVEL_2_END_LOST:
                GameManager.State = newState;
                SwitchLevel(LEVELS.LEVEL2);
                break;
            case GAME_STATE.LEVEL_2_END_WON:
                GameManager.State = newState;
                GameManager.Level = LEVELS.WORLD_MAP; 
                SwitchLevel(LEVELS.WORLD_MAP);
                break;
            case GAME_STATE.VENDOR:
                GameManager.Level = LEVELS.VENDOR;
                SwitchLevel(LEVELS.VENDOR);
                break;
            default:
                throw new System.Exception(newState.ToString()+" not found as a Game State");
        }
        onGameStateChanged?.Invoke(newState); // Invokes the Event whenever the Gamestate is changed
    }
    protected void HighlightNextLevel()
    {
        var all = FindObjectsOfType<GameObject>(true);
        GameObject[] towers = all.Where(obj => obj.name.StartsWith("Tower")).ToList().ToArray();
        foreach(GameObject tower in towers)
        {
            if (tower.GetComponent<ChangeLevel>().newState == State + 1)
            {
                Light towerLight = tower.GetComponent<Light>();
                towerLight.color = Color.cyan;
                towerLight.intensity = 50;
            }
        }
    }
}

    
public enum GAME_STATE
{
    WORLD_MAP,
    LEVEL_1_START,
    LEVEL_1_END_LOST,
    LEVEL_1_END_WON,
    LEVEL_2_START,
    LEVEL_2_END_LOST,
    LEVEL_2_END_WON,
    LEVEL_3_START,
    VENDOR
}
public enum LEVELS
{
    WORLD_MAP,
    LEVEL1,
    LEVEL2,
    VENDOR,
}
