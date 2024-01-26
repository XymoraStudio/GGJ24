using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    public static KeyBinds instance;

    public KeyCode lvlRestartKey = KeyCode.R;
    public KeyCode gunShoot = KeyCode.Mouse0;

    private void Awake() {
        instance = this;
    }
}
