﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour {

    public GameObject PainelCompleto;
    public Text displayContagem;

    bool isPause = false;
    float contagem = 30f;

	// Use this for initialization
	void Start ()
    {
        displayContagem.text = displayContagem.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (contagem > 0.0f)
        {
            contagem -= Time.deltaTime;
            displayContagem.text = contagem.ToString("F2");
        }
        
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

    public void Cronometro()
    {

    }
}
