dotnet clean ./CommentService/
dotnet clean ./backend/
dotnet clean ./EmailNotificationService/
dotnet clean ./FaqService/
dotnet clean ./FileService/
dotnet clean ./NotificationService/
dotnet clean ./TagService/
dotnet clean ./TelegramBotService/
dotnet restore ./backend/SachkovTech.sln
dotnet restore ./CommentService/CommentService.sln
dotnet restore ./EmailNotificationService/EmailNotificationService.sln
dotnet restore ./FileService/FileService.sln
dotnet restore ./NotificationService/NotificationService.sln
dotnet restore ./TagService/TagService.sln
dotnet restore ./TelegramBotService/TelegramBotService.sln
dotnet build ./CommentService/ --configuration Release --no-restore
dotnet build ./backend/ --configuration Release --no-restore
dotnet build ./EmailNotificationService/ --configuration Release --no-restore
dotnet build ./FaqService/ --configuration Release --no-restore
dotnet build ./FileService/ --configuration Release --no-restore
dotnet build ./NotificationService/ --configuration Release --no-restore
dotnet build ./TagService/ --configuration Release --no-restore
dotnet build ./TelegramBotService/ --configuration Release --no-restore