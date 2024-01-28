using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int timeAfterDeathToRestart = 3;

    private void Awake() {
        GameState.currentNumberOfActiveEnemies = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameState.OnGameOver += PlayerDeath;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GameState.UpdateClock();
        if(Input.GetKeyDown(KeyBinds.lvlRestartKey)){
            RestartingLvl();
        }
    }

    void RestartingLvl(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlayerDeath(){
        Invoke(nameof(RestartingLvl), timeAfterDeathToRestart);
    }
}
