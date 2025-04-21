#!/bin/bash
# Script para fazer o deploy da aplicação no Minikube

echo "🚀 Iniciando deploy no Minikube..."

# Subir o Minikube (caso ainda não esteja ativo)
minikube status | grep -q "Running" || minikube start

echo "🔧 Aplicando configuração com Kustomize..."
kubectl apply -k k8s/overlays/minikube

echo "📦 Serviços:"
minikube service list

echo "✅ Deploy finalizado!"
