![изображение](https://github.com/user-attachments/assets/965f269f-8beb-4a84-bcff-3895b9b4960c)# sachkov-tech

# Как подтянуть nuget пакеты других сервисов?

В репозитории есть несколько сервисов, чтобы общатсья друг с другом они использует общие пакеты из nuget. К этим пакетам имеют доступ только те, кто имеет доступ к организации Sachkov Tech.
Чтобы вы могли использовать nuget пакеты нужно добавить nuget source - https://nuget.pkg.github.com/SachkovTech/index.json
Гайд как это сделать:
1. Заходим в настройки github
![изображение](https://github.com/user-attachments/assets/ef67a9b7-73c2-48b6-98bd-10986c1a9683)
2. Создаём новый бессрочный токен classic с разрешениями на редактирование и удаление пакетов
![изображение](https://github.com/user-attachments/assets/b0940509-3798-4bc8-87f0-2a86982710aa)
![изображение](https://github.com/user-attachments/assets/e0e59136-966f-4f98-ae02-59f10e193e09)
3. Копируем токен и сохраняем
4. В rider или через консоль добавляем новый nuget source - https://nuget.pkg.github.com/SachkovTech/index.json
User - это ваш логин в github
Password - это токен
![изображение](https://github.com/user-attachments/assets/cc3713b2-d357-4a25-993c-beccd98989b6)
![изображение](https://github.com/user-attachments/assets/588733cd-ed5e-4862-9174-53251463bcee)
5. Теперь вы можете скачивать из nuget нужные пакеты

# Как пушить новые nuget пакеты?
Документация - [https://docs.github.com/ru/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry](https://docs.github.com/ru/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#publishing-a-package-using-a-nugetconfig-file)

# .editorconfig
Актуальный шаблон лежит в самом репозитории. Если для текущего проекта нужно расширить его функционал просто копируем .editorconfig в папку с решением .sln и конфигурируем. Если вводите что-то новое, то пишите это в конце после комментария #CUSTOM, править то что выше #CUSTOM не рекомендуется