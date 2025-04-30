# Health Center Backend

## Tech Stack

- .net 8
- Tools:
  Unit Test: xUnit

- Database: PostgreSQL
- Real-time: SignalR, SSE

## CI/CD

- via Github actions

- in appsetttings.json, if we use nested values like

```
{
  value:{
    child:"abc"
  }
}
```

we use double underscore in Azure web app -- app settings tab, like value\_\_child

- for ConnectionStrings, in AZure web app -- connction strings tab, use the same name

```
"ConnectionStrings": {
  "Default": "Host=your-db-server;Database=your-db;Username=your-user;Password=your-password;"
}
```

Just like the below image
![azure](/screenshots/1appsettings_in_azure.png)

The deployed URL address is the below:
[azure web app](https://eclinic-jennifer-gtg8hzadcgcdeff3.canadacentral-01.azurewebsites.net/)
