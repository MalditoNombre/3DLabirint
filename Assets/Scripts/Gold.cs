using UnityEngine;

public class Gold : MonoBehaviour
{
    public int pointValue = 5; // ���������� ����� �� �������
    private SpawnGold coinSpawner; // ������ �� CoinSpawner

    private void Start()
    {
        // �������� ������ �� CoinSpawner
        coinSpawner = FindFirstObjectByType<SpawnGold>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ����� ������ �������
        if (other.CompareTag("Player"))
        {
            // �������� ��������� ������ � ��������� ����
            PlayerController playerScore = other.GetComponent<PlayerController>();
            if (playerScore != null)
            {
                playerScore.AddScore(pointValue);
            }

            // �������� CoinSpawner, ��� ������ �������
            if (coinSpawner != null)
            {
                coinSpawner.CollectCoin();
            }

            // ���������� ������, ���� �� ������, ����� ��� ���������� � �����
            Destroy(gameObject);
        }
    }
}
