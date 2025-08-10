# .NET Microservices – Distributed Application

This project demonstrates how to design, build, and deploy a microservices-based architecture using **.NET 9**, **Docker**, and modern communication patterns.

---

## 🚀 Tech Stack
- **.NET 9 / ASP.NET Core**
- **Entity Framework Core**
- **gRPC** for high-performance service-to-service communication[ongoing]
- **RabbitMQ / Event Bus** for asynchronous messaging[ongoing]
- **Docker & Docker Compose** for containerized deployment
- **SQL Server** as the primary database
- **REST API** and **Async Messaging**

---

## 📌 Features
- Multiple independent microservices
- REST and gRPC endpoints for inter-service communication
- Event-driven messaging with **RabbitMQ**
- Centralized configuration and environment variables
- Database migrations and data seeding
- Easy local deployment with Docker
- Scalable architecture ready for cloud deployment

---

## 🏗 Architecture Overview

```plaintext
[ PlatformService ]  <--->  [ CommandService ]
       |                         |
    REST/gRPC                 RabbitMQ
```
- PlatformService – Manages platforms and exposes APIs (REST + gRPC)
- CommandService – Handles commands for platforms, subscribed via RabbitMQ events


## 🛠 Getting Started
Follow these steps to run the project locally.

**1. Prerequisites**
Make sure you have installed:
  - Docker & Docker Compose
  - .NET 9 SDK
  - Git

**2. Clone the Repository and build the microservices**
```

  git clone https://github.com/BassamRamadan/.NET-Microservices.git
  cd .NET-Microservices
  docker-compose up --build

```
**3. Verify Running Services**
- PlatformService API → ```http://localhost:5154```
- CommandService API → ```http://localhost:5171```


**4. Test the APIs via Postman/curl**

  - Create a new platform
    ```
    curl -X POST http://localhost:5154/api/platforms \
    -H "Content-Type: application/json" \
    -d '{"name":"DotNet","publisher":"Microsoft","cost":"Free"}'
    ```
  - Get all platforms
    ```
    curl http://localhost:5154/api/platforms
    ```
