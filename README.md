# [Diascan-WebApi] PokemonReviewApp

Данное WebApi приложение было создано для прохождения практики в Диаскан.
Оно было создано с целью демонстрации возможностей абстрагированной работы с базой данных и взаимодействия с ASP .NET.

## Quick Start

- Настройте на машине PostgreSQL
- В файле Diascan-WebAPI/appsettings.json укажите актуальные данны для подключения к БД
 ```csharp
"DefaultConnection": "Host=localhost;Port=5432;Database=pokemonreview;Username=postgres;Password=123"
```
- Выполните две команды в диспетчере пакетов для миграции и обновление вашей базы данных
 ```sh
 Add-Migration Initial
 Update-Database
```
- Запускайте приложение, SwaggerUI откроется автоматически

## Стек использованных технологий
- База данных PostgreSQL
- Entity Framework
- AutoMapper
- ASP .NET
- SwaggerUI

###### P.S. Тесты актуальны, но я их переместил из другого решения
[![Tests](https://i.imgur.com/WXGY43a.png "Tests")](https://i.imgur.com/WXGY43a.png "Tests")
