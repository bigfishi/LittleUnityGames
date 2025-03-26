using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Ball ball;

    public Text playerScoreText;
    public Text computerScoreText;

    public PlayerPaddle playerPaddle;
    public ComputerPaddle computerPaddle;

    
    private int _playerScore;
    private int _computerScore;

    public void PlayerScores()
    {
        _playerScore++;

        playerScoreText.text = _playerScore.ToString();
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();
        ball.AddStartingForce();
    }

    public void ComputerScores()
    {
        _computerScore++;

        computerScoreText.text = _computerScore.ToString();
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();
        ball.AddStartingForce();
    }
}
