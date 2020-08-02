# Мониторинг сервера
## Как работать:
### Подключаем GoogleSheets:
Предварительно создать таблицу и запомнить её id.
Пример: docs.google.com/spreadsheets/d/__1qJodoeJxOdVFUy8SqGApvgtAKygl2Ww4XHKMslQqO1M__ - жирным выделен id.
Перейти по [ссылке](https://console.developers.google.com/apis/library/sheets.googleapis.com) и включить api на своем аккаунте.
После завершения настройки скачается .json файл с данными учетной записи.
Данный файл переименовать в `credits.json` и поместить в Test/Test .
В данном файле скопировать почту (поле client_email) и дать права редактирования таблицы для данной почты.
### Подключаем PostgreSQL:
В файле конфигурации `app.config` требуется вписать в секцию `Servers` данные о серверах, где `key` - адрес сервера, `value` - размер сервера в ГБ.
`user`, `password` - данные учетной записи PostgreSQL.
`delaySeconds` - время между обновлением информацией в секундах.
`sheetId` - id таблицы, который мы запомнили в самом начале.
