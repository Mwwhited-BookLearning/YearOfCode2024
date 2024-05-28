
docker run -d ^
-v ollama:/root/.ollama ^
-p 11434:11434 ^
--name ollama ^
--gpus all ^
ollama/ollama

REM docker exec -it ollama ollama run llama2