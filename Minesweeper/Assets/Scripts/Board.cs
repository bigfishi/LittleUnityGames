using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }

    public Tile tileUnknown;
    public Tile tileEmpty;
    public Tile tileFlag;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile[] tileNumbers;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int x=0;x<width;x++) {
            for (int y=0;y<height;y++) {
                Cell cell = state[x,y];
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
    }

    private Tile GetTile(Cell cell)
    {
        if (cell.revealed) {
            return GetRevealedTile(cell);
        } else if (cell.flagged) {
            return tileFlag;
        } else {
            return tileUnknown;
        }
    }

    private Tile GetRevealedTile(Cell cell) {
        switch (cell.type) {
            case Cell.Type.Empty: return tileEmpty;
            case Cell.Type.Mine: return cell.exploded ? tileExploded : tileMine;
            case Cell.Type.Number: return GetNumberTile(cell);
            default: return null;
        }
    }

    private Tile GetNumberTile(Cell cell) {
        int numIndex = cell.number -1;
        return tileNumbers[numIndex];
    }
}
