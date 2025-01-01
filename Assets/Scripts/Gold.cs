using UnityEngine;

public class Gold : MonoBehaviour
{
    public int pointValue = 5; // Количество очков за монетку
    private SpawnGold coinSpawner; // Ссылка на CoinSpawner

    private void Start()
    {
        // Получаем ссылку на CoinSpawner
        coinSpawner = FindFirstObjectByType<SpawnGold>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что игрок собрал монетку
        if (other.CompareTag("Player"))
        {
            // Получаем компонент игрока и добавляем очки
            PlayerController playerScore = other.GetComponent<PlayerController>();
            if (playerScore != null)
            {
                playerScore.AddScore(pointValue);
            }

            // Сообщаем CoinSpawner, что монета собрана
            if (coinSpawner != null)
            {
                coinSpawner.CollectCoin();
            }

            // Уничтожаем монету, если не хотите, чтобы она оставалась в сцене
            Destroy(gameObject);
        }
    }
}
