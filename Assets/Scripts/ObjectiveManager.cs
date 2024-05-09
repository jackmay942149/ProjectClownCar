using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ObjectiveManager : MonoBehaviour
{
    public List<DropOffObjectiveBasic> dropOffObjectiveBasicList = new List<DropOffObjectiveBasic>();
    public TextMeshProUGUI objectiveText;
    public List<DropOffObjectiveBasic> mainObjectives;
    public float objectiveStrikethroughTime;
    private Coroutine fadeEffect;
    private float alpha = 1f;

    void Start()
    {
        UpdateMainObjectives();
        EnableText();
    }

    void Update()
    {
        UpdateObjectiveUI();
    }

    public void CheckObjectives(int clowns, string dropOffName, int cash)
    {
        foreach (DropOffObjectiveBasic obj in dropOffObjectiveBasicList)
        {
            if (obj.locationReq == dropOffName && clowns >= obj.clownReq && cash >= obj.cashReq)
            {
                obj.isCompleted = true;
            }
        }
        Invoke("UpdateMainObjectives", objectiveStrikethroughTime);
        EnableText();        
    }

    public void UpdateMainObjectives()
    {
        int counter = 0;
        for (int i = 0; i < 3; i++)
        {
            mainObjectives[i] = dropOffObjectiveBasicList[counter];
            counter++;
            if (mainObjectives[i].isCompleted) {i--;}
        }
    }

    public void UpdateObjectiveUI()
    {
        string s = "Objectives: \n";
        for (int i = 0; i < 3; i++)
        {
            if (mainObjectives[i].isCompleted)
            {
                if (mainObjectives[i].cashReq == 0)
                {
                    s += "<s>Drop of " + mainObjectives[i].clownReq + " clowns to the " + mainObjectives[i].locationReq + "</s>\n";
                }
                else
                {
                    s += "<s>Drop of " + mainObjectives[i].clownReq + " clowns to the " + mainObjectives[i].locationReq + " and earn at least $" + mainObjectives[i].cashReq + "</s>\n";
                }
            }
            else
            {
                if (mainObjectives[i].cashReq == 0)
                {
                    s += "Drop of " + mainObjectives[i].clownReq + " clowns to the " + mainObjectives[i].locationReq + "\n";
                }
                else
                {
                    s += "Drop of " + mainObjectives[i].clownReq + " clowns to the " + mainObjectives[i].locationReq + " and earn at least $" + mainObjectives[i].cashReq + "\n";
                }
            }
            
        }
        objectiveText.text = s;
    }

    public void EnableText()
    {
        if (fadeEffect != null) {StopCoroutine(fadeEffect);}
        alpha = 1f;
        objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 1.0f);
        fadeEffect = StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        // Loop until alpha reaches 0
        while (alpha > 0f)
        {
            // Reduce alpha over time
            alpha -= Time.deltaTime / objectiveStrikethroughTime;

            // Set the text's alpha
            objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, alpha);

            // Wait for the next frame
            yield return null;
        }
    }

}
