using Unity.Netcode;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // ������ ����� ������
    [SerializeField] private GameObject playerPrefab;

    private int nextSpawnIndex = 0; // ������ ��������� ����� ������
    private bool isSubscribed = false; // ���� ��� �������� �� �������

    public override void OnNetworkSpawn()
    {
        if (IsServer && !isSubscribed)
        {
            Debug.Log("�������� �� ������� OnClientConnectedCallback.");
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            isSubscribed = true;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log($"������ {clientId} ���������. ��������� ������������� ������.");

        // ���������, ���������� �� ��� ����� ��� ������� clientId
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId) &&
            NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null)
        {
            Debug.Log($"����� ��� ������� {clientId} ��� ����������.");
            return;
        }

        Debug.Log($"����� ������ ��� ������� {clientId}.");
        SpawnPlayers(clientId);
    }

    private void SpawnPlayers(ulong clientId)
    {
        // ���������, ���� �� ��������� ����� ������
        if (nextSpawnIndex < spawnPoints.Length)
        {
            // �������� ����� ������
            Transform spawnPoint = spawnPoints[nextSpawnIndex];
            Debug.Log($"����� ������ {clientId} �� ����� {spawnPoint.position}");

            nextSpawnIndex++; // ����������� ������ ��� ���������� ������

            // ������� ������
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); // ������� ������ � ����
        }
        else
        {
            Debug.Log("��� ��������� ����� ������!");
            // ����� �������, ���� ��� ����� ������
            nextSpawnIndex = 0; // ����� �������, ���� ��� ����� ������
        }
    }
}


