using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;

    public int mineCount = 32;

    private Board board;
    private Cell[,] state;

    private bool gameover;

    void OnValidate()
    {
        mineCount = Mathf.Clamp(mineCount, 0, width*height);
    }

    void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        state = new Cell[width, height];
        gameover = false;

        GenerateCells();
        GenerateMineCells();
        GenerateNumbers();

        Camera.main.transform.position = new Vector3(width/2f, height/2f, -10f);

        board.Draw(state);
    }

    private void GenerateCells()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x,y] = cell;
            }
        }
    }

    private void GenerateMineCells()
    {
        List<int> nums = Shuffle(width, height, mineCount);
        for (int i=0; i<mineCount; i++)
        {
            int index = nums[i];
            int x = index/width;
            int y = index%height;

            state[x, y].type = Cell.Type.Mine;
            
            // Debug.LogFormat("GenerateMineCells [{0}, {1}]", x, y);
        }
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine) {
                    continue;
                }

                cell.number = CountMines(x, y);

                if (cell.number > 0) {
                    cell.type = Cell.Type.Number;
                }

                state[x, y] = cell;
            }
        }
    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;
        for (int i=-1; i<=1; i++)
        {
            for (int j=-1; j<=1; j++)
            {
                if (i==0 && j==0) {
                    continue;
                }
                int x = cellX+i;
                int y = cellY+j;
                if (!ValidCellPosition(x, y)) {
                    continue;
                }
                if (state[x, y].type == Cell.Type.Mine) {
                    count++;
                }
            }
        }
        return count;
    }

    private bool ValidCellPosition(int cellX, int cellY)
    {
        return cellX>=0 && cellX<width && cellY>=0 && cellY<height;
    }

    private List<int> Shuffle(int width, int height, int count) {
        int len = width*height;
        List<int> nums = new List<int>(len);
        Debug.Log("Shuffle nums.Count=" + nums.Count + ", len=" + len);
        // 初始化
        for (int i=0; i<len; i++)
        {
            nums.Add(i);
        }
        Debug.Log("Before Shuffle nums=" + GetListString(nums));
        // 洗牌
        for (int i=0;i<count;i++) {
            int index = UnityEngine.Random.Range(i+1, len);
            // Debug.Log("Before Shuffle random index=" + index);
            int temp = nums[index];
            nums[index] = nums[i];
            nums[i] = temp;
        }
        // 输出
        Debug.Log("After Shuffle nums=" + GetListString(nums));

        return nums;
    }

    private string GetListString(List<int> nums) {
        string str = "[";
        for (int i=0; i<nums.Count; i++) {
            if (i!=nums.Count-1) {
                str = str + nums[i] + ",";
            }
            else {
                str = str + nums[i];
            }
        }
        str += "]";
        return str;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
            return;
        }
        if (gameover)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1)) {
            Flag();
        }
        else if (Input.GetMouseButtonDown(0)) {
            Reveal();
        }
    }

    private void Flag()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);

        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed) {
            return;
        }
        cell.flagged = !cell.flagged;
        state[cellPosition.x, cellPosition.y] = cell;

        board.Draw(state);
    }

    private void Reveal()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);

        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged) {
            return;
        }

        switch (cell.type)
        {
            case Cell.Type.Mine:
                Explode(cell);
                break;
            case Cell.Type.Empty:
                Flood(cell);
                CheckWinCondition();
                break;
            default:
                cell.revealed = true;
                state[cell.position.x, cell.position.y] = cell;
                CheckWinCondition();
                break;
        }

        board.Draw(state);
    }

    private void Flood(Cell cell)
    {
        if (cell.revealed) return;
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) return;

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if (cell.type == Cell.Type.Empty) {
            Flood(GetCell(cell.position.x-1, cell.position.y));
            Flood(GetCell(cell.position.x+1, cell.position.y));
            Flood(GetCell(cell.position.x, cell.position.y-1));
            Flood(GetCell(cell.position.x, cell.position.y+1));
        }
    }

    private void Explode(Cell cell) {
        Debug.Log("Game Over!");
        gameover = true;

        cell.exploded = true;
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        // 显示所有炸弹
        for (int x=0; x<width; x++)
        {
            for (int y=0; y<height; y++)
            {
                Cell otherCell = state[x, y];
                if (otherCell.type == Cell.Type.Mine) {
                    otherCell.revealed = true;
                    state[x, y] = otherCell;
                }
            }
        }
    }

    private bool CheckWinCondition()
    {
        for (int x=0; x<width; x++)
        {
            for (int y=0; y<height; y++)
            {
                Cell cell = state[x, y];
                if (cell.type != Cell.Type.Mine && !cell.revealed) {
                    return false;
                }
            }
        }

        for (int x=0; x<width; x++)
        {
            for (int y=0; y<height; y++)
            {
                Cell cell = state[x, y];
                if (cell.type == Cell.Type.Mine) {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }

        Debug.Log("Winner!");
        gameover = true;
        return true;
    }

    private Cell GetCell(int x, int y)
    {
        if (ValidCellPosition(x, y)) {
            return state[x, y];
        }
        else {
            return new Cell();
        }
    }
}
