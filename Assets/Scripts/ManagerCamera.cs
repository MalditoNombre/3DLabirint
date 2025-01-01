using System.Collections.Generic;
using UnityEngine;

public class ManagerCamera : MonoBehaviour
{
    public GameObject cameraPrefab; // ������ ������
    private Dictionary<int, GameObject> playerCameras = new Dictionary<int, GameObject>(); // ������� ��� �������� ����� �������

    public GameObject CreateCameraForPlayer(int playerId, Transform playerTransform)
    {
        Debug.Log($"�������� ������ ��� ������ � ID: {playerId}");

        if (cameraPrefab == null)
        {
            Debug.LogError("cameraPrefab �� ����������!");
            return null;
        }

        if (!playerCameras.ContainsKey(playerId))
        {
            GameObject newCamera = Instantiate(cameraPrefab);

            CameraPlayer cameraPlayer = newCamera.GetComponent<CameraPlayer>();
            if (cameraPlayer == null)
            {
                Debug.LogError("��������� CameraPlayer �� ������ �� ������� ������!");
                Destroy(newCamera);
                return null;
            }

            cameraPlayer.SetLocalPlayer(playerTransform);
            playerCameras.Add(playerId, newCamera);
            newCamera.SetActive(true);
            Debug.Log("������ ������� ������� � ������������.");

            return newCamera; // ���������� ��������� ������ ������
        }
        else
        {
            Debug.Log($"������ ��� ������ � ID {playerId} ��� ����������.");
            return playerCameras[playerId]; // ���������� ��� ������������ ������
        }
    }

    public void RemoveCameraForPlayer(int playerId)
    {
        if (playerCameras.ContainsKey(playerId))
        {
            Destroy(playerCameras[playerId]); // ������� ������ ������
            playerCameras.Remove(playerId); // ������� ������ �� �������
            Debug.Log($"������ ��� ������ � ID {playerId} ���� �������.");
        }
        else
        {
            Debug.LogWarning($"������ ��� ������ � ID {playerId} �� �������.");
        }
    }
}
