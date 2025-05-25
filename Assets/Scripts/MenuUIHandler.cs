// MenuUIHandler.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For TextMeshPro UI components (Input Field & Text)
#if UNITY_EDITOR // Only include EditorApplication if compiled in editor
using UnityEditor;
#endif
// Removed: using UnityEngine.UI; (only if you're fully on TMPro)
// Removed: using UnityEngine.UIElements; (if not using UI Toolkit)

public class MenuUIHandler : MonoBehaviour
{
    // These references are assigned in the Inspector for *this scene's* UI elements.
    public TMP_InputField nameInputField; // For where the player types their name
    public TextMeshProUGUI highScoreTextDisplay; // For displaying the high score

    void Start()
    {
        // Ensure the MenuManager instance exists and is ready
        if (MenuManager.Instance == null)
        {
            Debug.LogError("MenuManager.Instance is not found! Make sure it exists in the first scene and is set up as a Singleton with DontDestroyOnLoad.");
            return;
        }

        // --- Initialize UI elements with data from the persistent MenuManager ---
        if (nameInputField != null)
        {
            // Display the player name loaded by MenuManager
            nameInputField.text = MenuManager.Instance.currentPlayerName;

            // Crucially: Hook up the input field's OnEndEdit event
            // This ensures that when the user finishes typing, the MenuManager's string is updated.
            nameInputField.onEndEdit.AddListener(MenuManager.Instance.SetPlayerName);
        }

        if (highScoreTextDisplay != null)
        {
            // Display the high score loaded by MenuManager
            highScoreTextDisplay.text = "High Score: " + MenuManager.Instance.currentPlayerName + " : " + MenuManager.Instance.highScore;
        }
    }

    public void ClearAndUpdateScoreDisplay()
    {
        MenuManager.Instance.ClearSaveData();
        highScoreTextDisplay.text = "High Score: " + MenuManager.Instance.currentPlayerName + " : " + MenuManager.Instance.highScore;
    }
    // This method is called when the "Start New Game" button is clicked
    public void StartNew()
    {
        // Optional: Ensure the latest name from the input field is set in the manager
        // This is a fallback if OnEndEdit didn't fire (e.g., user clicks start without deselecting input)
        if (nameInputField != null)
        {
            MenuManager.Instance.SetPlayerName(nameInputField.text);
        }

        // Save current game state (player name, potentially high score if it was updated)
        MenuManager.Instance.SaveHighscore();

        // Load your main game scene
        SceneManager.LoadScene(1);
    }

    // This method is called when the "Exit" button is clicked
    public void Exit()
    {
        // Optional: Save game data before quitting
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.SaveHighscore();
        }

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}