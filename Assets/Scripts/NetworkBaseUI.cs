using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class NetworkBaseUI : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Button _serverButton;
    [SerializeField] private InputField _ipInputField; // Поле для ввода IP-адреса
    public GameObject cameraPrefab; // Префаб камеры

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} подключен.");
        // Создание камеры для нового клиента
        CreateCameraForPlayer((int)clientId);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} отключен.");
    }

    private void Awake()
    {
        _hostButton.onClick.AddListener(onStartHost);
        _clientButton.onClick.AddListener(onStartClient);
        _serverButton.onClick.AddListener(onStartServer);
    }

    private void onStartHost()
    {
        NetworkManager.Singleton.StartHost();
        // Создание камеры для локального игрока
        CreateCameraForPlayer(0); // Предполагаем, что ID локального игрока - 0
    }

    private void onStartClient()
    {
        string ipAddress = _ipInputField.text; // Получаем IP-адрес из текстового поля
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(ipAddress, 7777); // Устанавливаем IP-адрес и порт
        NetworkManager.Singleton.StartClient();
    }

    private void onStartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    private void CreateCameraForPlayer(int playerId)
    {
        // Получаем трансформ игрока по его ID
        Transform playerTransform = GetPlayerTransformById(playerId);
        if (playerTransform != null)
        {
            // Создаем камеру для игрока
            GameObject newCamera = Instantiate(cameraPrefab);
            newCamera.transform.position = playerTransform.position + new Vector3(0, 1.5f, -3); // Пример смещения
            newCamera.transform.LookAt(playerTransform); // Направляем камеру на игрока
            newCamera.SetActive(true);
            Debug.Log($"Камера успешно создана для игрока с ID: {playerId}");
        }
        else
        {
            Debug.LogWarning($"Не удалось получить трансформ для игрока с ID: {playerId}");
        }
    }

    private Transform GetPlayerTransformById(int playerId)
    {
        // Получаем NetworkObject для игрока с указанным ID
        var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject((ulong)playerId);

        // Проверяем, что NetworkObject не равен null
        if (playerNetworkObject != null)
        {
            // Возвращаем трансформ игрока
            return playerNetworkObject.transform;
        }

        // Если NetworkObject не найден, возвращаем null
        return null;
    }
}

