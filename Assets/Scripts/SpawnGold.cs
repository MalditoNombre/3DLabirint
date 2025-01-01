using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnGold : MonoBehaviour
{
    public GameObject coinPrefab; // Префаб монетки
    public float spawnDelay = 5f; // Задержка перед спавном новой монеты
    public List<Transform> spawnPoints; // Список точек спавна
    public Text scoreText; // UI элемент для отображения счета

    private GameObject currentCoin; // Текущая монета
    private int playerScore = 0; // Очки игрока

    void Start()
    {
        SpawnCoin(); // Спавним первую монету
        UpdateScoreText(); // Обновляем текст счета при старте
    }

    void SpawnCoin()
    {
        // Проверяем, есть ли доступные точки спавна
        if (spawnPoints.Count > 0)
        {
            // Выбираем случайную точку спавна
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Спавним монету в выбранной точке
            currentCoin = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void CollectCoin()
    {
        // Вызывается, когда игрок собирает монету
        if (currentCoin != null)
        {
            Destroy(currentCoin); // Уничтожаем текущую монету
            playerScore += 5; // Увеличиваем очки игрока
            UpdateScoreText(); // Обновляем текст счета
            StartCoroutine(SpawnCoinAfterDelay()); // Запускаем корутину для спавна новой монеты

            // Проверяем, достиг ли игрок 100 очков
            if (playerScore >= 100)
            {
                RestartGame(); // Перезагружаем игру
            }
        }
    }

    private IEnumerator SpawnCoinAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay); // Ждем заданное время
        SpawnCoin(); // Спавним новую монету
    }

    private void RestartGame()
    {
        // Перезагрузка текущей сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + playerScore; // Обновляем текст на UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Убедитесь, что у вашего игрока есть тег "Player"
        {
            CollectCoin(); // Вызываем метод сбора монеты
        }
    }
}
