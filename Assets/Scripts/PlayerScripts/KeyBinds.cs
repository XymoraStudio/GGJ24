using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    public static KeyBinds instance;

    public KeyCode lvlRestartKey = KeyCode.R;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode attack = KeyCode.Mouse0;
    public KeyCode jump = KeyCode.Space;
    public KeyCode dash = KeyCode.E;
    
    private void Awake() {
        instance = this;
    }
}
