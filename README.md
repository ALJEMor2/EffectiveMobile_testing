Рекламные площадки - API сервис

Веб сервис для поиска рекламных площадок по локациям с учетом вложенности. 



Требования
.NET 9.0 SDK



Запуск
bash
cd EffectiveMobile
dotnet run

Сервис будет доступен по адресу: http://localhost:5000

API Endpoints

1. Загрузка площадок
   http
   POST /api/platforms/upload
   Content-Type: text/plain

Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik

2. Поиск площадок
   http
   GET /api/platforms?location=/ru/svrd/revda



Тестирование

bash
Запуск тестов
dotnet test



Тестирование API
curl -X POST http://localhost:5000/api/platforms/upload   
-H "Content-Type: text/plain"   
-d @../sample\_data.txt

curl "http://localhost:5000/api/platforms?location=/ru/svrd/revda"



Структура

EffectiveMobileTest/          Основной проект
EffectiveMobileTest.Test/     Тесты
sample\_data.txt               Пример данных
README.md                     Инструкция

