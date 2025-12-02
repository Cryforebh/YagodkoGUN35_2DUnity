using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private float MaxViewDistance = 7f; // Максимальная дистанция обзора
    [SerializeField] private Transform _playerPosition; // Ссылка на объект игрока
    [SerializeField] private SpriteRenderer _iconEay; // Иконка для отображения состояния видимости
    [SerializeField] private SpriteRenderer _iconRadius;
    private Color _color = Color.red; // Цвет, когда игрок виден
    private Color _defoltColor; // Исходный цвет иконки
    private Vector2 _enemyPosition; // Позиция врага
    private bool _isView = false; // Флаг видимости игрока
    private bool _isWarning = false; // Флаг предупреждения
    private Vector2 _viewVector; // Вектор обзора врага

    private void Awake()
    {
        _enemyPosition = transform.position; // Получаем позицию врага из компонента Transform
        _defoltColor = _iconEay.color; // Сохраняем исходный цвет иконки
        _iconRadius.transform.localScale = new Vector3(MaxViewDistance * 2, MaxViewDistance * 2);
    }

    private void Update()
    {
        // Обновляем позицию врага на каждом кадре
        _enemyPosition = transform.position;

        // Получаем позицию игрока
        Vector2 playerPosition = _playerPosition.position;

        // Вычисляем вектор от врага к игроку
        Vector2 directionToPlayer = playerPosition - _enemyPosition;

        // Определяем направление взгляда врага
        _viewVector = transform.eulerAngles.y < 180 ? new Vector2(1, 0) : new Vector2(-1, 0);

        // Вычисляем скалярное произведение вектора обзора и вектора к игроку
        float result = Vector2.Dot(_viewVector, directionToPlayer);

        // Проверяем, находится ли игрок в зоне видимости
        _isView = result > 0 && directionToPlayer.magnitude < MaxViewDistance;

        // Если активен режим предупреждения и игрок близко, считаем его видимым
        if (_isWarning && directionToPlayer.magnitude < MaxViewDistance) _isView = true;
        else _isWarning = false;

        // Меняем цвет иконки в зависимости от видимости игрока
        if (_isView)
        {
            _iconEay.color = _color;
            _isWarning = true;

            // Поворачиваем врага в сторону игрока
            transform.eulerAngles = directionToPlayer.x > 0
                ? new Vector3(0, 0, 0) // Смотрим вправо
                : new Vector3(0, 180, 0); // Смотрим влево
        }
        else
        {
            _iconEay.color = _defoltColor;
        }
    }
}
