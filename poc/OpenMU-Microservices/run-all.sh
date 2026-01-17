#!/bin/bash

echo "Starting OpenMU Microservices..."

# Start Auth Service
echo "Starting Auth Service on port 5001..."
cd AuthService/AuthService
dotnet run &
AUTH_PID=$!

# Wait for Auth Service to start
sleep 5

# Start Chat Service
echo "Starting Chat Service on port 5002..."
cd ../../ChatService/ChatService
dotnet run &
CHAT_PID=$!

# Start Friend Service
echo "Starting Friend Service on port 5003..."
cd ../../FriendService/FriendService
dotnet run &
FRIEND_PID=$!

echo ""
echo "All services started!"
echo "Auth Service: http://localhost:5001"
echo "Chat Service: http://localhost:5002"
echo "Friend Service: http://localhost:5003"
echo ""
echo "Press Ctrl+C to stop all services"

# Wait for Ctrl+C
trap "kill $AUTH_PID $CHAT_PID $FRIEND_PID; exit" INT
wait
