# Trial Assignment (Creobit)
На разработку данного проекта ушло 4 дня (28.07.24-31.07.24).
- Первые два дня - разработка системы загрузки ассетов.
- Третий день - создание 2 игры и систему получения/сохранения данных из игр.
- Четвертый день - доработка игр и функционала систем (включая рефакторинг).

Кликер был создан в первый день за 5-10 минут (не считая систему получения данных).
На все дни уходило максимум примерно по 6-8 часов.

Сцена лаунчера не пропадает при запуске игры, а весь её основной интерфейс попросту скрывается. Выход из игр происходит по специальной кнопке слева сверху, которая выгружает сцену игры и включает интерфейс лаунчера.

В игре Adventure бродилка получилась больше похожей на платформер. Но геймплей добраться от начала до финиша был реализован. Управление - WASD для движения и пробел на прыжок.

Пока вместо Dependency Injection а-ля Zenject был использован обычный Singleton, но можно заменить позднее.

Из импортированных сторонних ассетов: 3D-модель персонажа и библиотека проверки свободного места.
