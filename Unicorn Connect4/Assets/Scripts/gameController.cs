using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public enum gameState
    {
        Gameplay = 0,
        yellowWin = 1,
        redWin = 2
    }

    public bool activeColour; //1-red 0-yellow


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
