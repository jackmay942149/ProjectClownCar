using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Add other game manager variables and functions here
    public GameObject car;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set the instance to this
            instance = this;
        }
        else if (instance != this)
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // Keep the game manager alive across scenes
        DontDestroyOnLoad(gameObject);

        // Initialize any necessary variables or setup here
    }

    // Example function to access the GameManager instance
    public static GameManager GetInstance()
    {
        return instance;
    }

    // Add other game manager functions here
}
