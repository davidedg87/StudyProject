# StudyProject

## Descrizione

StudyProject è un'applicazione nata allo scopo di testare le potenzialità del mondo .NET.
Il sistema permette di creare e gestire ordini.
Il progetto è configurato per essere eseguito in un ambiente containerizzato utilizzando Podman e Podman Compose, 
ma può essere eseguito anche senza container per una configurazione locale.

## Requisiti

- **Podman**: Utilizzato per la gestione dei container.
- **Podman-compose**: Compatibile con Docker Compose, necessario per gestire i container in Podman.
- **.NET SDK 8.0**: Per compilare ed eseguire il progetto in locale.
- **PostgreSQL**: Database utilizzato dal progetto.

## Installazione e Avvio

### 1. Clona il Progetto

Poiché il progetto è sotto versionamento, puoi clonare il repository direttamente utilizzando Git. Esegui il comando:

```bash
git clone https://github.com/davidedg87/StudyProject.git
```

### 2. Configurazione dell'Ambiente

#### Con Docker/Podman:

1. **Configura il file `docker-compose.yml`**:
   - Assicurati che il file `docker-compose.yml` sia configurato correttamente per l'ambiente di sviluppo, come indicato nel seguente esempio:

    ```yaml
    version: '3.8'

	services:
	  redis:
		image: redis:latest
		container_name: my-redis
		ports:
		  - "6379:6379"  # Esponi la porta di Redis
		volumes:
		  - redis_data:/data  # Monta un volume per la persistenza dei dati
	 
	  postgres:
		image: postgres:latest
		container_name: my-postgres
		environment:
		  POSTGRES_USER: myuser
		  POSTGRES_PASSWORD: mypassword
		  POSTGRES_DB: order-monitoring-db
		ports:
		  - "6543:5432"
		volumes:
		  - postgres_data:/var/lib/postgresql/data  # Aggiungi questa riga
		networks:
		  - app_network

	  rabbitmq:
		image: rabbitmq:3-management
		container_name: my-rabbitmq
		ports:
		  - "5672:5672" # Porta per le connessioni AMQP
		  - "15672:15672" # Porta per l'interfaccia di gestione
		networks:
		  - app_network
		volumes:
		  - rabbitmq_data:/var/lib/rabbitmq  # Volume per RabbitMQ
		
	  order_management_consumer:
		build:
		  context: .  # Imposta il contesto al livello superiore
		  dockerfile: src/OrderManagement.MessageConsumer/Dockerfile  # Specifica il percorso del Dockerfile
		environment:
		  ASPNETCORE_ENVIRONMENT: Development
		  ConnectionStrings__DefaultConnection: "Host=postgres;Database=order-monitoring-db;Username=myuser;Password=mypassword;"
		  RabbitMq__Host: "rabbitmq"
		  Elastic__Url: "http://elasticsearch:9200"
		  Redis__Host: "redis"
		ports:
		  - "5000:80"
		depends_on:
		  - postgres
		  - rabbitmq
		  - elasticsearch
		networks:
		  - app_network
		  
	  order_background_service:
		build:
		  context: .  # Imposta il contesto al livello superiore
		  dockerfile: src/OrderManagement.BackgroundServices/Dockerfile  # Specifica il percorso del Dockerfile
		environment:
		  ASPNETCORE_ENVIRONMENT: Development
		  ConnectionStrings__DefaultConnection: "Host=postgres;Database=order-monitoring-db;Username=myuser;Password=mypassword;"
		  RabbitMq__Host: "rabbitmq"
		  Elastic__Url: "http://elasticsearch:9200"
		  Redis__Host: "redis"
		ports:
		  - "5001:80"
		depends_on:
		  - postgres
		  - rabbitmq
		  - elasticsearch
		networks:
		  - app_network
		  
	  order_management_api:
		build:
		  context: .  # Imposta il contesto al livello superiore
		  dockerfile: src/OrderManagement.API/Dockerfile  # Specifica il percorso del Dockerfile
		environment:
		  ASPNETCORE_ENVIRONMENT: Development
		  ConnectionStrings__DefaultConnection: "Host=postgres;Database=order-monitoring-db;Username=myuser;Password=mypassword;"
		  RabbitMq__Host: "rabbitmq"
		  Elastic__Url: "http://elasticsearch:9200"
		  Redis__Host: "redis"
		ports:
		  - "5002:8080"
		depends_on:
		  - postgres
		  - rabbitmq
		  - elasticsearch
		networks:
		  - app_network

	  elasticsearch:
		image: docker.elastic.co/elasticsearch/elasticsearch:8.10.2
		container_name: my-elastic-search
		environment:
		  - discovery.type=single-node
		  - xpack.security.enabled=false
		  - ES_JAVA_OPTS=-Xms512m -Xmx512m
		ports:
		  - "9200:9200"
		networks:
		  - app_network
		volumes:
		  - elasticsearch_data:/usr/share/elasticsearch/data  # Volume per Elasticsearch

	  kibana:
		image: docker.elastic.co/kibana/kibana:8.10.2
		container_name: my-kibana
		environment:
		  - ELASTICSEARCH_HOSTS=http://elasticsearch:9200
		ports:
		  - "5601:5601"
		depends_on:
		  - elasticsearch
		networks:
		  - app_network

	networks:
	  app_network:
		driver: bridge
		
	volumes:
	  postgres_data:  # Volume per PostgreSQL
	  rabbitmq_data:  # Volume per RabbitMQ
	  elasticsearch_data:  # Volume per Elasticsearch
	  redis_data: #Volume per Redis

    ```

2. **Avvia i container con Podman**:
   - Assicurati di avere **Podman** e **Podman Compose** installati. Se non li hai, installali seguendo la documentazione ufficiale:
     - [Installazione Podman](https://podman.io/getting-started/installation)
     - [Installazione Podman Compose](https://github.com/containers/podman-compose)
   - Avvia i container eseguendo il seguente comando nella directory contenente il file `docker-compose.yml`:

    ```bash
    podman-compose -f docker-compose.yml up --build
    ```

   Questo comando costruirà e avvierà i container per il database PostgreSQL, Redis, RabbitMQ, ElasticSearch, Kibana, API Order Management, Application BackgroundService gestione ordini, Application Consumer Messaggi.

#### Senza Docker/Podman (In Locale):

Se preferisci eseguire il progetto senza Docker o Podman, esegui i seguenti passaggi:

1. **Configura PostgreSQL in locale**:
   - Installa PostgreSQL sul tuo sistema (seguendo le istruzioni per il tuo sistema operativo).
   - Crea un database chiamato `test-db` e un utente con i permessi necessari.

2. **Configura la Stringa di Connessione**:
   - Modifica il file `appsettings.Development.json` per includere la stringa di connessione al tuo database locale PostgreSQL:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=test-db;Username=myuser;Password=mypassword;"
      }
    }
    ```

3. **Avvia il Progetto**:
   - Apri il progetto in Visual Studio o usa il comando `dotnet` per avviarlo:

    ```bash
    dotnet run
    ```

   Il progetto verrà avviato sulla porta predefinita (`http://localhost:5000`).

### 3. API Disponibili

Le seguenti API sono disponibili nell'applicazione:

- **Health**:
  - `GET /health`: Stato PostgreSql, DBContext , RabbitMQ, ElasticSearch, Redis 

Porta di ascolto per le API:
- In ambiente locale (senza Docker):  
  HTTP: `http://localhost:5172`  
  HTTPS: `https://localhost:7101`

- In ambiente Docker:  
  La porta interna del container Docker è configurata su `5002`.

### 4. Testing

Per eseguire i test unitari:

1. **Esegui i Test con .NET CLI**:

    ```bash
    dotnet test
    ```

   Questo comando eseguirà tutti i test definiti nel progetto.

### 5. Configurazione di Rete e Volumi

Vengono utilizzati i seguenti volumi e reti all'interno di `docker-compose.yml`:

```yaml
volumes:
	  postgres_data:  # Volume per PostgreSQL
	  rabbitmq_data:  # Volume per RabbitMQ
	  elasticsearch_data:  # Volume per Elasticsearch
	  redis_data: #Volume per Redis
	  
networks:
		  - app_network
