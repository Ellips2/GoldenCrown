# GoldenCrown

Безопасное приложение финансового API, построенное на ASP.NET Core 10 и Entity Framework Core с поддержкой аутентификации пользователей, управления аккаунтами и отслеживанием транзакций.

## Описание проекта

GoldenCrown — это серверный API сервис, предоставляющий следующие возможности:

- **Управление пользователями**: Регистрация и вход с JWT аутентификацией
- **Управление аккаунтами**: Аккаунты пользователей с отслеживанием баланса
- **Финансовые операции**: Пополнение счета и перевод денег между пользователями
- **История транзакций**: Отслеживание всех финансовых транзакций с поддержкой пагинации
- **Управление сессиями**: Автоматическая очистка истекших сессий
- **Безопасность**: JWT авторизация с пользовательским middleware

### Стек технологий

- **.NET**: 10.0
- **База данных**: SQL Server
- **ORM**: Entity Framework Core
- **Документация API**: Swagger/OpenAPI
- **Аутентификация**: JWT токены

## Инструкция по запуску

### Требования

- .NET 10 SDK или выше
- SQL Server (локальная или удаленная)
- Visual Studio 2026 или VS Code с расширением C#

### Установка и настройка

1. **Клонируйте репозиторий**
   ```bash
   git clone https://github.com/Ellips2/GoldenCrown.git
   cd GoldenCrown
   ```

2. **Настройте строку подключения к базе данных**

   Обновите строку подключения в файле `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=имя_сервера;Database=GoldenCrown;Trusted_Connection=true;"
     }
   }
   ```

3. **Примените миграции базы данных**
   ```bash
   dotnet ef database update
   ```

4. **Соберите проект**
   ```bash
   dotnet build
   ```

5. **Запустите приложение**
   ```bash
   dotnet run
   ```

6. **Получите доступ к API**
   - URL приложения: `https://localhost:5000` (или настроенный порт)
   - Swagger UI: `https://localhost:5000/swagger`

## Примеры API запросов

### Аутентификация пользователей

#### Регистрация нового пользователя
```http
POST /api/user/register
Content-Type: application/json

{
  "login": "john_doe",
  "name": "Иван Иванов",
  "password": "SecurePassword123!"
}
```

Ответ: `200 OK`

#### Вход в систему
```http
POST /api/user/login
Content-Type: application/json

{
  "login": "john_doe",
  "password": "SecurePassword123!"
}
```

Ответ: `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Финансовые операции

Все финансовые операции требуют JWT аутентификации через заголовок `Authorization`:
```
Authorization: Bearer <ваш_jwt_токен>
```

#### Получить баланс счета
```http
GET /api/finance/balance
Authorization: Bearer <token>
```

Ответ: `200 OK`
```json
{
  "balance": 1000.00
}
```

#### Пополнить счет
```http
POST /api/finance/deposit
Authorization: Bearer <token>
Content-Type: application/json

{
  "amount": 500.00
}
```

Ответ: `200 OK`

#### Перевести деньги другому пользователю
```http
POST /api/finance/transfer
Authorization: Bearer <token>
Content-Type: application/json

{
  "receiverLogin": "jane_doe",
  "amount": 250.00
}
```

Ответ: `200 OK`

#### Получить историю транзакций
```http
POST /api/finance/history
Authorization: Bearer <token>
Content-Type: application/json

{
  "from": "2025-01-01T00:00:00Z",
  "to": "2025-12-31T23:59:59Z",
  "offset": 0,
  "limit": 10
}
```

Ответ: `200 OK`
```json
{
  "transactions": [
    {
      "id": 1,
      "type": "deposit",
      "amount": 500.00,
      "timestamp": "2025-01-15T10:30:00Z",
      "description": "Пополнение"
    },
    {
      "id": 2,
      "type": "transfer",
      "amount": 250.00,
      "timestamp": "2025-01-16T14:20:00Z",
      "description": "Перевод пользователю jane_doe"
    }
  ]
}
```

## Структура базы данных

### Схема базы данных

#### Таблица Users (Пользователи)
```sql
CREATE TABLE users (
  id INT PRIMARY KEY IDENTITY(1,1),
  login NVARCHAR(MAX) NOT NULL,
  name NVARCHAR(MAX) NOT NULL,
  password NVARCHAR(MAX) NOT NULL
);
```

| Столбец | Тип | Описание |
|---------|-----|---------|
| `id` | int (PK) | Уникальный идентификатор пользователя |
| `login` | nvarchar(max) | Логин для аутентификации |
| `name` | nvarchar(max) | Имя пользователя |
| `password` | nvarchar(max) | Хэшированный пароль |

#### Таблица Accounts (Аккаунты)
```sql
CREATE TABLE accounts (
  id INT PRIMARY KEY IDENTITY(1,1),
  user_id INT NOT NULL FOREIGN KEY REFERENCES users(id),
  balance DECIMAL(18,2) NOT NULL
);
```

| Столбец | Тип | Описание |
|---------|-----|---------|
| `id` | int (PK) | Уникальный идентификатор аккаунта |
| `user_id` | int (FK) | Ссылка на пользователя |
| `balance` | decimal(18,2) | Текущий баланс счета |

#### Таблица Transactions (Транзакции)
```sql
CREATE TABLE transactions (
  id INT PRIMARY KEY IDENTITY(1,1),
  account_id INT NOT NULL FOREIGN KEY REFERENCES accounts(id),
  type NVARCHAR(MAX) NOT NULL,
  amount DECIMAL(18,2) NOT NULL,
  timestamp DATETIME2 NOT NULL,
  description NVARCHAR(MAX)
);
```

| Столбец | Тип | Описание |
|---------|-----|---------|
| `id` | int (PK) | Уникальный идентификатор транзакции |
| `account_id` | int (FK) | Ссылка на аккаунт |
| `type` | nvarchar(max) | Тип транзакции (deposit, transfer) |
| `amount` | decimal(18,2) | Размер транзакции |
| `timestamp` | datetime2 | Дата и время транзакции |
| `description` | nvarchar(max) | Описание транзакции |

#### Таблица Sessions (Сессии)
```sql
CREATE TABLE sessions (
  id INT PRIMARY KEY IDENTITY(1,1),
  user_id INT NOT NULL FOREIGN KEY REFERENCES users(id),
  token NVARCHAR(MAX) NOT NULL,
  expires_at DATETIME2 NOT NULL
);
```

| Столбец | Тип | Описание |
|---------|-----|---------|
| `id` | int (PK) | Уникальный идентификатор сессии |
| `user_id` | int (FK) | Ссылка на пользователя |
| `token` | nvarchar(max) | JWT токен |
| `expires_at` | datetime2 | Время истечения сессии |

### Связи между сущностями

```
Users (1) ──→ (Many) Accounts
Users (1) ──→ (Many) Sessions
Accounts (1) ──→ (Many) Transactions
```

## Структура проекта

```
GoldenCrown/
├── Controllers/              # API контроллеры
│   ├── UserController.cs
│   └── FinanceController.cs
├── Models/                  # Модели данных
│   ├── User.cs
│   ├── Account.cs
│   ├── Transaction.cs
│   └── Session.cs
├── Services/                # Бизнес-логика
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IFinanceService.cs
│   ├── FinanceService.cs
│   ├── IAccountService.cs
│   └── AccountService.cs
├── DTOs/                    # Объекты передачи данных
│   ├── User/
│   └── Finance/
├── Database/
│   └── ApplicationDbContext.cs
├── Middlewares/             # Пользовательские middleware
│   └── AuthorizationMiddleware.cs
├── BackgroundServices/      # Фоновые сервисы
│   └── SessionCleanupService.cs
├── Migrations/              # EF Core миграции
├── Program.cs               # Точка входа приложения
└── appsettings.json         # Конфигурация
```

## Обработка ошибок

API возвращает стандартные HTTP коды состояния:

- `200 OK` — успешный запрос
- `400 Bad Request` — неверный ввод или ошибка бизнес-логики
- `401 Unauthorized` — отсутствует или недействителен токен аутентификации
- `404 Not Found` — ресурс не найден
- `500 Internal Server Error` — ошибка на сервере

Ошибки возвращаются в следующем формате:
```json
{
  "message": "Описание ошибки"
}
```

## Примечания для разработчиков

- Используются JWT токены для безгосударственной аутентификации
- Сессии автоматически очищаются каждые 10 минут
- Все денежные значения используют точность `decimal(18,2)`
- Временные метки хранятся в формате UTC
- Приложение использует EF Core для доступа к базе данных с асинхронными операциями

## Лицензия

Этот проект является частью организации Ellips2 на GitHub.

## Способы участия

Для внесения вклада обратитесь к репозиторию GitHub: https://github.com/Ellips2/GoldenCrown
