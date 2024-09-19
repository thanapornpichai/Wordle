using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wordle : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI randomWordText, resultText;
    [SerializeField]
    private TextMeshProUGUI[] guessRows, currentGuessUI;
    [SerializeField]
    private Button[] guessRowBackgrounds, keyboardButtons;
    [SerializeField]
    private Button deleteButton, enterButton;
    [SerializeField]
    private GameObject playAgainButton;

    private string[] wordList = { "zebra", "debuz", "water", "piano", "house" };
    private string targetWord, currentGuess;
    private int currentRow, letterCount;
    private int maxGuesses = 6;

    void Start()
    {
        targetWord = wordList[Random.Range(0, wordList.Length)];
        randomWordText.text = targetWord.ToUpper();

        ResetUI();

        foreach (Button btn in keyboardButtons)
        {
            btn.onClick.AddListener(() => OnKeyboardClick(btn));
        }
        deleteButton.onClick.AddListener(OnDeleteClick);
        enterButton.onClick.AddListener(OnEnterClick);
        playAgainButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void ResetUI()
    {
        currentRow = 0;
        currentGuess = "";
        foreach (TextMeshProUGUI row in guessRows)
        {
            row.text = "";
        }
        for (int i = 0; i < guessRowBackgrounds.Length; i++)
        {
            guessRowBackgrounds[i].GetComponent<Image>().color = Color.black;
        }
        resultText.text = "";
    }

    void OnKeyboardClick(Button btn)
    {
        if (currentGuess.Length < 5)
        {
            string pressLetter = btn.GetComponentInChildren<TextMeshProUGUI>().text.ToUpper();
            UpdateGuessUI(letterCount, pressLetter);
            letterCount++;
            pressLetter = pressLetter.ToLower();
            currentGuess += pressLetter;
        }
    }

    void OnDeleteClick()
    {
        if (currentGuess.Length > 0)
        {
            currentGuess = currentGuess.Substring(0, currentGuess.Length - 1);
            letterCount--;
            UpdateGuessUI(letterCount, " ");
        }
    }

    void OnEnterClick()
    {
        if (currentGuess.Length == 5)
        {
            CheckGuess();
        }
    }

    void UpdateGuessUI(int i, string pressLetter)
    {
        currentGuessUI[i].text = pressLetter;
    }

    void CheckGuess()
    {
        if (currentGuess == targetWord)
        {
            resultText.text = "You win!";
            ShowCorrectGuessInRow();
            EndGame();
        }
        else
        {
            UpdateGuessBackgroundColors();
            UpdateKeyboardColors();
            currentRow++;
            currentGuess = "";
            if (currentRow >= maxGuesses)
            {
                resultText.text = "You lose! The word was: " + targetWord;
                EndGame();
            }
        }
    }

    void UpdateGuessBackgroundColors()
    {
        for (int i = 0; i < 5; i++)
        {
            char letter = currentGuess[i];
            string letterString = letter.ToString();

            int index = currentRow * 5 + i;

            if (letter == targetWord[i])
            {
                guessRowBackgrounds[index].GetComponent<Image>().color = Color.green;
            }
            else if (targetWord.Contains(letterString))
            {
                guessRowBackgrounds[index].GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                guessRowBackgrounds[index].GetComponent<Image>().color = Color.gray;
            }
        }
    }

    void ShowCorrectGuessInRow()
    {
        for (int i = 0; i < 5; i++)
        {
            int index = currentRow * 5 + i;

            guessRowBackgrounds[index].GetComponent<Image>().color = Color.green;
        }
    }

    void UpdateKeyboardColors()
    {
        for (int i = 0; i < 5; i++)
        {
            char letter = currentGuess[i];
            string letterString = letter.ToString();

            if (letter == targetWord[i])
            {
                SetKeyboardColor(letterString, Color.green);
            }
            else if (targetWord.Contains(letterString))
            {
                SetKeyboardColor(letterString, Color.yellow);
            }
            else
            {
                SetKeyboardColor(letterString, Color.gray);
            }
        }
    }

    void SetKeyboardColor(string letter, Color color)
    {
        foreach (Button btn in keyboardButtons)
        {
            if (btn.GetComponentInChildren<TextMeshProUGUI>().text.ToLower() == letter)
            {
                btn.GetComponent<Image>().color = color;
            }
        }
    }

    void EndGame()
    {
        playAgainButton.SetActive(true);
    }

    public void OnPlayAgainClick()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
