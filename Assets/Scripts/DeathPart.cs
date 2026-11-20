using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPart : MonoBehaviour
{
    private static readonly Color DEATH_COLOR = new Color(0.937f, 0.267f, 0.267f); // RGB for #EF4444

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = DEATH_COLOR;
    }

    public void HitDeathPart()
    {
        GameManager.singleton.GameOver();
    }
}
