1. Склонировать данный GitHub репозиторий.
2. Установить docker desktop
3. Создать RabbitMq контейнер при помощи команды:
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management
4. В проекте FileParser:
4.1. NuGet:
dotnet add package Newtonsoft.Json --version 13.0.3
dotnet add package NLog --version 5.2.8
dotnet add package RabbitMQ.Client --version 6.8.1
dotnet add package System.CodeDom --version 8.0.0
dotnet add package System.Configuration.ConfigurationManager --version 8.0.0
4.2. App.config:
XmlPath - путь до xml файла из которого получаются данные (status.xml)
RabbitMQHostName - название RabbitMQ сервера (по умолчанию localhost)
QueueName - название очереди в RabbitMQ (должны совпадать в каждом проекте проектов)
5. В проекте DataProcessor:
5.1. NuGet:
dotnet add package System.Configuration.ConfigurationManager --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.2
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.2
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.2
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.2
dotnet add package Newtonsoft.Json --version 13.0.3
dotnet add package NLog --version 5.2.8
dotnet add package RabbitMQ.Client --version 6.8.1
5.2. App.config:
RabbitMQHostName - название RabbitMQ сервера (по умолчанию localhost)
QueueName - название очереди в RabbitMQ (должны совпадать в каждом проекте проектов)
SqlLite - строка подключения к базе данных. У параметра Data Source указать путь к базе данных которая была склонирована с GitHub репозитория
6. Запустить приложение FileParser и DataProcessor
