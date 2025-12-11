#!/bin/bash

set -e  # Stop script on error

echo "🚀 STARTING FULL K8S SETUP..."

########################################
# 1) Start Minikube
########################################
echo "🔹 1) Starting Minikube..."
minikube start --driver=docker

########################################
# 2) Switch Docker to Minikube
########################################
echo "🔹 2) Switching Docker to Minikube..."
eval $(minikube docker-env)
docker info | grep "Docker Root Dir"

########################################
# 3) Build Docker images
########################################
echo "🔹 3) Building Docker images..."

PROJECTS=(
  "api-gateway:./src/api-gateway"
  "audit-service:./src/microservices/audit-service"
  "election-service:./src/microservices/election-service"
  "identity-service:./src/microservices/identity-service"
  "tally-service:./src/microservices/tally-service"
  "vote-casting-service:./src/microservices/vote-casting-service"
  "voter-registry-service:./src/microservices/voter-registry-service"
)

for item in "${PROJECTS[@]}"; do
  IMAGE="${item%%:*}"
  PATH_DIR="${item#*:}"

  if [[ ! -d "$PATH_DIR" ]]; then
    echo "❌ ERROR: Directory does not exist → $PATH_DIR"
    exit 1
  fi

  echo "🔨 Building $IMAGE from $PATH_DIR ..."
  docker build -t "$IMAGE:latest" "$PATH_DIR"
done

echo "✅ All Docker images built successfully."

########################################
# 4) Deploy to Kubernetes
########################################
echo "🔹 4) Applying Kubernetes manifests..."

kubectl apply -f deploy/k8s/secrets.yaml --validate=false
kubectl apply -f deploy/k8s/postgres/ --validate=false
kubectl apply -R -f deploy/k8s --validate=false

########################################
# 5) Wait for pods to become Running
########################################
echo "⏳ Waiting for services to start..."
sleep 5
kubectl wait --for=condition=ready pod --all --timeout=120s

echo "✅ All pods are running."

########################################
# 6) Port-forward all services
########################################
echo "🔹 6) Enabling port forwarding..."

SERVICES=(
  "api-gateway:9000"
  "audit-service:9001"
  "identity-service:9002"
  "election-service:9003"
  "tally-service:9004"
  "vote-casting-service:9005"
  "voter-registry-service:9006"
)

for item in "${SERVICES[@]}"; do
  NAME="${item%%:*}"
  PORT="${item#*:}"
  POD=$(kubectl get pod -l app=$NAME -o jsonpath="{.items[0].metadata.name}")

  if [[ -z "$POD" ]]; then
    echo "❌ ERROR: Pod for $NAME not found!"
    continue
  fi

  echo "➡️ Forwarding $NAME on port $PORT..."
  kubectl port-forward "$POD" "$PORT:8080" >/dev/null 2>&1 &
done

########################################
# 7) Output Swagger URLs
########################################
echo ""
echo "🎉 ALL SERVICES ARE READY!"
echo "------------------------------------"
echo "API Gateway:            http://localhost:9000/index.html"
echo "Audit Service:          http://localhost:9001/index.html"
echo "Identity Service:       http://localhost:9002/index.html"
echo "Election Service:       http://localhost:9003/index.html"
echo "Tally Service:          http://localhost:9004/index.html"
echo "Vote Casting Service:   http://localhost:9005/index.html"
echo "Voter Registry Service: http://localhost:9006/index.html"
echo "------------------------------------"
echo "🔥 Enjoy your full Kubernetes environment!"
