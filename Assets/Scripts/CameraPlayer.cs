using Unity.Netcode;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    private Transform localPlayer; // Ссылка на локального игрока
    private Transform playerClone; // Ссылка на клон игрока
    public Vector3 offset; // Смещение камеры относительно игрока
    public float smoothSpeed = 0.125f; // Скорость сглаживания

    void Update()
    {
        // Проверяем, если клон игрока существует
        if (playerClone == null)
        {
            // Находим клон игрока в сцене
            GameObject playerObject = GameObject.FindWithTag("Player"); // Убедитесь, что у вашего префаба игрока установлен тег "Player"
            if (playerObject != null)
            {
                playerClone = playerObject.transform; // Сохраняем ссылку на трансформ клона
            }
        }
    }

    void LateUpdate()
    {
        // Проверяем, если локальный игрок существует
        if (localPlayer != null && localPlayer.GetComponent<NetworkObject>().IsLocalPlayer) // Проверяем, является ли это локальным игроком
        {
            FollowPlayer(localPlayer);
        }
        // Если локальный игрок не установлен, но есть клон, следуем за клоном
        else if (playerClone != null)
        {
            FollowPlayer(playerClone);
        }
    }

    private void FollowPlayer(Transform player)
    {
        // Определяем желаемую позицию камеры с учетом поворота игрока
        Vector3 desiredPosition = player.position + player.TransformDirection(offset);
        // Плавно перемещаем камеру к желаемой позиции
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Направляем камеру на игрока
        transform.LookAt(player);
    }

    public void SetLocalPlayer(Transform playerTransform)
    {
        localPlayer = playerTransform; // Устанавливаем ссылку на локального игрока
    }
}

