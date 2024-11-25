docker-compose up -d

dotnet-ef database drop -f -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\

dotnet-ef migrations remove -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations remove -c AccountsWriteDbContext -p .\src\Accounts\SachkovTech.Accounts.Infrastructure\ -s .\src\SachkovTech.Web\


dotnet-ef migrations add Issues_init -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef migrations add Accounts_init -c AccountsWriteDbContext -p .\src\Accounts\SachkovTech.Accounts.Infrastructure\ -s .\src\SachkovTech.Web\


dotnet-ef database update -c IssuesWriteDbContext -p .\src\Issues\SachkovTech.Issues.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef database update -c AccountsWriteDbContext -p .\src\Accounts\SachkovTech.Accounts.Infrastructure\ -s .\src\SachkovTech.Web\
dotnet-ef database update -c AccountsWriteDbContext -p .\src\Accounts\SachkovTech.Accounts.Infrastructure\ -s .\src\SachkovTech.Web

pause