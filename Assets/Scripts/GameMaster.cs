using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameMaster : MonoBehaviour
{
    public UnityEvent OnAllPlayersDead;
    public UnityEvent OnStartCounterFinished;
    public UnityEvent OnBeginGame;
    List<PlayerController> players = new List<PlayerController>();

    bool _allPlayersDead = false;
    // Start is called before the first frame update
    void Start()
    {
        void Master_OnBeginGame()
        {
            var p = FindObjectsOfType<PlayerController>();
            foreach (var pl in p)
                players.Add(pl);
        }
        void Master_OnAllPlayersDead()
        {
            var entities = FindObjectsOfType<EntityHealth>();
            for (int i = 0; i < entities.Length; i++)
                Destroy(entities[i].gameObject);
        }

        IEnumerator StartCounter()
        {
            yield return new WaitForSeconds(4.0f);
            OnStartCounterFinished.Invoke();
            yield return new WaitForSeconds(0.5f);
            OnBeginGame.Invoke();
        }
        StartCoroutine(StartCounter());

        OnAllPlayersDead.AddListener(Master_OnAllPlayersDead);
        OnBeginGame.AddListener(Master_OnBeginGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (_allPlayersDead || players.Count == 0)
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
