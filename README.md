# Отчет по проекту Zoo API

## 1. Реализованный функционал
- **Животные**  
  - Добавление, удаление, получение списка и по ID (`AnimalsController`, `IAnimalRepository`)  
  - Пометка больным и лечение (`AnimalsController`)  
  - Перемещение между вольерами (`AnimalTransferService`, `AnimalsController`)
- **Вольеры**  
  - Добавление, удаление, получение списка и по ID (`EnclosuresController`, `IEnclosureRepository`)  
  - При удалении — освобождение животных (`EnclosuresController`)
- **Расписание кормлений**  
  - Планирование новых кормлений (`FeedingOrganizationService`, `FeedingScheduleController`)  
  - Получение расписания (фильтрация по дате)  
  - Отметка о выполнении кормления (`FeedingOrganizationService`, `FeedingScheduleController`)
- **Статистика**  
  - Сбор общей статистики: количество животных, число вольеров, занятые/свободные (`ZooStatisticsService`, `StatisticsController`)

## 2. Архитектура и слои

### Domain (ядро)
- **Сущности (Entities)**: `Animal`, `Enclosure`, `FeedingSchedule`  
- **Value Objects**: `AnimalId`, `EnclosureId`, `FoodType`  
- **Перечисления**: `Gender`, `HealthStatus`, `EnclosureType`  
- **Доменные события**: `AnimalMovedEvent`, `FeedingTimeEvent`  
- Бизнес-правила и инварианты инкапсулированы в методах сущностей

### Application (слой бизнес-логики)
- **DTO**: `AnimalDto`, `CreateAnimalDto`, `EnclosureDto`, `CreateEnclosureDto`, `FeedingScheduleDto`, `CreateFeedingScheduleDto`, `ZooStatisticsDto`  
- **Интерфейсы**: `IAnimalTransferService`, `IFeedingOrganizationService`, `IStatisticsService`  
- **Сервисы**: `AnimalTransferService`, `FeedingOrganizationService`, `StatisticsService`

### Infrastructure (взаимодействие с внешним миром)
- **In-memory репозитории**: `InMemoryAnimalRepository`, `InMemoryEnclosureRepository`, `InMemoryFeedingScheduleRepository`  
- **Dispatching доменных событий**: `DomainEventDispatcher`  
- **Обработка ошибок**: `ErrorHandlingMiddleware`

### Presentation (Web API)
- ASP.NET Core Web API проект (`Microsoft.NET.Sdk.Web`)  
- **Контроллеры**:  
  - `AnimalsController`  
  - `EnclosuresController`  
  - `FeedingScheduleController`  
  - `StatisticsController`  
- Полная настройка Swagger с XML-комментариями и атрибутами Swashbuckle  
- Маршруты соответствуют RESTful-стилю: `/api/Animals`, `/api/Enclosures`, etc.

## 3. Применение принципов

### Domain-Driven Design (DDD)
- Явное разделение сущностей, VO и событий  
- Инкапсуляция бизнес-логики в методах доменных объектов  
- Использование Value Objects для семантически значимых примитивов

### Clean Architecture
- Слои зависят только внутрь (Domain не зависит ни от кого)  
- Все связи между слоями реализованы через интерфейсы  
- Бизнес-логика находится только в Domain и Application слоях


## 5. Запуск приложения
1. Клонировать репозиторий и перейти в папку `Presentation`  
2. Выполнить `dotnet restore`  
3. Запустить `dotnet run`  
4. Открыть Swagger UI по ссылке из терминала
