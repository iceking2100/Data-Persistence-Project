using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreText2;
    public GameObject GameOverText;

    private bool m_Started = false;
    [SerializeField] public static int m_Points;
    [SerializeField] public static int highScore;

    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText2.text = $"Best Score: {MenuManager.Instance.currentPlayerName} : {MenuManager.Instance.highScore}";
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
                ExitToMenu();
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > MenuManager.Instance.highScore)
        {
            // If it's a new high score, update the MenuManager's high score
            MenuManager.Instance.highScore = m_Points;

            // Optional: You could add a visual indicator here, e.g., "NEW HIGH SCORE!"
            Debug.Log("New High Score Achieved: " + m_Points);
        }

        // 2. Save the (potentially updated) high score to the file
        //    This calls the SaveHighscore method on your persistent MenuManager.
        MenuManager.Instance.SaveHighscore();

        // 3. Display the final best score and player name on the UI
        //    Retrieve the player name and the (potentially new) high score from the MenuManager.
        ScoreText2.text = $"Best Score: {MenuManager.Instance.currentPlayerName} : {MenuManager.Instance.highScore}";

        // If you also want to show the score for the *current* game round (not just the high score)
        // You would need a separate TextMeshProUGUI for that:
        // CurrentScoreText.text = $"Your Score: {m_Points}";
    }


    public void ExitToMenu()
    {

        SceneManager.LoadScene(0);


    }
}
