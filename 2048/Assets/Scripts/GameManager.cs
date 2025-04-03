using UnityEngine;
using System.Collections;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public TileBoard board;

    public CanvasGroup gameOver;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;

    public Button btnHammer;
    public DestroyHammer hammer;

    private int score;

    void Awake()
    {
        if (Instance != null) {
            Destroy(Instance);
        } else {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        hammer.gameObject.SetActive(false);
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        hiscoreText.text = LoadHiscore().ToString();
        
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void InscreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;

        scoreText.text = score.ToString();

        SaveHiscore();
    }

    private void SaveHiscore()
    {
        int hiscore = LoadHiscore();

        if (score > hiscore)
        {
            PlayerPrefs.SetInt("hiscore", score);
            hiscoreText.text = score.ToString();
        }
    }

    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }

    public void OnBtnHammerClick()
    {
        board.SetHammerState(true);
        // 进入抡锤状态，暂停游戏棋盘逻辑，高亮有数字的位置，点击空白区域退出抡锤状态，继续棋盘逻辑，点击数字，从锤子位置播放炸弹人拿着锤子跑到数字位置，执行锤子旋转动画，隐藏炸弹人和锤子，然后消除数字，继续棋盘逻辑
    }

    // 显示锤子移动，砸在cell上，消除cell
    public void HammerHitCell(Tile tile) {
        // 隐藏锤子按钮
        btnHammer.gameObject.SetActive(false);
        // 显示锤子，并移动到tile 锤子执行旋转的动画
        hammer.gameObject.SetActive(true);
        hammer.PlayHitAnimation(tile.gameObject.transform.position);
        // 删除tile
        StartCoroutine(HitTile(tile));


        // TODO
        // 隐藏锤子按钮
        // 显示炸弹人和锤子
        // 移动炸弹人和锤子
        // 锤子执行旋转的动画
        // 播放粒子特效
        // 删除tile
        // 结束
    }

    // 移除tile
    private IEnumerator HitTile(Tile tile)
    {
        yield return new WaitForSeconds(1.0f);

        board.RemoveTile(tile);

        OnHammerHitEnd();
    }

    // 锤子结束
    public void OnHammerHitEnd() {
        board.SetHammerHitting(false, null);
        board.SetHammerState(false);

        btnHammer.gameObject.SetActive(true);
        hammer.transform.position = btnHammer.transform.position;
        hammer.gameObject.SetActive(false);
    }
}
