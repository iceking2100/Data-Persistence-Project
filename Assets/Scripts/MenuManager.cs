// MenuManager.cs
using System;
using System.IO;
using UnityEngine;
// No need for UnityEngine.UI or TMPro in this class if it only handles data
// You can remove: using UnityEngine.UI; using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public int highScore;
    public string currentPlayerName; // This is the actual string that persists

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore(); // Load data when the manager is first created
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string playerName; // This must be a string to be serializable
    }

    // Method to be called to update the player name string
    // This is called by the UI handler when the input field changes
    public void SetPlayerName(string name)
    {
        currentPlayerName = name;
        Debug.Log("MenuManager: Current player name set to: " + currentPlayerName);
    }

    // Method to be called to save the current data
    public void SaveHighscore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.playerName = currentPlayerName; // Save the string value

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Game data saved.");
    }

    // Method to load data
    void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            currentPlayerName = data.playerName; // Load the string value

            Debug.Log($"Loaded - High Score: {highScore}, Player Name: {currentPlayerName}");
        }
        else
        {
            // Set initial defaults if no save file exists
            Debug.Log("No save file found. Initializing defaults.");
            ResetGameData(); // Use the new reset method for defaults too
        }
    }

    // NEW METHOD: Clears the save file and resets in-memory data
    public void ClearSaveData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at: " + path);
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }

        // Reset the in-memory data to default values
        ResetGameData();

        // Optional: After clearing, you might want to immediately save the new default state
        // This creates a fresh save file with defaults instead of just deleting it.
        SaveHighscore(); // Uncomment if you want to create a new default save file immediately
        LoadHighScore();
    }

    // NEW HELPER METHOD: Resets the in-memory game data to default values
    private void ResetGameData()
    {
        highScore = 0;
        currentPlayerName = "Player"; // Default name
        Debug.Log("Game data reset to defaults in memory.");
    }
}