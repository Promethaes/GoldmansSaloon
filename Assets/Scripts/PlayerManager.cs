using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        int numPlayers = PlayerPrefs.GetInt("NumPlayers");

        for(int i = 0; i < numPlayers;i++){
            playerInputManager.playerPrefab = playerPrefab;
            playerInputManager.JoinPlayer(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
