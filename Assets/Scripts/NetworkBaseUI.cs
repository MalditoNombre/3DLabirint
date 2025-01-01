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
    [SerializeField] private InputField _ipInputField; // ���� ��� ����� IP-������
    public GameObject cameraPrefab; // ������ ������

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} ���������.");
        // �������� ������ ��� ������ �������
        CreateCameraForPlayer((int)clientId);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} ��������.");
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
        // �������� ������ ��� ���������� ������
        CreateCameraForPlayer(0); // ������������, ��� ID ���������� ������ - 0
    }

    private void onStartClient()
    {
        string ipAddress = _ipInputField.text; // �������� IP-����� �� ���������� ����
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(ipAddress, 7777); // ������������� IP-����� � ����
        NetworkManager.Singleton.StartClient();
    }

    private void onStartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    private void CreateCameraForPlayer(int playerId)
    {
        // �������� ��������� ������ �� ��� ID
        Transform playerTransform = GetPlayerTransformById(playerId);
        if (playerTransform != null)
        {
            // ������� ������ ��� ������
            GameObject newCamera = Instantiate(cameraPrefab);
            newCamera.transform.position = playerTransform.position + new Vector3(0, 1.5f, -3); // ������ ��������
            newCamera.transform.LookAt(playerTransform); // ���������� ������ �� ������
            newCamera.SetActive(true);
            Debug.Log($"������ ������� ������� ��� ������ � ID: {playerId}");
        }
        else
        {
            Debug.LogWarning($"�� ������� �������� ��������� ��� ������ � ID: {playerId}");
        }
    }

    private Transform GetPlayerTransformById(int playerId)
    {
        // �������� NetworkObject ��� ������ � ��������� ID
        var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject((ulong)playerId);

        // ���������, ��� NetworkObject �� ����� null
        if (playerNetworkObject != null)
        {
            // ���������� ��������� ������
            return playerNetworkObject.transform;
        }

        // ���� NetworkObject �� ������, ���������� null
        return null;
    }
}

