using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objectives/DropOffObjectiveBasic")]
public class DropOffObjectiveBasic : ScriptableObject
{
    public int id;
    public string locationReq;
    public int clownReq;
    public int cashReq;
    public bool isCompleted;
}
