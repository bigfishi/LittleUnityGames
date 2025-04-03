using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set;}

    public TileCell cell {get; private set;}

    public int number { get; private set; }

    public bool locked { get; set;}

    private Image background;
    private TextMeshProUGUI text;

    void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetState(TileState state, int number)
    {
        this.state = state;
        this.number = number;

        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = number.ToString();
    }

    public void Spawn(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null;
        }
        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTo(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        // transform.position = cell.transform.position;
        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }

    public void DestroyCell()
    {
        if (this.cell != null) {
            this.cell.tile = null;
        }

        this.cell = null;
        DestroyImmediate(gameObject);
    }

    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f;
        float duration = GameManager.Instance.tileMoveDuration;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging) {
            Destroy(gameObject);
        }
    }

    public void RunScaleAnimation(Vector3 to) {
        StartCoroutine(ScaleAnimate(to, Vector3.one));
    }

    private IEnumerator ScaleAnimate(Vector3 to, Vector3 nextTo)
    {
        float elapsed = 0f;
        float duration = 0.3f;

        Vector3 from = transform.localScale;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = to;

        StartCoroutine(ScaleAnimate(nextTo, to));
    }

    public void StopScaleAnimation() {
        StopAllCoroutines();
        transform.localScale = Vector3.one;
    }
}
