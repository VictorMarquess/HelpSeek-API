# HelpSeek.API (ASP.NET Core Web API)

API central para integrar Desktop (WinForms), Web e Mobile ao **mesmo banco** `victor\mssqlserver01.HelpSeek.dbo`.

## Requisitos
- .NET 8 SDK
- SQL Server acessível (string em `appsettings.json`)
- (Opcional) dotnet-ef para scaffold
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Executar
```bash
dotnet restore
dotnet run
```
Swagger: https://localhost:5001/swagger

## Teste de conexão com o banco
`GET /api/health/db` → retorna `server`, `database` e quantidade de tabelas (usa ADO.NET).

## Scaffold do EF Core (opcional, recomendado)
Gere as entidades reais do seu banco HelpSeek e substitua o contexto placeholder:
```bash
dotnet ef dbcontext scaffold "Server=victor\mssqlserver01;Database=HelpSeek;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c HelpSeekContext --force
```

> Após rodar o scaffold, você pode criar controladores automáticos:
> - No Visual Studio: **Add → New Scaffolded Item → API Controller with actions, using EF**.

## Ajuste rápido dos endpoints
O controlador `ChamadosController` está **funcional via ADO.NET**, mas com tabela/colunas genéricas:
```sql
SELECT TOP 100 Id, Titulo, Descricao FROM Chamados ORDER BY Id DESC
```
Ajuste conforme os nomes reais do seu banco (`HelpSeek`).

## CORS
Liberado para todos (perfil desenvolvimento). Ajuste para produção em `Program.cs`.

## Segurança (sugestão)
- JWT Bearer para autenticação
- Logs de auditoria (LGPD)
- HTTPS obrigatório
