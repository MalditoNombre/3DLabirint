using System.Collections.Generic;
using UnityEngine;

public class LabirintScript : MonoBehaviour
{
    public int width = 10; // Ширина лабиринта
    public int height = 10; // Высота лабиринта
    public GameObject wallPrefab; // Префаб стены

    private int[,] maze;

    void Start()
    {
        GenerateMaze();
        DrawMaze();
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        // Инициализация массива
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1; // 1 - стена
            }
        }

        // Начальная точка
        maze[1, 1] = 0; // 0 - проход
        CarvePath(1, 1);

        // Создание проходов в углах
        CreateCornerPassages();
    }

    void CarvePath(int x, int y)
    {
        // Направления движения
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(2, 0), // Вправо
            new Vector2Int(-2, 0), // Влево
            new Vector2Int(0, 2), // Вверх
            new Vector2Int(0, -2) // Вниз
        };

        // Перемешивание направлений
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
                maze[newX, newY] = 0; // Создаем проход
                maze[x + direction.x / 2, y + direction.y / 2] = 0; // Убираем стену между

                CarvePath(newX, newY); // Рекурсивный вызов
            }
        }
    }

    void CreateCornerPassages()
    {
        // Проходы в углах
        maze[0, 1] = 0; // Левый верхний угол
        maze[1, 0] = 0; // Левый нижний угол
        maze[width - 1, 1] = 0; // Правый верхний угол
        maze[width - 2, 0] = 0; // Правый нижний угол
        maze[0, height - 2] = 0; // Левый нижний угол
        maze[1, height - 1] = 0; // Левый верхний угол
        maze[width - 1, height - 2] = 0; // Правый нижний угол
        maze[width - 2, height - 1] = 0; // Правый нижний угол

        // Проходы к центру
        maze[1, 1] = 0; // Проход из левого верхнего угла
        maze[1, height - 2] = 0; // Проход из левого нижнего угла
        maze[width - 2, 1] = 0; // Проход из правого верхнего угла
        maze[width - 2, height - 2] = 0; // Проход из правого нижнего угла
    }

    void DrawMaze()
    {
        // Создание стен по периметру
        for (int x = 0; x < width; x++)
        {
            // Нижняя стена
            Instantiate(wallPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            // Верхняя стена
            Instantiate(wallPrefab, new Vector3(x, 0, height - 1), Quaternion.identity);
        }

        for (int y = 0; y < height; y++)
        {
            // Левая стена
            Instantiate(wallPrefab, new Vector3(0, 0, y), Quaternion.identity);
            // Правая стена
            Instantiate(wallPrefab, new Vector3(width - 1, 0, y), Quaternion.identity);
        }

        // Создание стен лабиринта
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity); // Стена лабиринта
                }
            }
        }
    }
} 



