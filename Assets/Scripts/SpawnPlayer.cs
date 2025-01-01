using Unity.Netcode;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // Массив точек спавна
    [SerializeField] private GameObject playerPrefab;

    private int nextSpawnIndex = 0; // Индекс следующей точки спавна
    private bool isSubscribed = false; // Флаг для подписки на события

    public override void OnNetworkSpawn()
    {
        if (IsServer && !isSubscribed)
        {
            Debug.Log("Подписка на событие OnClientConnectedCallback.");
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            isSubscribed = true;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log($"Клиент {clientId} подключен. Проверяем существование игрока.");

        // Проверяем, существует ли уже игрок для данного clientId
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId) &&
            NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null)
        {
            Debug.Log($"Игрок для клиента {clientId} уже существует.");
            return;
        }

        Debug.Log($"Спавн игрока для клиента {clientId}.");
        SpawnPlayers(clientId);
    }

    private void SpawnPlayers(ulong clientId)
    {
        // Проверяем, есть ли доступные точки спавна
        if (nextSpawnIndex < spawnPoints.Length)
        {
            // Получаем точку спавна
            Transform spawnPoint = spawnPoints[nextSpawnIndex];
            Debug.Log($"Спавн игрока {clientId} на точке {spawnPoint.position}");

            nextSpawnIndex++; // Увеличиваем индекс для следующего игрока

            // Создаем игрока
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); // Спавним игрока в сети
        }
        else
        {
            Debug.Log("Нет доступных точек спавна!");
            // Сброс индекса, если все точки заняты
            nextSpawnIndex = 0; // Сброс индекса, если все точки заняты
        }
    }
}


