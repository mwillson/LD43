using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private bool paused = false;

    [SerializeField]
    private GameObject pauseMenuObject;

    [SerializeField]
    private GameObject gameOverUIObject;

    [SerializeField]
    private Manager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        if (!paused)
        {
            paused = true;
            gameManager.paused = true;
            pauseMenuObject.SetActive(true);
        }
        else
        {
            paused = false;
            gameManager.paused = false;
            pauseMenuObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        GameObject.FindObjectOfType<HighScore>().SubmitNewScore(gameManager.score);
        gameManager.ChangeTexture1(gameManager.world1Texture);
        SceneManager.LoadScene("mainmenu");
    }

    public void RestartGame()
    {
        Debug.LogError("restarting!");
        paused = false;
        gameManager.paused = false;
        gameOverUIObject.SetActive(false);

        GameObject.FindObjectOfType<HighScore>().SubmitNewScore(gameManager.score);
        gameManager.ChangeTexture1(gameManager.world1Texture);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
