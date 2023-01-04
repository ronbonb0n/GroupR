using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeScript : MonoBehaviour
{
    public Camera WorldMapCamera;
    public Camera NarrativeCamera;
    private string[] dialogues = {"Well, I drew the short straw, guess it was just my time to be kicked from the party and figure a way out of this place, clear the dungeons and get the keys – a trap to be sure, but I don’t have much choice – Never let it said that Ferynier was a quitter",
            "This used to be the hub – a player exclusive area, somewhere you could meet and chat with your friends and jump into a dungeon… it’s a lot smaller now. Only a few dungeons left, and they’re a lot more dangerous than they used to be",
            "Either way, I should be safe here, although safe is relative I suppose. I believe there was some kind of shop around here, might be dangerous but that little bot always seemed friendly, you never know might be worth checking in on.",
            "I think they said the first dungeon was up here to the left... Well here goes nothing", }; 
    private void Awake()
    {
        WorldMapCamera.enabled = true;
        NarrativeCamera.enabled = false;
    }


    //Start Narrative
    public void onNarrativeStart()
    {
        GameObject.Find("ButtonControls").GetComponent<WorldMapCanvasControls>().onNarrationScreen();
    }

    public void onNarrative()
    {
        WorldMapCamera.enabled = false;
        NarrativeCamera.enabled = true;
        GameObject.Find("Player").GetComponent<PlayerNavMesh_Controller>().enabled = false;
    }
    public void offNarrative()
    {
        WorldMapCamera.enabled = true;
        NarrativeCamera.enabled = false;
        GameObject.Find("Player").GetComponent<PlayerNavMesh_Controller>().enabled = true;
    }

    public string getNextText(int dialogueNumber)
    {
        if (dialogueNumber > dialogues.Length - 1) return "-1";

        return dialogues[dialogueNumber];
    }

}
