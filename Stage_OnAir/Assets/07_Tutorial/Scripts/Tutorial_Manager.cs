using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Manager : MonoBehaviour
{
    public List<Tutorial> Tutorials = new List<Tutorial>();

    public GameObject expImage;
    public Text expText;

    private Tutorial currentTutorial;

    private static Tutorial_Manager instance;
    public static Tutorial_Manager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<Tutorial_Manager>();

            if (instance == null)
                Debug.Log("There is no TutorialManager");

            return instance;
        }
    }

    void Start()
    {
        SetNextTutorial(0);
    }

    void Update()
    {
        if (currentTutorial)
            currentTutorial.CheckIfHappening();
    }

    public void CompletedTutorial()
    {
        SetNextTutorial(currentTutorial.Order + 1);
    }

    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTurorialByOrder(currentOrder);

        if (!currentTutorial)
        {
            CompletedAllTutorials();
            return;
        }

        expText.text = currentTutorial.Explanation;
    }

    public void CompletedAllTutorials()
    {
        expText.text = "You have completed all the tutorials";

        //loadnextscene
    }

    public Tutorial GetTurorialByOrder(int Order)
    {
        for (int i = 0; i < Tutorials.Count; i++)
        {
            if (Tutorials[i].Order == Order)
                return Tutorials[i];
        }

        return null;
    }
}
