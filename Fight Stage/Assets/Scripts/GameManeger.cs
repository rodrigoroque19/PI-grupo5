using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour {

    public GameObject PainelCompleto;

    bool isPause = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Pause()
    {        
        if (isPause)
        {
            PainelCompleto.SetActive(false);
            isPause = false;
        }
        else
        {
            PainelCompleto.SetActive(true);
            isPause = true;
        }
    }
}
