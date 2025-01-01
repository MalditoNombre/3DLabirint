using System.Collections.Generic;
using UnityEngine;

public class ManagerCamera : MonoBehaviour
{
    public GameObject cameraPrefab; // Префаб камеры
    private Dictionary<int, GameObject> playerCameras = new Dictionary<int, GameObject>(); // Словарь для хранения камер игроков

    public GameObject CreateCameraForPlayer(int playerId, Transform playerTransform)
    {
        Debug.Log($"Создание камеры для игрока с ID: {playerId}");

        if (cameraPrefab == null)
        {
            Debug.LogError("cameraPrefab не установлен!");
            return null;
        }

        if (!playerCameras.ContainsKey(playerId))
        {
            GameObject newCamera = Instantiate(cameraPrefab);

            CameraPlayer cameraPlayer = newCamera.GetComponent<CameraPlayer>();
            if (cameraPlayer == null)
            {
                Debug.LogError("Компонент CameraPlayer не найден на префабе камеры!");
                Destroy(newCamera);
                return null;
            }

            cameraPlayer.SetLocalPlayer(playerTransform);
            playerCameras.Add(playerId, newCamera);
            newCamera.SetActive(true);
            Debug.Log("Камера успешно создана и активирована.");

            return newCamera; // Возвращаем созданный объект камеры
        }
        else
        {
            Debug.Log($"Камера для игрока с ID {playerId} уже существует.");
            return playerCameras[playerId]; // Возвращаем уже существующую камеру
        }
    }

    public void RemoveCameraForPlayer(int playerId)
    {
        if (playerCameras.ContainsKey(playerId))
        {
            Destroy(playerCameras[playerId]); // Удаляем камеру игрока
            playerCameras.Remove(playerId); // Удаляем запись из словаря
            Debug.Log($"Камера для игрока с ID {playerId} была удалена.");
        }
        else
        {
            Debug.LogWarning($"Камера для игрока с ID {playerId} не найдена.");
        }
    }
}
