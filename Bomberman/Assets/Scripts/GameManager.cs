using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject[] players;

    void Awake()
    {
        if (Instance != null) {
            Destroy(Instance);
            Instance = this;
        } else {
            Instance = this;
        }
    }

    private void OnDestroy() {
        Instance = null;
    }

    public void CheckWinState()
    {
        int aliveCount = 0;

        foreach(GameObject player in players)
        {
            if (player.activeSelf) {
                aliveCount++;
            }
        }

        if (aliveCount<=1) {
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
