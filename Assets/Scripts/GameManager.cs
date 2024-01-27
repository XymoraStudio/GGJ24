using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int timeAfterDeathToRestart = 3;

    private void Awake() {
        instance = this;
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
        if(Input.GetKeyDown(KeyBinds.instance.lvlRestartKey)){
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
