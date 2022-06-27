using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public int totalTutorialTabs;
    public int currentTutorialTab;

    public bool hasFinishedTutorial;
    public bool canStartTutorial;
    public bool wantsToRight;
    public bool wantsToLeft; 

    public List<GameObject> tutorialTabs = new List<GameObject>(); 
    public GameObject basicCombatTutorial;
    public GameObject basicMovementTutorial;
    public GameObject abilityTutorial;
    public GameObject pilarTutorial;

    public PlayerInputCheck input;

    private void Start()
    {
        hasFinishedTutorial = false;

        foreach (GameObject tab in tutorialTabs)
        {
            tab.SetActive(false);
        }

        totalTutorialTabs = tutorialTabs.Count;

    }

    private void Update()
    {
        if (input.dPadRightPressed)
        {
            wantsToRight = true; 
        }


        if (input.dPadLeftPressed)
        {
            wantsToLeft = true; 
        }

        if(!hasFinishedTutorial && canStartTutorial) CheckForTutorial(); 
    }

    void CheckForTutorial()
    {
        if(!input.dPadRightPressed && wantsToRight && currentTutorialTab < totalTutorialTabs)
        {
            NextTab();
            wantsToRight = false;
            wantsToLeft = false; 
        }
        else if(!input.dPadRightPressed && wantsToRight && currentTutorialTab >= totalTutorialTabs)
        {
            StopTutorial(); 
        }

        if(!input.dPadLeftPressed && wantsToLeft && currentTutorialTab > 0)
        {
            wantsToLeft = false;
            wantsToRight = false;
            PreviousTab(); 
        }
    }

    public void StartTutorial()
    {
        currentTutorialTab = 0;
        tutorialTabs[currentTutorialTab].SetActive(true);
        canStartTutorial = true;
    }

    void StopTutorial()
    {
        hasFinishedTutorial = true;
        foreach (GameObject tab in tutorialTabs)
        {
            tab.SetActive(false);
        }
    }

    void NextTab()
    {
        currentTutorialTab++; 
        
        foreach(GameObject tab in tutorialTabs)
        {
            tab.SetActive(false);
        }

        tutorialTabs[currentTutorialTab].SetActive(true); 
    }

    void PreviousTab()
    {
        currentTutorialTab--;

        foreach (GameObject tab in tutorialTabs)
        {
            tab.SetActive(false);
        }

        tutorialTabs[currentTutorialTab].SetActive(true);
    }
}
