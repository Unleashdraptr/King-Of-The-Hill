using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool PauseState;
    //Wave Info that will be changed during GamePlay
    public TextMeshProUGUI WaveTimer;
    public TextMeshProUGUI WaveNum;
    int WaveCountdown;
    WaveManager WaveNums;

    public Slider HealthBar;

    //Win or Lost States and their UI to corespond to it
    public GameObject WinState;
    public GameObject LostState;
    public GameObject RetryButton;
    public void Restart(){ SceneManager.LoadScene(0); }
    public void ExitGame(){ Application.Quit(); }


    // This is where we'll have the game states be sorted e.g., Win, Lose, Pausing
    void Start()
    {
        WaveNums = GameObject.Find("AIManager").GetComponent<WaveManager>();
        WinState.SetActive(false);
        LostState.SetActive(false);
        RetryButton.SetActive(false);
        WaveTimer.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
        PauseState = false;
    }

    void Update()
    {
        HealthBar.value = GameObject.Find("Player").GetComponent<PlayerControls>().Hp;

        WaveTimer.SetText(Mathf.Round(WaveNums.Countdown + WaveNums.WaveTimer).ToString());
        if(Mathf.Round(WaveNums.Countdown + WaveNums.WaveTimer) <= 1)
            WaveNum.SetText("Wave: " + (WaveNums.WaveNum + 1) + "/20".ToString());
    }
    public void WonGame()
    {
        WinState.SetActive(true);
        RetryButton.SetActive(true);
        PauseState = true;
        WaveTimer.gameObject.SetActive(false);
    }
    public void LostGame()
    {
        LostState.SetActive(true);
        RetryButton.SetActive(true);
        PauseState = true; 
        WaveTimer.gameObject.SetActive(false);
        GameObject.Find("Wave Num").GetComponent<TextMeshProUGUI>().SetText((WaveNums.WaveNum + 1).ToString());
    }
}
