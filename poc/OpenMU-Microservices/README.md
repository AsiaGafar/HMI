# OpenMU Microservices Architecture

This project demonstrates converting OpenMU to a microservices architecture with 3 core features:

## Services

### 1. AuthService (Port 5001)
- User authentication (login/logout)
- User management
- Token generation

**Endpoints:**
- POST `/api/auth/login` - Login user
- POST `/api/auth/logout` - Logout user
- GET `/api/auth/users/{id}` - Get user details

### 2. ChatService (Port 5002)
- Global chat messaging
- Message history
- Real-time communication

**Endpoints:**
- POST `/api/chat/send?senderId={guid}` - Send message
- GET `/api/chat/messages?type=Global` - Get messages

### 3. FriendService (Port 5003)
- Friend requests
- Friend list management
- Accept/reject requests

**Endpoints:**
- POST `/api/friend/request?userId={guid}` - Send friend request
- POST `/api/friend/accept/{friendshipId}` - Accept friend request
- GET `/api/friend/list?userId={guid}` - Get friends list

## Communication

All services communicate via **OpenAPI (REST)** using HTTP:
- ChatService → AuthService (to validate users)
- FriendService → AuthService (to validate users)

## Running the Services

### Option 1: Run individually
```bash
# Terminal 1 - Auth Service
cd AuthService/AuthService
dotnet run

# Terminal 2 - Chat Service
cd ChatService/ChatService
dotnet run

# Terminal 3 - Friend Service
cd FriendService/FriendService
dotnet run
```

### Option 2: Docker Compose
```bash
docker-compose up --build
```

## Testing the APIs

### 1. Login
```bash
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

### 2. Send Chat Message
```bash
curl -X POST "http://localhost:5002/api/chat/send?senderId=<USER_ID>" \
  -H "Content-Type: application/json" \
  -d '{"content":"Hello World","chatType":"Global"}'
```

### 3. Get Messages
```bash
curl http://localhost:5002/api/chat/messages?type=Global
```

### 4. Send Friend Request
```bash
curl -X POST "http://localhost:5003/api/friend/request?userId=<USER_ID>" \
  -H "Content-Type: application/json" \
  -d '{"friendId":"<FRIEND_ID>"}'
```

## OpenAPI Documentation

Access Swagger UI for each service:
- AuthService: http://localhost:5001/openapi/v1.json
- ChatService: http://localhost:5002/openapi/v1.json
- FriendService: http://localhost:5003/openapi/v1.json

## Architecture

```
┌─────────────┐
│   Client    │
└──────┬──────┘
       │
   ┌───┴────┬────────────┐
   │        │            │
┌──▼──┐  ┌─▼──┐     ┌───▼───┐
│Auth │  │Chat│     │Friend │
│5001 │  │5002│     │5003   │
└─────┘  └──┬─┘     └───┬───┘
            │            │
            └────────────┘
         (HTTP/OpenAPI)
```

## Project Structure

```
OpenMU-Microservices/
├── Shared/
│   └── OpenMU.Shared/          # Shared models and DTOs
├── AuthService/
│   └── AuthService/            # Authentication microservice
├── ChatService/
│   └── ChatService/            # Chat microservice
├── FriendService/
│   └── FriendService/          # Friend management microservice
├── docker-compose.yml
└── README.md
```

## Next Steps

1. Add database persistence (PostgreSQL/MongoDB)
2. Implement JWT authentication
3. Add message queue (RabbitMQ) for async communication
4. Add API Gateway (Ocelot/YARP)
5. Implement service discovery (Consul)
6. Add monitoring (Prometheus/Grafana)
