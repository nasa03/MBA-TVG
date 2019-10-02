﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1TextStuff : MonoBehaviour
{
    public Text screentext;

    private void Start()
    {
        StartCoroutine("HideText");
    }

    public void showtext(string texttoshow)
    {
        StopCoroutine("HideText");
        screentext.text = texttoshow;
        StartCoroutine("HideText");
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(10f);
        screentext.text = "";
    }
}
