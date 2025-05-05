> Back‑end **.NET 7** + SQL Server 2022, patrón **MVC + DAO**,
> API REST documentada en **Swagger 3** y vistas Razor para CRUD.

## Acceder al proyecto
**1. Clonar el repositorio**

```bash
git clone https://github.com/lau052004/personapi-dotnet.git
cd personapi-dotnet
```
**2. Descarga desde un TAG de release**
1. Ve a Releases → v1.0.0 y pulsa “Download source code (.zip)”.
2. Descomprime el archivo en tu equipo.
3. Abre una terminal en la carpeta del proyecto descomprimido y ejecuta:

```bash
cd personapi-dotnet      # carpeta descomprimida
git checkout v1.0.0
```

---

## Levantar contenedores

```bash
docker compose up --build -d
```

| Servicio   | Descripción              | Puerto   |
| ---------- | ------------------------ | -------- |
| **webapi** | .NET 7 + Swagger + Razor | **5062** |
| **db**     | SQL Server 2022 Express  | **1433** |

---

#### 3 . Probar la aplicación

| URL                              | Qué verás                         |
| -------------------------------- | --------------------------------- |
| `http://localhost:5062/swagger`  | Endpoints REST listos para probar |
| `http://localhost:5062/Personas` | Vistas MVC para CRUD (Razor)      |

---

#### 4 . Comandos útiles

```bash
docker compose down -v --remove-orphans # detener y borrar volúmenes/red
docker compose logs -f db          # ver inicialización y script SQL
docker compose exec webapi bash    # shell dentro de la WebAPI
```

---

### ⚠️ Nota importante sobre **entrypoint.sh**

El contenedor **db** ejecuta `entrypoint.sh` para:

1. Instalar **mssql‑tools**
2. Crear la base y tablas con `init.sql`

> El script **debe** estar guardado con finales de línea **LF (Unix)**.
> Si lo editas en Windows, cambia *CRLF → LF* en VS Code antes de `git add`.
> Si usas CRLF el contenedor se detendrá con errores de `command not found`.
