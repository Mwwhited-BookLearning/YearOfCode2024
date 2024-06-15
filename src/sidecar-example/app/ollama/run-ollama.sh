#!/bin/bash

BASE_MODEL=$1
echo "Starting Ollama server..."
ollama serve &
OLLAMA_PID=$!

echo "Waiting for Ollama server to be active..."
while [ "$(ollama list | grep 'NAME')" == "" ]; do
  sleep 1
done

echo "Pulling model... $BASE_MODEL"
ollama pull $BASE_MODEL

kill $OLLAMA_PID