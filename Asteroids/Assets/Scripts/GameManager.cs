using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    public int lives {get; private set;} = 3;

    public int score {get; private set;} = 0;

    void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        Asteriod[] asteriods = FindObjectsOfType<Asteriod>();
        for (int i=0; i<asteriods.Length; i++) {
            Destroy(asteriods[i].gameObject);
        }

        Bullet[] bullets = FindObjectsOfType<Bullet>();
        for (int i=0; i<bullets.Length; i++) {
            Destroy(bullets[i].gameObject);
        }

        gameOverUI.SetActive(false);
        SetScore(0);
        SetLives(3);
        Respown();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

    public void OnAsteroidDestroyed(Asteriod asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.7f) {
            SetScore(score + 100);
        } else if (asteroid.size < 1.4f) {
            SetScore(score + 50);
        } else {
            SetScore(score + 25);
        }
        Debug.Log("OnAsteroidDestroyed score=" + this.score);
    }

    public void OnPlayerDeath(Player player)
    {
        player.gameObject.SetActive(false);

        this.explosion.transform.position = player.transform.position;
        this.explosion.Play();

        SetLives(lives -1);

        if (this.lives <= 0) {
            gameOverUI.SetActive(true);
        } else {
            Invoke(nameof(Respown), player.respawnDelay);
        }
    }

    private void Respown()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    
}
