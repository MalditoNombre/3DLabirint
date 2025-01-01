using Unity.Netcode;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    private Transform localPlayer; // ������ �� ���������� ������
    private Transform playerClone; // ������ �� ���� ������
    public Vector3 offset; // �������� ������ ������������ ������
    public float smoothSpeed = 0.125f; // �������� �����������

    void Update()
    {
        // ���������, ���� ���� ������ ����������
        if (playerClone == null)
        {
            // ������� ���� ������ � �����
            GameObject playerObject = GameObject.FindWithTag("Player"); // ���������, ��� � ������ ������� ������ ���������� ��� "Player"
            if (playerObject != null)
            {
                playerClone = playerObject.transform; // ��������� ������ �� ��������� �����
            }
        }
    }

    void LateUpdate()
    {
        // ���������, ���� ��������� ����� ����������
        if (localPlayer != null && localPlayer.GetComponent<NetworkObject>().IsLocalPlayer) // ���������, �������� �� ��� ��������� �������
        {
            FollowPlayer(localPlayer);
        }
        // ���� ��������� ����� �� ����������, �� ���� ����, ������� �� ������
        else if (playerClone != null)
        {
            FollowPlayer(playerClone);
        }
    }

    private void FollowPlayer(Transform player)
    {
        // ���������� �������� ������� ������ � ������ �������� ������
        Vector3 desiredPosition = player.position + player.TransformDirection(offset);
        // ������ ���������� ������ � �������� �������
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ���������� ������ �� ������
        transform.LookAt(player);
    }

    public void SetLocalPlayer(Transform playerTransform)
    {
        localPlayer = playerTransform; // ������������� ������ �� ���������� ������
    }
}

