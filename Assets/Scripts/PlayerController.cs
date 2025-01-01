using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


    public class PlayerController : NetworkBehaviour
    {
        public float moveSpeed = 5f; // ������� �������� ��������
        public float maxSpeed = 10f; // ������������ ��������
        public float accelerationTime = 3.5f; // ����� ��� ���������� ������������ ��������
        public float deceleration = 5f; // ����������
        public float backwardSpeed = 2f; // �������� ������� ����
        public float score = 0f; // ���� ������
        public Text scoreText; // UI ������� ��� ����������� �����

        private float currentSpeed = 0f; // ������� ��������
        private Vector3 movement; // ������ ��������

        void Update()
        {
            // ���������, �������� �� ���� ������ ��������� �������
            if (!IsLocalPlayer) return;

            // ��������� ����� �� ������������
            float moveHorizontal = Input.GetAxis("Horizontal"); // A � D
            float moveVertical = Input.GetAxis("Vertical"); // W � S

            // ��������� �������� ������
            if (moveVertical > 0)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += (maxSpeed / accelerationTime) * Time.deltaTime;
                }
            }
            // ��������� �������� �����
            else if (moveVertical < 0)
            {
                // ������������� �������� ������� ����
                currentSpeed = backwardSpeed;
            }
            else
            {
                // ������� ����������, ���� �� ������ W ��� S
                currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
            }

            // �������� ������ � �����
            movement = transform.forward * currentSpeed * Time.deltaTime;

            // ���� ����� �������� �����, ����������� �����������
            if (moveVertical < 0)
            {
                movement = -transform.forward * backwardSpeed * Time.deltaTime; // �������� �����
            }

            transform.position += movement;

            // ������� ����� � ������ � ������� A � D
            if (moveHorizontal != 0)
            {
                transform.Rotate(0, moveHorizontal * 100f * Time.deltaTime, 0);
            }
        }

        public void AddScore(int points)
        {
            score += points; // ��������� ����
            UpdateScoreText(); // ��������� ����� �����
        }

        private void UpdateScoreText()
        {
            if (scoreText != null)
            {
                scoreText.text = "����: " + score; // ��������� ����� �� UI
            }
        }
    }
