# Документация проекта JalgrataEksamDotNet

## 1. Этапы создания проекта

Ниже описаны ключевые этапы, которые отражают структуру приложения и его функции:

1. **Анализ требований** — выделение ролей (пользователь/администратор), определения фильтрации, сортировки и API. Основано на контроллере и UI-логике MVC/JS, где реализованы фильтры, сортировка и админ-режим.【F:Views/Exams/Index.cshtml†L8-L251】【F:Controllers/ExamsController.cs†L7-L66】
2. **Настройка ASP.NET Core (MVC + API)** — приложение использует MVC страницы и отдельные API endpoints для таблицы и управления данными.【F:Program.cs†L6-L49】【F:Controllers/Api/ExamsApiController.cs†L1-L168】
3. **База данных и seed** — подключение SQLite и первичное заполнение базы из XML при запуске приложения.【F:Program.cs†L14-L26】
4. **Пользовательский интерфейс таблицы** — форма фильтрации, сортировка по колонкам и рендер таблицы через API-запросы.【F:Views/Exams/Index.cshtml†L8-L197】
5. **Админ-режим** — активация режима, добавление и удаление записей, профиль администратора.【F:Views/Exams/Index.cshtml†L52-L251】【F:Views/Exams/Profile.cshtml†L1-L23】【F:Controllers/Api/ExamsApiController.cs†L94-L150】
6. **Swagger для проверки API** — Swagger UI подключён в dev-среде для просмотра и тестирования API.【F:Program.cs†L28-L37】

![Этапы создания проекта](docs/images/project-stages.svg)

## 2. Роли в системе

Основные роли:

- **Пользователь (просмотр)** — читает таблицу, фильтрует, сортирует, экспортирует JSON.【F:Views/Exams/Index.cshtml†L8-L51】
- **Администратор** — активирует админ-режим, добавляет/удаляет экзамены, использует профиль администратора.【F:Views/Exams/Index.cshtml†L52-L251】【F:Views/Exams/Profile.cshtml†L1-L23】【F:Controllers/Api/ExamsApiController.cs†L94-L150】

![Роли в системе](docs/images/roles-guide.svg)

## 3. Руководство пользователя по ролям

### 3.1 Пользователь (просмотр)

**Что доступно:**
- Просмотр таблицы экзаменов.
- Фильтр по имени (экзаменатор/ученик) и дате.
- Сортировка по колонкам.
- Экспорт JSON через кнопку `Export to JSON`.

**Как использовать:**
1. Перейдите на главную страницу `/Exams/Index`.
2. Введите имя или дату в форму фильтра и нажмите **Otsi**.
3. Для сортировки нажмите на название колонки (ID, Exam aeg, Koht и т.д.).
4. Для экспорта данных нажмите **Export to JSON** (скачивается файл `eksamid.json`).【F:Views/Exams/Index.cshtml†L8-L197】

### 3.2 Администратор

**Что доступно:**
- Добавление экзамена (форма внизу таблицы).
- Удаление экзамена кнопкой **Kustuta**.
- Страница профиля администратора.

**Как войти в админ-режим:**
1. Откройте `/Exams/Index`.
2. Трижды нажмите на заголовок страницы **Exam detailid**.
3. Введите пароль администратора (по умолчанию `1234`).
4. После успешного входа вы попадёте на страницу профиля `/Exams/Profile`.

> Пароль проверяется через API `/Exams/EnableAdmin`, а активный статус администратора хранится в сессии.【F:Views/Exams/Index.cshtml†L198-L251】【F:Controllers/ExamsController.cs†L17-L62】

**Добавление экзамена:**
1. В админ-режиме заполните форму **Admin: lisa uus eksam**.
2. Нажмите **Lisa eksam**.
3. Запись появится в таблице после обновления.【F:Views/Exams/Index.cshtml†L55-L251】【F:Controllers/Api/ExamsApiController.cs†L94-L120】

**Удаление экзамена:**
1. В строке экзамена нажмите **Kustuta**.
2. Запись будет удалена, если админ-сессия активна.【F:Views/Exams/Index.cshtml†L116-L183】【F:Controllers/Api/ExamsApiController.cs†L123-L150】

**Выход из админ-режима:**
1. Откройте **Admin profiil**.
2. Нажмите **Logi välja administ**.
3. Сессия будет сброшена и вы вернётесь в обычный режим.【F:Views/Exams/Profile.cshtml†L1-L23】【F:Controllers/ExamsController.cs†L47-L52】

## 4. Swagger и проверка API

Swagger UI доступен **только в среде Development**. Перейдите по адресу:

```
http://<host>:<port>/swagger
```

Swagger подключён через `UseSwagger()` и `UseSwaggerUI()` в блоке для development-режима.【F:Program.cs†L28-L37】

## 5. Основные API запросы

- `GET /api/exams` — таблица экзаменов с фильтрами и сортировкой.
- `GET /api/exams/export-json` — экспорт в JSON.
- `POST /api/exams` — добавление экзамена (только админ).
- `DELETE /api/exams/{id}` — удаление экзамена (только админ).

![Основные API запросы](docs/images/api-flow.svg)

Источники реализованы в `ExamsApiController`.【F:Controllers/Api/ExamsApiController.cs†L17-L150】
