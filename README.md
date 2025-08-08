# WebWorkerService

## Описание

Проект представляет из себя реализацию шаблона веб-работ

## Зависимости

UI - Blazor App  
Job - Quartz

## Использование

### Пользователь

Открывается главная страница, нажимаются нужные кнопки.
Также возможен одиночный запуск задачи. При изменении конфига нужно
перегенирировать расписание (нажимается на кнопку, без перезапуска приложения)

Настройки которые требуют перестроить расписание (Enabled, VisibleOneStart, Cron)

### Разработка

Регистрируем задачи в DI
```csharp
builder.AddWorkers();
```
Запускаем задачи перед стартом приложения
```csharp
await app.RunWorkersAsync();
```

### Реализация задач

Каждая задача должна наследоваться от абстрактного класса.
Реализует абстрактный метод RunAsync.  

Также имеет интеграцию с DI.

```csharp
    public class TestJob : BaseConcurrentCancelJob
    {
        private readonly TestService _service;

        public TestJob(TestService service)
        {
            _service = service;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            // Работа
        }
    }
```

Описание абстрактных классов:  

- __BaseJob__  
Базовая реализация задачи.

- __BaseSingleJob__  
Реализация сингл задачи. Повторяет базовую реализацию.

- __BaseConcurrentJob__  
Реализация мультистартовой задачи. Повторяет базовую реализацию.

- __BaseSingleCancelJob__  
Реализация отменяемой сингл задачи.

- __BaseConcurrentCancelJob__  
Реализация отменяемой мультистартовой задачи.
