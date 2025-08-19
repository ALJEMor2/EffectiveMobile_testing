# Рекламные площадки — API сервис

Веб-сервис для **поиска рекламных площадок по локациям** с учетом вложенности.



## Требования
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)



## Запуск сервиса

```bash
cd EffectiveMobile
dotnet run
```

После запуска сервис будет доступен по адресу:  
 [http://localhost:5000](http://localhost:5000)



## API Endpoints

### Загрузка площадок  
**POST** `/api/platforms/upload`  
`Content-Type: text/plain`

Пример данных:
```
Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
```

Пример ответа (успешная загрузка):
```json
{
  "status": "success",
  "message": "Platforms uploaded successfully",
  "count": 2
}
```



### Поиск площадок  
**GET** `/api/platforms?location=/ru/svrd/revda`

Пример ответа:
```json
  {
    "name": "Ревдинский рабочий",
    "locations": [
      "/ru/svrd/revda",
      "/ru/svrd/pervik"
    ]
  }
```



## Тестирование

### Запуск тестов
```bash
dotnet test
```

### Тестирование API

Загрузка данных:
```bash
curl -X POST http://localhost:5000/api/platforms/upload -H "Content-Type: text/plain" -d @../sample_data.txt
```

Поиск площадок:
```bash
curl "http://localhost:5000/api/platforms?location=/ru/svrd/revda"
```



## Структура проекта
```
EffectiveMobileTest/       # Основной проект
EffectiveMobileTest.Test/  # Тесты
sample_data.txt            # Пример данных
README.md                  # Инструкция
```
