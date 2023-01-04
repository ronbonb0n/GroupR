using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LevelCanvasControls : MonoBehaviour
{
    public GameObject LevelWonText;
    public GameObject LevelLostText;
    public bool levelOver;
    public TextMeshProUGUI[] InventoryTexts;
    public Canvas_Controls canvasInput;
    public bool isPaused;
    private InputAction Pause;
    private Script_Player_Control playerController;
    private GameObject PauseScreen;
    private GameObject ControlsScreen;

    private void Awake()
    {
        canvasInput = new Canvas_Controls();
    }
    void Start()
    {     
        LevelLostText.SetActive(false);
        LevelWonText.SetActive(false);
        levelOver = false;
        onItemUse();
        playerController = GameObject.Find("Player").GetComponent<Script_Player_Control>();
        PauseScreen = GameObject.Find("PauseScreen");
        PauseScreen.SetActive(false);
        ControlsScreen = GameObject.Find("ControlsScreen");
        ControlsScreen.SetActive(false);
    }
    private void OnEnable()
    {
        Pause = canvasInput.Pause.Pause;
        Pause.Enable();
        Pause.performed += onPause;
    }
    private void OnDisable()
    {
        Pause.Disable();
    }

    public void onLevelLost()
    {
        if (!levelOver)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            LevelLostText.SetActive(true);
            levelOver = true;
            playerController.PauseUnpauseActions(true);
            Time.timeScale = 0;
        }
    }

    public void onLevelWon()
    {
        if (!levelOver)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            LevelWonText.SetActive(true);
            levelOver = true;
            playerController.PauseUnpauseActions(true);
        }
    }

    public void onReturnToWorldMap()
    {
        GameManager.SwitchLevel(LEVELS.WORLD_MAP);
    }
    public void onRetry()
    {
        GameManager.SwitchLevel(GameManager.Level);
    }
    public void onItemUse()
    {
        for(var i=0; i<InventoryManager.Inventory.Length; i++)
        {
            InventoryTexts[i].text = InventoryManager.Inventory[i].ToString();
        }
    }
    public void onPause(InputAction.CallbackContext context)
    {
        if (!LevelLostText.activeInHierarchy && !LevelWonText.activeInHierarchy)
        {
            isPaused = !isPaused;
            if (isPaused)
            {

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ControlsScreen.SetActive(isPaused);
            }
            PauseScreen.SetActive(isPaused);
            playerController.PauseUnpauseActions(isPaused);
            foreach(GameObject Audio in GameObject.FindGameObjectsWithTag("Audio"))
            {
                //Debug.Log(Audio);
                Audio.GetComponent<AudioSource>().enabled = !isPaused;
            }
        }
    }
    public void onContinue()
    {
        LevelWonText.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void onControlsScreen()
    {
        PauseScreen.SetActive(!PauseScreen.activeSelf);
        ControlsScreen.SetActive(!ControlsScreen.activeSelf);
    }

    
    private void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else {
            if (!levelOver)
            {
                Time.timeScale = 1;
            }
        }
    }
    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
