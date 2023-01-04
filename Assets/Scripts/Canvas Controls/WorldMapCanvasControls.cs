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

    }
    public void offNarrationScreen()
    {
        GameManager.State = GAME_STATE.WORLD_MAP;
        NarrationScreen.SetActive(false);
        narrativeScript.offNarrative();
    }
    public void continueNarrative()
    {
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
}
