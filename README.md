# Simple Tactics

A multiplayer tactical duel game base on ASP.NET Core + SignalR + Vue.js + PIXI.js

![License](https://img.shields.io/github/license/Fragoler/simple-tactics)
![Docker](https://img.shields.io/badge/docker-ready-blue)
![Helm](https://img.shields.io/badge/helm-chart-0F1689)


## Quick start

### Docker
```sh
docker run harbor.fragoler.space/simple-tactics/gameserver:latest --rm --name gameserver -p 8080:8080 
```

### Local docker build

```sh
docker build -t simple-tactics .
docker run -p 8080:8080 simple-tactics

# Go to: http://localhost:8080
```

### Local dev run

```sh
git clone --recurse-submodules https://github.com/Fragoler/simple-tactics.git
cd simple-tactics

cd GameServer
dotnet run

cd GameFront
npm install
npm run dev

# http://localhost:5173
```


## Monitoring

### Health Checks

- **Liveness**: `GET /health` 
- **Readiness**: `GET /health/ready`

## Technology stack

### Backend:
- C# / .NET 9
- ASP.NET Core (Web API + SignalR)
- YamlDotNet (For prototypes)

### Frontend:
- Vue 3 + TypeScript
- PIXI.js
- Pinia
- Vite
- SignalR Client


## Links

- [Registry](https://harbor.fragoler.space/harbor/projects/2/repositories/gameserver/artifacts-tab?publicAndNotLogged=yes)
- [Game](https://tactics.fragoler.space)


## License

Simple Tactics is distributed under [MIT License](LICENSE)