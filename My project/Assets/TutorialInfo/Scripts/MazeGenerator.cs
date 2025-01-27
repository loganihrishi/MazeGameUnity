using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]
    public int mazeWidth = 30, mazeHeight = 30; // default values

    public int startX = 0;
    public int startY = 0;

    MazeCell[,] maze;

    Vector2Int currentCell; // the maze cell we're currently looking at

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];

        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                maze[i, j] = new MazeCell(i, j);
            }
        }

        CarvePath(startX, startY);

        return maze;
    }

    List<Direction> directions = new List<Direction> {
        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    };

    List<Direction> getRandomDirections()
    {
        // Copy the list
        List<Direction> copy = new List<Direction>(directions);

        List<Direction> randomDir = new List<Direction>();

        while (copy.Count > 0)
        {
            int rnd = Random.Range(0, copy.Count);
            randomDir.Add(copy[rnd]);
            copy.RemoveAt(rnd);
        }

        return randomDir;
    }

    bool isValidCell(int x, int y)
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited) return false;
        return true;
    }

    Vector2Int CheckNeighbor()
    {
        List<Direction> rndDir = getRandomDirections();
        for (int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbor = currentCell;

            switch (rndDir[i])
            {
                case Direction.Up:
                    neighbor.y++;
                    break;
                case Direction.Down:
                    neighbor.y--;
                    break;
                case Direction.Right:
                    neighbor.x++;
                    break;
                case Direction.Left:
                    neighbor.x--;
                    break;
            }

            if (isValidCell(neighbor.x, neighbor.y)) return neighbor;
        }

        return currentCell;
    }

    void breakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if (primaryCell.x > secondaryCell.x)
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        }
        else if (primaryCell.x < secondaryCell.x)
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }
        else if (primaryCell.y < secondaryCell.y)
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        }
        else if (primaryCell.y > secondaryCell.y)
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }

    void CarvePath(int x, int y)
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds, defaulting to 0,0");
        }

        currentCell = new Vector2Int(x, y);

        List<Vector2Int> path = new List<Vector2Int>();

        bool deadEnd = false;

        while (!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbor();

            if (nextCell == currentCell)
            {
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i];
                    path.RemoveAt(i);

                    nextCell = CheckNeighbor();
                    if (nextCell != currentCell) break;
                }

                if (nextCell == currentCell) deadEnd = true;
            }
            else
            {
                breakWalls(currentCell, nextCell);
                maze[currentCell.x, currentCell.y].visited = true;
                currentCell = nextCell;
                path.Add(currentCell);
            }
        }
    }
}

public enum Direction
{
    Up, Down, Left, Right
}

public class MazeCell
{
    public bool visited; // determines if this cell has been visited
    public int x, y; // position of the cell
    public bool topWall, bottomWall, leftWall, rightWall; // walls of the cell

    public Vector2Int position
    {
        get { return new Vector2Int(x, y); }
    }

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.visited = false;
        topWall = bottomWall = leftWall = rightWall = true; // All walls start present
    }
}
