using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] private float restartTimer = 999999999;

    // Start is called before the first frame update
    void Start()
    {
        GameState.OnGameOver += PlayerDeath;
        GameState.OnDayEnd += FinishedLevel;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        restartTimer -= Time.deltaTime;
        if(restartTimer <= 0) {
            RestartingLvl();
        }
        GameState.UpdateClock();
        if(Input.GetKeyDown(KeyBindsPlayer.lvlRestartKey)){
            RestartingLvl();
        }
    }

    void RestartingLvl(){
        GameState.NextScene = "Over";
        SceneManager.LoadScene("GameOver");
    }

    private void PlayerDeath(){
        restartTimer = 3f;
    }
    void FinishedLevel() {
        if(SceneManager.GetActiveScene().name == "Level1") {
            GameState.NextScene = "Level2";
        }
        else {
            GameState.NextScene = "Over";
        }
        SceneManager.LoadScene("GameOver");
    } 

}
