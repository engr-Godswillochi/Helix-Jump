using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GameManager.singleton.NextLevel();
    }
}