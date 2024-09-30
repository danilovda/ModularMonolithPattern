# ModularMonolithPattern

This solution demonstrates a **Modular Monolith Architecture** using **ASP.NET Core**. It consists of a primary **Web API** that interacts with two separate modules—**Inventory** and **Orders**—which are implemented as distinct class libraries. The goal of this project is to illustrate how to structure a modular monolith where each module has its own database schema, while communication between them is achieved asynchronously using **RabbitMQ**.

---

## Project Structure

The solution is composed of the following main components:

- **WebApi**: The entry point of the application, implemented with **ASP.NET Core**. It serves as the API layer and coordinates requests between the modules.
- **Modules.Inventory**: A class library that handles inventory-related logic. This module uses the `products` schema in the database.
- **Modules.Orders**: A class library responsible for handling order-related operations. This module uses the `orders` schema in the database.
- **Common**: A shared library containing common services, utilities, and base classes used across modules.

---

## Modules Overview

### Modules.Inventory

- **Schema**: `products`
- **Functionality**: Handles product catalog management, inventory levels, and product-related business logic.
- **Database**: Data related to the inventory is stored in the `products` schema of the MSSQL database.
- **Communication**: This module can send and receive messages via **RabbitMQ**, using **DTOs** from the `Modules.Inventory.Models` namespace.

### Modules.Orders

- **Schema**: `orders`
- **Functionality**: Manages customer orders, processing, and order status updates.
- **Database**: Order-related data is stored in the `orders` schema of the MSSQL database.
- **Communication**: This module interacts with other services via **RabbitMQ**, utilizing **DTOs** defined in `Modules.Orders.Models`.

---

## Communication Between Modules

The modules within this solution communicate asynchronously using **RabbitMQ**. When events occur within one module (e.g., an order is placed), the event is published to RabbitMQ, and the relevant module (e.g., the Inventory module) will subscribe to and process the event as necessary.

This approach decouples the modules, allowing them to operate independently while still being part of the monolithic architecture.

---

## Telemetry and Monitoring

- **OpenTelemetry**: The solution is instrumented with **OpenTelemetry** to capture telemetry data across the entire system, allowing for comprehensive tracing and metrics collection.
- **Jaeger**: Collected telemetry data is visualized in **Jaeger**, where traces of requests and operations can be inspected, providing insights into system performance and module interactions.

---

## Running the Application

This project uses **Docker Compose** to orchestrate the following services:

- **Web API** (ASP.NET Core)
- **MSSQL** (Database for storing modular data with separate schemas for each module)
- **RabbitMQ** (Message broker for asynchronous communication)
- **Jaeger** (For distributed tracing and monitoring)

---

## Technologies Used

- **ASP.NET Core**: Web API framework
- **RabbitMQ**: Message broker for asynchronous communication
- **MSSQL**: Database with separate schemas for different modules
- **OpenTelemetry**: Telemetry collection for tracing and metrics
- **Jaeger**: Visualizing distributed traces
- **Docker Compose**: Container orchestration

