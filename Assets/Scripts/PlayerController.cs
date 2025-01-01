using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


    public class PlayerController : NetworkBehaviour
    {
        public float moveSpeed = 5f; // Базовая скорость движения
        public float maxSpeed = 10f; // Максимальная скорость
        public float accelerationTime = 3.5f; // Время для достижения максимальной скорости
        public float deceleration = 5f; // Замедление
        public float backwardSpeed = 2f; // Скорость заднего хода
        public float score = 0f; // Очки игрока
        public Text scoreText; // UI элемент для отображения счета

        private float currentSpeed = 0f; // Текущая скорость
        private Vector3 movement; // Вектор движения

        void Update()
        {
            // Проверяем, является ли этот объект локальным игроком
            if (!IsLocalPlayer) return;

            // Получение ввода от пользователя
            float moveHorizontal = Input.GetAxis("Horizontal"); // A и D
            float moveVertical = Input.GetAxis("Vertical"); // W и S

            // Обработка движения вперед
            if (moveVertical > 0)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += (maxSpeed / accelerationTime) * Time.deltaTime;
                }
            }
            // Обработка движения назад
            else if (moveVertical < 0)
            {
                // Устанавливаем скорость заднего хода
                currentSpeed = backwardSpeed;
            }
            else
            {
                // Плавное замедление, если не нажаты W или S
                currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
            }

            // Движение вперед и назад
            movement = transform.forward * currentSpeed * Time.deltaTime;

            // Если игрок движется назад, инвертируем направление
            if (moveVertical < 0)
            {
                movement = -transform.forward * backwardSpeed * Time.deltaTime; // Движение назад
            }

            transform.position += movement;

            // Поворот влево и вправо с помощью A и D
            if (moveHorizontal != 0)
            {
                transform.Rotate(0, moveHorizontal * 100f * Time.deltaTime, 0);
            }
        }

        public void AddScore(int points)
        {
            score += points; // Добавляем очки
            UpdateScoreText(); // Обновляем текст счета
        }

        private void UpdateScoreText()
        {
            if (scoreText != null)
            {
                scoreText.text = "Очки: " + score; // Обновляем текст на UI
            }
        }
    }
