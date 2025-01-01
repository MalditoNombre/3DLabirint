using System.Collections.Generic;
using UnityEngine;

public class LabirintScript : MonoBehaviour
{
    public int width = 10; // ������ ���������
    public int height = 10; // ������ ���������
    public GameObject wallPrefab; // ������ �����

    private int[,] maze;

    void Start()
    {
        GenerateMaze();
        DrawMaze();
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        // ������������� �������
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1; // 1 - �����
            }
        }

        // ��������� �����
        maze[1, 1] = 0; // 0 - ������
        CarvePath(1, 1);

        // �������� �������� � �����
        CreateCornerPassages();
    }

    void CarvePath(int x, int y)
    {
        // ����������� ��������
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(2, 0), // ������
            new Vector2Int(-2, 0), // �����
            new Vector2Int(0, 2), // �����
            new Vector2Int(0, -2) // ����
        };

        // ������������� �����������
        for (int i = 0; i < directions.Count; i++)
        {
            Vector2Int temp = directions[i];
            int randomIndex = Random.Range(i, directions.Count);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        foreach (var direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (newX > 0 && newX < width && newY > 0 && newY < height && maze[newX, newY] == 1)
            {
                maze[newX, newY] = 0; // ������� ������
                maze[x + direction.x / 2, y + direction.y / 2] = 0; // ������� ����� �����

                CarvePath(newX, newY); // ����������� �����
            }
        }
    }

    void CreateCornerPassages()
    {
        // ������� � �����
        maze[0, 1] = 0; // ����� ������� ����
        maze[1, 0] = 0; // ����� ������ ����
        maze[width - 1, 1] = 0; // ������ ������� ����
        maze[width - 2, 0] = 0; // ������ ������ ����
        maze[0, height - 2] = 0; // ����� ������ ����
        maze[1, height - 1] = 0; // ����� ������� ����
        maze[width - 1, height - 2] = 0; // ������ ������ ����
        maze[width - 2, height - 1] = 0; // ������ ������ ����

        // ������� � ������
        maze[1, 1] = 0; // ������ �� ������ �������� ����
        maze[1, height - 2] = 0; // ������ �� ������ ������� ����
        maze[width - 2, 1] = 0; // ������ �� ������� �������� ����
        maze[width - 2, height - 2] = 0; // ������ �� ������� ������� ����
    }

    void DrawMaze()
    {
        // �������� ���� �� ���������
        for (int x = 0; x < width; x++)
        {
            // ������ �����
            Instantiate(wallPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            // ������� �����
            Instantiate(wallPrefab, new Vector3(x, 0, height - 1), Quaternion.identity);
        }

        for (int y = 0; y < height; y++)
        {
            // ����� �����
            Instantiate(wallPrefab, new Vector3(0, 0, y), Quaternion.identity);
            // ������ �����
            Instantiate(wallPrefab, new Vector3(width - 1, 0, y), Quaternion.identity);
        }

        // �������� ���� ���������
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity); // ����� ���������
                }
            }
        }
    }
} 



