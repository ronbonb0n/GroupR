using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapCanvasControls : MonoBehaviour
{
    public GameObject ControlsBtn;
    public GameObject ClearScreen;
    public GameObject NarrationScreen;
    public TextMeshProUGUI narrativeText;
    public NarrativeScript narrativeScript;
    public bool fadeToWhite;
    private int dialogueNumber = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        ControlsBtn.SetActive(true);
        ClearScreen.SetActive(false);
        NarrationScreen.SetActive(false);
    }

    public void OnToggle()
    {
        ControlsBtn.SetActive(!ControlsBtn.activeInHierarchy);
        ClearScreen.SetActive(!ClearScreen.activeInHierarchy);
    }
    public void onNarrationScreen()
    {
        NarrationScreen.SetActive(true);
        dialogueNumber = 0;
        narrativeText.text = narrativeScript.getNextText(dialogueNumber);
        narrativeScript.onNarrative();
        if (GameManager.State == GAME_STATE.DUNGEON_4_COMPLETE)
        {
            NarrationScreen.transform.Find("SkipBtn").gameObject.SetActive(false);
        }
    }
    public void offNarrationScreen()
    {
        if (GameManager.State != GAME_STATE.DUNGEON_4_COMPLETE) { 
            GameManager.State = GAME_STATE.WORLD_MAP; 
        }
        NarrationScreen.SetActive(false);
        narrativeScript.offNarrative();
        GameManager.SwitchLevel(LEVELS.WORLD_MAP);

    }
    public void continueNarrative()
    {
        if (GameManager.State == GAME_STATE.DUNGEON_4_COMPLETE)
        {
            fadeToWhite = true;
            NarrationScreen.transform.Find("ContinueBtn").gameObject.SetActive(false);

        }
        if (narrativeScript.getNextText(dialogueNumber + 1) == "-1")
        {
            offNarrationScreen();
        }
        else
        {
            narrativeText.text = narrativeScript.getNextText(dialogueNumber + 1);
            dialogueNumber++;
        }
    }
    private void Update()
    {
        if (fadeToWhite == true)
        {
            GameObject whiteScreen = GameObject.Find("Canvas").transform.Find("WhiteScreen").gameObject;
            if (!whiteScreen.activeInHierarchy) whiteScreen.SetActive(true);
            Color whiteScreenColor = whiteScreen.GetComponent<Image>().color;
            whiteScreen.GetComponent<Image>().color = new Color(whiteScreenColor.r, whiteScreenColor.g, whiteScreenColor.b, Mathf.Clamp(whiteScreenColor.a + 0.1f * Time.deltaTime,0,1));
        }
    }
}
