# Wedding Website

Lauras und Marvins [Hochzeits-Website](https://laura-und.marvin-stue.de).

# Lokal testen

## Voraussetzungen

- [NodeJs](https://nodejs.org/en)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Azure Function Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=macos%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp#install-the-azure-functions-core-tools)

## Website

Die Website ist static HTML, ergänzt um SCSS / Javascript. Preprocessing und minifying erfolgen mit Gulp.

```bash
cd src/app
npm install
npm run build:dev
```

Nachdem Gulp fertig ist, kann man einfach die [index.html](./src/app/index.html) aufrufen.
In der lokalen Entwicklung muss man dran denken, dass CSS/JS Änderungen erst mit einem erneuten `npm run build:dev` verfügbar sind.
Außerdem kann Caching je nach Browser Änderungen blockieren. In so einem Fall Cache clearen oder einen Icognito-Mode nutzen.

## Azure Functions

Azure Functions werden für Funktionen wie RSVP genutzt. Sie können ebenfalls lokal getestet werden.
Um die Function testen zu können, wird eine InviteCode configuration benötigt. Um diese zu erstellen, kann im `Function` folder eine eigene `local.settings.json` angelegt werden.
Inhalt:

```json
{
  "IsEncrypted": false,
  "Values": {

    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "RsvpOptions:InviteCode": "123456" // Der wird so auch für dev in der website gesetzt
  }
}
```

```bash
cd src/api/Function
func start
```

Nach erfolgreichem Build & Start können die Functions lokal aufgerufen werden, z.B.
```bash
curl \
  -X POST "http://localhost:7234/api/Rsvp" \
  -d "name=asd&email=asd@asd.de&extras=3&invite_code=123456" \
  -H "content-type: application/x-www-form-urlencoded"
```

Die API-URL ist in der script.js hinterlegt. Diese wird je nach Build `npm run build:dev` / `npm run build:azure` auf die passende URL geändert.

# Einrichtung

1. Ressourcen aus [Komponente](#komponenten) wie beschrieben einrichten. Die Verlinkung von Static Web App und Function erfolgt per Hand nachdem beides da ist über die Web App (Bereich APIs).
2. CNAME Record auf die Adresse der Static Web App setzen und in der App konfigurieren (*Custom domains*)
3. Im [gulpfile.js](./src/app/gulpfile.js) die Function App URL für production setzen (*<https://laura-und.marvin-stue.de/api/Rsvp>*). Für lokale Tests `npm run build:dev` nicht vergessen.
4. Google Maps Javascript API API-Key erzeugen und in der [index.html](./src/app/index.html) (Line 501) eintragen.
5. Mit Hilfe eines MD5 Generators, z.B. [hier](https://www.md5hashgenerator.com/) einen MD5 Hash einer sechstelligen Zahl generieren. Dies ist der Invite-Code. Der Hash wird in [gulpfile.js](./src/app/gulpfile.js) (`inviteCodeMd5`) eingetragen, die Zahl in den [Github Secrets](https://github.com/jabenz/wedding-website/settings/secrets/actions) (`RSVP_INVITE_CODE`). Nach Update der Codes muss ein Deployment laufen damit der in der Function gesetzt ist (oder man ändert die env var dort per Hand).

# Komponenten

Die Anwendung besteht aus einer [Static Web App](./src/app/) und [Azure Functions](./src/api/) für Funktionen

## Static Web App

**Resource:** [Link](https://portal.azure.com/#@hamburger-software.de/resource/subscriptions/9a9dbdfd-8117-4af8-8973-1a6e111f5f46/resourceGroups/wed-web/providers/Microsoft.Web/staticSites/wed-web/staticsite)

Die Azure Static Web App hostet die Website.

### Konfiguration

Nur Werte die vom Default abweichen:

- SKU: Standard
- Custom Domain (CNAME-style): laura-und.marvin-stue.de
- Backend: Bring-Your-Own-Function-Model
- Preview-Environments: Disabled

Das Deployment erfolgt über eine [Github-Action](./.github/workflows/azure-static-web-apps.yml).
Diese wurde mit Hilfe des [Azure Static Web Apps Toolkits](https://marketplace.visualstudio.com/items/?itemName=ms-azuretools.vscode-azurestaticwebapps) erzeugt.
Im Workflow wurden nur die Pfade angepasst.

## Function App

Wir benutzen Azure Functions, um Backend-Funktionen wie die RSVP-Funktion abzubilden.

### Konfiguration

Nur Werte die vom Default abweichen:

- SKU: Flex Consumption Linux
- Model: .NET 8.0 - Isolated Worker
- Application Insights: Enabled
- Anonymous Access: Enabled

## Cosmos DB

Azure Cosmos DB Table Storage wird verwendet, um die RSVPs zu speichern.

### Konfiguration

Nur Werte die vom Default abweichen:

- Workload: Azure Cosmos DB for Table
- Availability Zones: Disabled
- Free Tier Discount: Apply
- Backup storage redundancy: Zone-redundant

### Table vorbereiten

- In der Resource über den Data-Explorer eine neue Tabelle anlegen `rsvps`, 1000 RU/s.
- In der Azure Functions Evironment Variables konfigurieren `CosmosDbOptions__ConnectionString = <Aus CosmosDb Resource kopieren>`, `CosmosDbOptions__TableName = rsvps`
