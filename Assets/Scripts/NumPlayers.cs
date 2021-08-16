using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumPlayers : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textMeshProUIGUI;
    public int numPlayers = 1;
    public GameObject p2Image;

    public void AddPlayer()
    {
        if (numPlayers == 1)
            numPlayers++;
        p2Image.SetActive(true);
        textMeshProUIGUI.text = numPlayers.ToString();
    }

    public void RemovePlayer()
    {
        if (numPlayers == 2)
            numPlayers--;
        p2Image.SetActive(false);
        textMeshProUIGUI.text = numPlayers.ToString();
    }

    public void SaveNumPlayers(){
        PlayerPrefs.SetInt("NumPlayers",numPlayers);
    }
}
