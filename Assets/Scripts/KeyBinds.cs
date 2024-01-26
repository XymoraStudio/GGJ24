using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    public static KeyBinds instance;

    public KeyCode lvlRestartKey = KeyCode.R;

    private void Awake() {
        instance = this;
    }
}
