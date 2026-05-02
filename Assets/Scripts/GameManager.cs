using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int leftPlayerScore = 0;
    [SerializeField] private int rightPlayerScore = 0;

    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;

    [SerializeField] private BallGeneration ballGeneration;

    [SerializeField] private GameObject player2;


    private IAPaddle iaPaddle;
    private Player2Controller player2Controller;

    private void Awake()
    {
        iaPaddle = player2.GetComponent<IAPaddle>();
        player2Controller = player2.GetComponent<Player2Controller>();
    }



    // Method to change de gameMode of the game when 0 = singlePlayer, 1 = multiplayer
    public void SetGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.SinglePlayer:
                iaPaddle.enabled = true;
                player2Controller.enabled = false;
                break;

            case GameMode.MultiPlayer:
                iaPaddle.enabled = false;
                player2Controller.enabled = true;
                break;
        }
    }

    public void setDifficulty (DifficultyLevel difficulty)
    {
        switch(difficulty)
        {
            case DifficultyLevel.Easy:
                iaPaddle.speed = 3f;      
                iaPaddle.maxError = 1f;   
                iaPaddle.deadZone = 0.3f; 
                break;

            case DifficultyLevel.Normal:
                iaPaddle.speed = 5f;
                iaPaddle.maxError = 0.5f;
                iaPaddle.deadZone = 0.2f;
                break;


            case DifficultyLevel.Hard:
                iaPaddle.speed = 6f;
                iaPaddle.maxError = 0.3f;
                iaPaddle.deadZone = 0.1f;
                break;

        }
    }



    public void AddPointToLeftPlayer()
    {
        leftPlayerScore++;
        leftScoreText.text = leftPlayerScore.ToString();
    }

    public void AddPointToRightPlayer()
    {
        rightPlayerScore++;
        rightScoreText.text = rightPlayerScore.ToString();
    }

    public void SpawnBall()
    {
        ballGeneration.GenerateBall();
    }

    void Start()
    {
        GameMode gameMode = GameController.Instance.GameMode;
        DifficultyLevel difficulty = GameController.Instance.Difficulty;
        SetGameMode(gameMode);   
        setDifficulty(difficulty);
    }

}

