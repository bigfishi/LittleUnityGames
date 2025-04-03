using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;
    public Tile tilePrefab;
    public TileState[] tileStates;
    private TileGrid grid;

    private List<Tile> tiles;

    private bool waiting;

    public bool hammerStete;
    public bool hammerHitting;

    private enum Orient {
        Unknown,
        Left,
        Right,
        Up,
        Down
    }

    
    private Vector2 startPosition = Vector2.zero;
    private bool isDragging = false;

    void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();

        tiles = new List<Tile>(16);
    }

    public void ClearBoard()
    {
        foreach ( var cell in grid.cells)
        {
            cell.tile = null;
        }
        foreach ( var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    public void CreateTile()
    {
        Tile tile =  Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], 2);
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    void Update()
    {
        if (hammerStete) {
            CheckHammerPosition();
            return;
        }
        if (waiting) {
            return;
        }
        Orient orient = GetOrient();
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || orient == Orient.Up) {
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || orient == Orient.Down) {
            MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || orient == Orient.Left) {
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || orient == Orient.Right) {
            MoveTiles(Vector2Int.right, grid.width -2, -1, 0, 1);
        }
    }

    private void CheckHammerPosition()
    {
        if (!hammerStete) {
            return;
        }
        if (Input.GetMouseButtonDown(0)) // 检测到鼠标左键点击
        {
            Tile targetTile = null;
            Vector2 mousePos = Input.mousePosition;
            foreach (var tile in tiles)
            {
                RectTransform tileTransform = tile.GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(tileTransform, mousePos))
                {
                    targetTile = tile;
                    break;
                }
            }
            if (targetTile == null) {
                SetHammerState(false);
            } else {
                SetHammerHitting(true, targetTile);
            }
        }
    }

    public void RemoveTile(Tile tile)
    {
        tile.DestroyCell();
        tiles.Remove(tile);
    }

    private Orient GetOrient()
    {
        // Debug.Log("GetOrient Input.touchCount=" + Input.touchCount);
 
        // 检查是否有触摸输入
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // 获取第一个触摸点
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position; // 记录触摸开始位置
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                Vector2 endPosition = touch.position; // 获取触摸结束位置
                float distance = Vector2.Distance(startPosition, endPosition); // 计算距离
                if (distance > 50) // 设置一个最小滑动距离阈值，例如50像素
                {
                    Vector2 direction = endPosition - startPosition; // 计算滑动方向向量
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // 如果水平移动大于垂直移动，则认为是左右滑动
                    {
                        if (direction.x > 0)
                        {
                            Debug.Log("右滑"); // 右滑处理
                            return Orient.Right;
                        }
                        else if (direction.x < 0)
                        {
                            Debug.Log("左滑"); // 左滑处理
                            return Orient.Left;
                        }
                    }
                    else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
                        if (direction.y > 0)
                        {
                            Debug.Log("上滑");
                            return Orient.Up;
                        }
                        else if (direction.y < 0)
                        {
                            Debug.Log("下滑");
                            return Orient.Down;
                        }
                    }
                }
                isDragging = false; // 重置拖动状态
            }
        }
        return Orient.Unknown;
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x=startX; x>=0 && x<grid.width; x+=incrementX)
        {
            for (int y=startY; y>=0 && y<grid.height; y+=incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied) {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacentCell = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacentCell != null)
        {
            if (adjacentCell.occupied)
            {
                if (CanMerge(tile, adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);
                    return true;
                }

                break;
            }

            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }

        if (newCell != null) {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length-1);
        int number = b.number * 2;

        b.SetState(tileStates[index], number);

        gameManager.InscreaseScore(number);
    }

    private int IndexOf(TileState state)
    {
        for (int i=0;i<tileStates.Length;i++)
        {
            if (state == tileStates[i]) {
                return i;
            }
        }
        return -1;
    }

    public IEnumerator WaitForChanges()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (tiles.Count != grid.size) {
            CreateTile();
        }

        if (CheckForGameOver()) {
            gameManager.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if (tiles.Count != grid.size)
        {
            return false;
        }
        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile)) {
                return false;
            }
            
            if (down != null && CanMerge(tile, down.tile)) {
                return false;
            }
            
            if (left != null && CanMerge(tile, left.tile)) {
                return false;
            }
            
            if (right != null && CanMerge(tile, right.tile)) {
                return false;
            }
        }
        return true;
    }

    // 设置锤子状态
    public void SetHammerState(bool b) {
        hammerStete = b;

        if (hammerStete) { // 暂停格子合并和按键监听，高亮有数字的格子
            // 高亮格子
            StartCoroutine(HighlightTileCell());
        }
        else {
            foreach (var tile in tiles)
            {
                tile.StopScaleAnimation();
            }
        }
    }
    
    public IEnumerator HighlightTileCell()
    {
        while (waiting) {
            yield return new WaitForSeconds(0.1f);
        }

        foreach (var tile in tiles)
        {
            tile.RunScaleAnimation(Vector3.one * 1.2f);
        }
    }

    public void SetHammerHitting(bool b, Tile targetTile) {
        hammerHitting = b;
        if (hammerHitting) {
            foreach (var tile in tiles)
            {
                tile.StopScaleAnimation();
            }
            // 显示锤子移动，砸在cell上，消除cell
            GameManager.Instance.HammerHitCell(targetTile);
        } else {
            SetHammerState(false);
        }
    }

}
