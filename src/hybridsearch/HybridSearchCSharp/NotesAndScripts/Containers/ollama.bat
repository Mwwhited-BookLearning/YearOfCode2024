

docker run -d ^
--gpus=all ^
-v ollama:/root/.ollama ^
-p 11434:11434 ^
--name ollama ^
ollama/ollama


docker run ^
--rm ^
-p 3000:8080 ^
-e OLLAMA_API_BASE_URL=http://192.168.1.170:11434/api ^
-v open-webui:/app/backend/data ^
--name open-webui ^
ghcr.io/open-webui/open-webui:main