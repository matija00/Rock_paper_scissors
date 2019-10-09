using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text roundNumberText;
    public Text roundResultText;
    public Sprite starImage;
    public GameObject shootingStarsFX;

    [Header("End Game Message")]
    public GameObject endGameMessage;
    public Text playerWonText;
    public Text gameScoreText;

    [Header("Players")]
    public Player playerOne;
    public Player playerTwo;

    private int _numRoundsToWin = 3;
    private int _currentRound = 1;
    private Player _roundWinner;
    private Player _gameWinner;

    void Start()
    {
        roundNumberText.text = "Round " + _currentRound;
        StartCoroutine(GameLoop());
    }

    // This coroutine is responsible for 1vs1 game mode and shows how the round proceed.
    IEnumerator GameLoop()
    {
        yield return StartCoroutine(PlayerOneTurn());

        yield return StartCoroutine(PlayerTwoTurn());

        yield return StartCoroutine(RoundEnding());

        // This code check if a winner has been found, if not it restarts game loop.
        if (_gameWinner != null)
        {
            ShowEndGameMessage();
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    IEnumerator PlayerOneTurn()
    {
        playerOne.isSelecting = true;

        if (!playerOne.selectionPanel.activeSelf)
            playerOne.EnableSelection();

        playerTwo.DisableSelection();

        while (playerOne.isSelecting)
        {
            yield return null;
        }

        playerOne.isSelecting = false;
        playerOne.DisableSelection();
    }

    IEnumerator PlayerTwoTurn()
    {
        playerTwo.isSelecting = true;

        if (!playerTwo.selectionPanel.activeSelf)
            playerTwo.EnableSelection();

        playerOne.DisableSelection();

        while (playerTwo.isSelecting)
        {
            yield return null;
        }

        playerTwo.isSelecting = false;
        playerTwo.DisableSelection();
    }

    // This coroutine updates gameUI and shows the result of the round.
    IEnumerator RoundEnding()
    {
        ShowPlayersPicks();
        yield return new WaitForSeconds(.5f);

        _roundWinner = GetRoundWinner();

        if (_roundWinner != null)
        {
            if (_roundWinner == playerOne)
            {
                playerOne.roundsWon++;
                UpdateStars(playerOne);
            }
            else
            {
                playerTwo.roundsWon++;
                UpdateStars(playerTwo);
            }
        }

        _gameWinner = GetGameWinner();
        roundResultText.text = ShowRoundResultMessage();

        yield return new WaitForSeconds(1.5f);

        _currentRound++;
        roundNumberText.text = "Round " + _currentRound;
        roundResultText.text = null;
    }

    void ShowPlayersPicks()
    {
        playerOne.ShowPlayerPickOnEnd();
        playerTwo.ShowPlayerPickOnEnd();
    }

    string ShowRoundResultMessage()
    {
        string message = "Draw!";

        if (_roundWinner != null)
            message = _roundWinner.name + " wins!";

        return message;
    }

    void ShowEndGameMessage()
    {
        endGameMessage.SetActive(true);

        playerWonText.text = _gameWinner.name + " WON!";

        gameScoreText.text = playerTwo.roundsWon + " : " + playerOne.roundsWon;
    }

    void UpdateStars(Player player)
    {
        player.stars[player.roundsWon - 1].sprite = starImage;

        Vector3 vector3 = player.stars[player.roundsWon - 1].transform.position;

        PlayStarFX(vector3);
    }

    void PlayStarFX(Vector3 vector3)
    {
        GameObject clearFX = Instantiate(shootingStarsFX, vector3, Quaternion.identity) as GameObject;
        clearFX.GetComponent<ParticleSystem>().Play();
    }

    Player GetRoundWinner()
    {
        if (playerOne.isSelecting == false && playerTwo.isSelecting == false)
        {   
            // 0 - rock, 1 - paper, 2 - scissors
            if (playerOne.pickNum == playerTwo.pickNum)
            {
                return null;
            }
            else if (playerOne.pickNum == 0 && playerTwo.pickNum == 2)
            {
                return playerOne;
            }
            else if (playerOne.pickNum == 1 && playerTwo.pickNum == 0)
            {
                return playerOne;
            }
            else if (playerOne.pickNum == 2 && playerTwo.pickNum == 1)
            {
                return playerOne;
            }
            else
            {
                return playerTwo;
            }
        }
        else
        {
            return null;
        }
    }

    Player GetGameWinner()
    {
        if (playerOne.roundsWon == _numRoundsToWin)
        {
            return playerOne;
        }
        else if (playerTwo.roundsWon == _numRoundsToWin)
        {
            return playerTwo;
        }

        return null;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
