using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnGold : MonoBehaviour
{
    public GameObject coinPrefab; // ������ �������
    public float spawnDelay = 5f; // �������� ����� ������� ����� ������
    public List<Transform> spawnPoints; // ������ ����� ������
    public Text scoreText; // UI ������� ��� ����������� �����

    private GameObject currentCoin; // ������� ������
    private int playerScore = 0; // ���� ������

    void Start()
    {
        SpawnCoin(); // ������� ������ ������
        UpdateScoreText(); // ��������� ����� ����� ��� ������
    }

    void SpawnCoin()
    {
        // ���������, ���� �� ��������� ����� ������
        if (spawnPoints.Count > 0)
        {
            // �������� ��������� ����� ������
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            // ������� ������ � ��������� �����
            currentCoin = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void CollectCoin()
    {
        // ����������, ����� ����� �������� ������
        if (currentCoin != null)
        {
            Destroy(currentCoin); // ���������� ������� ������
            playerScore += 5; // ����������� ���� ������
            UpdateScoreText(); // ��������� ����� �����
            StartCoroutine(SpawnCoinAfterDelay()); // ��������� �������� ��� ������ ����� ������

            // ���������, ������ �� ����� 100 �����
            if (playerScore >= 100)
            {
                RestartGame(); // ������������� ����
            }
        }
    }

    private IEnumerator SpawnCoinAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay); // ���� �������� �����
        SpawnCoin(); // ������� ����� ������
    }

    private void RestartGame()
    {
        // ������������ ������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "����: " + playerScore; // ��������� ����� �� UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ���������, ��� � ������ ������ ���� ��� "Player"
        {
            CollectCoin(); // �������� ����� ����� ������
        }
    }
}
