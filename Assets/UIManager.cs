using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject allObjectivesUI;
    public TextMeshProUGUI allObjectives;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume game time
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Freeze game time
        isPaused = true;
    }

    public void LoadMenu(){
        // Load the main menu scene
        // SceneManager.LoadScene("MainMenu"); // Uncomment this line and add using UnityEngine.SceneManagement;
    }

    public void QuitGame(){
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Objectives(){
        pauseMenuUI.SetActive(false);
        allObjectivesUI.SetActive(true);
        allObjectives.text = MakeFullObjectiveList();
    }

    public string MakeFullObjectiveList(){
        string s = "";
        foreach (DropOffObjectiveBasic obj in GameManager.GetInstance().objectiveManager.GetComponent<ObjectiveManager>().dropOffObjectiveBasicList){
            if (obj.isCompleted){
                if (obj.cashReq == 0){
                    s += "<s>Drop of " + obj.clownReq + " clowns to the " + obj.locationReq + "</s>\n";
                }
                else{
                    s += "<s>Drop of " + obj.clownReq + " clowns to the " + obj.locationReq + " and earn at least $" + obj.cashReq + "</s>\n";
                }
            }
            else{
                if (obj.cashReq == 0){
                    s += "Drop of " + obj.clownReq + " clowns to the " + obj.locationReq + "\n";
                }
                else{
                    s += "Drop of " + obj.clownReq + " clowns to the " + obj.locationReq + " and earn at least $" + obj.cashReq + "\n";
                }
            }
        }
        return s;
    }
}