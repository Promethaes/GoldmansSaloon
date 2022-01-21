using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    public UnityEvent OnAllPlayersDead;
    List<PlayerController> players = new List<PlayerController>();

    bool _allPlayersDead = false;
    // Start is called before the first frame update
    void Start()
    {
        var p = FindObjectsOfType<PlayerController>();
        foreach (var pl in p)
            players.Add(pl);
    }

    // Update is called once per frame
    void Update()
    {
        if (_allPlayersDead)
            return;
        foreach (var p in players)
        {
            if (p.hp > 0)
                return;
        }
        _allPlayersDead = true;
        foreach (var p in players)
            p.GetComponent<HighscoreManager>().OnAllPlayersDead();
        OnAllPlayersDead.Invoke();
    }
}
