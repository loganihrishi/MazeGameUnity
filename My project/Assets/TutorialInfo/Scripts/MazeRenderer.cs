using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private GameObject MazeCellPrefab;

    public float CellSize = 1f;

    private void Start()
    {
        // Debugging: Check mazeGenerator
        if (mazeGenerator == null)
        {
            Debug.LogError("MazeGenerator is not assigned in MazeRenderer!");
            return;
        }

        // Debugging: Check MazeCellPrefab
        if (MazeCellPrefab == null)
        {
            Debug.LogError("MazeCellPrefab is not assigned in MazeRenderer!");
            return;
        }

        // Get the maze from the generator
        MazeCell[,] maze = mazeGenerator.GetMaze();
        if (maze == null)
        {
            Debug.LogError("MazeGenerator.GetMaze() returned null!");
            return;
        }

        // Loop through every cell in the maze
        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                if (maze[x, y] == null)
                {
                    Debug.LogError($"Maze cell at ({x}, {y}) is null!");
                    continue;
                }

                // Instantiate a new MazeCellPrefab for each cell
                GameObject newCell = Instantiate(
                    MazeCellPrefab,
                    new Vector3(x * CellSize, 0f, y * CellSize),
                    Quaternion.identity,
                    transform
                );

                // Get the MazeCellObject script attached to the prefab
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();
                if (mazeCell == null)
                {
                    Debug.LogError("MazeCellPrefab does not have a MazeCellObject component!");
                    continue;
                }

                // Get the wall data from the maze
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                bool right = false;
                bool bottom = false; 

                if (x == mazeGenerator.mazeWidth-1) right = true; 
                if (y==0) bottom = true; 

                // Initialize the cell with the wall data
                mazeCell.Init(top, bottom, right, left);
            }
        }
    }
}
