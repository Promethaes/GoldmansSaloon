using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class HighscoreManager : MonoBehaviour
{
    [System.Serializable]
    public class LetterHelper
    {
        public TMPro.TextMeshProUGUI letter;
        public GameObject upArrow;
        public GameObject downArrow;
        public void SetSelected(bool yn)
        {
            upArrow.SetActive(yn);
            downArrow.SetActive(yn);
        }

    }
    public GameObject highscoreCanvas;
    public LetterHelper firstLetter;
    public LetterHelper secondLetter;
    public LetterHelper thirdLetter;
    public PlayerController owner;

    List<PlayerController> players = new List<PlayerController>();

    string scoreString = "";
    int letterIndex = 0;
    List<LetterHelper> letters = new List<LetterHelper>();

    bool endsceen = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        letters.Add(firstLetter);
        letters.Add(secondLetter);
        letters.Add(thirdLetter);

        scoreString = PlayerPrefs.GetString("HS", "");
        var p = FindObjectsOfType<PlayerController>();
        foreach (var pl in p)
            players.Add(pl);
    }
    public void OnAllPlayersDead()
    {
        foreach (var p in players)
        {
            if (p == owner)
                continue;
            if (p.currentScore > owner.currentScore || p.currentScore == owner.currentScore && p.gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                owner.gameObject.SetActive(false);
                return;
            }
        }
        endsceen = true;
        highscoreCanvas.SetActive(true);
        owner.GetComponent<PlayerInput>().SwitchCurrentActionMap("Highscore");

    }

    IEnumerator ArrowEffect(GameObject arrow)
    {
        var button = arrow.GetComponent<Button>();
        var buttonImage = arrow.GetComponent<Image>();
        buttonImage.color = button.colors.selectedColor;
        yield return new WaitForSeconds(0.125f);
        buttonImage.color = Color.white;
    }

    public void OnUpArrow(CallbackContext ctx)
    {
        if (!ctx.performed || !endsceen)
            return;

        var letter = letters[letterIndex].letter;
        StartCoroutine(ArrowEffect(letters[letterIndex].upArrow));
        if ((char)((int)letter.text[0] + 1) > 90)
            letter.text = "" + (char)65;
        else
            letter.text = "" + (char)((int)letter.text[0] + 1);
    }

    public void OnDownArrow(CallbackContext ctx)
    {
        if (!ctx.performed || !endsceen)
            return;

        var letter = letters[letterIndex].letter;
        StartCoroutine(ArrowEffect(letters[letterIndex].downArrow));
        if ((char)((int)letter.text[0] - 1) < 65)
            letter.text = "" + (char)90;
        else
            letter.text = "" + (char)((int)letter.text[0] - 1);
    }

    public void OnAcceptLetter(CallbackContext ctx)
    {
        if (!ctx.performed || !endsceen)
            return;

        letters[letterIndex].SetSelected(false);
        if (letterIndex + 1 > letters.Count - 1)
        {
            OnSubmitHighscore();
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            return;
        }

        letterIndex++;
        letters[letterIndex].SetSelected(true);

    }
    public void OnCancelLetter(CallbackContext ctx)
    {
        if (!ctx.performed || letterIndex == 0 || !endsceen)
            return;

        letters[letterIndex].SetSelected(false);
        letterIndex--;
        letters[letterIndex].SetSelected(true);
    }

    public void OnSubmitHighscore()
    {
        scoreString += firstLetter.letter.text + secondLetter.letter.text + thirdLetter.letter.text + " " + owner.currentScore.ToString() + ",";
        PlayerPrefs.SetString("HS", scoreString);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("HS"));
    }
}
