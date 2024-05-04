using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<DropOffObjectiveBasic> dropOffObjectiveBasicList = new List<DropOffObjectiveBasic>();

    public void CheckObjectives(int clowns, string dropOffName, int cash)
    {
        foreach (DropOffObjectiveBasic obj in dropOffObjectiveBasicList)
        {
            if (obj.locationReq == dropOffName && clowns >= obj.clownReq && cash >= obj.cashReq)
            {
                obj.isCompleted = true;
            }
        }
    }
}
