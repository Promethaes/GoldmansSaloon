using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    class ScoreboardHelper
    {
        public string name;
        public int score;

        public ScoreboardHelper(string n, int s)
        {
            name = n;
            score = s;
        }
    }

    [SerializeField]
    GameObject scoreBoardText = null;
    [SerializeField]
    Transform scoreTextParent = null;
    // Start is called before the first frame update
    void Start()
    {
        var scoreString = PlayerPrefs.GetString("HS", "");
        if (scoreString == "")
            gameObject.SetActive(false);

        var scores = scoreString.Split(',');
        List<ScoreboardHelper> helpers = new List<ScoreboardHelper>();
        foreach (var score in scores)
        {
            if (score == "")
                continue;
            var finalString = score.Split(' ');
            helpers.Add(new ScoreboardHelper(finalString[0], int.Parse(finalString[1])));
        }


        //sort by score then remove past the 10th entry
        helpers = helpers.OrderByDescending(x => x.score).ToList();
        for (int i = 9; i < helpers.Count; i++)
        {
            helpers.RemoveAt(i);
            i--;
        }

        //build scoreboard, reset player prefs HS value
        scoreString = "";
        foreach (var h in helpers)
        {
            scoreString += h.name + " " + h.score.ToString() + ",";
            var text = GameObject.Instantiate(scoreBoardText, scoreTextParent);
            text.SetActive(true);
            text.GetComponent<TMPro.TextMeshProUGUI>().text = h.name + "\t" + h.score;
        }
        Debug.Log(scoreString);
        PlayerPrefs.SetString("HS", scoreString);

        // var text = GameObject.Instantiate(scoreBoardText,scoreTextParent.transform);
    }
}
