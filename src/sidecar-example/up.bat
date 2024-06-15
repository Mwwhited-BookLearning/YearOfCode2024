
@REM docker compose --project-name sidecar-example --file docker-compose.yml up --force-recreate

@REM --detach 

docker build --tag oobdev/sidecar-example  --file .\DockerFile .
docker run ^
--rm --interactive --tty ^
--name sidecar ^
--publish 8080:80 ^
--publish 8085:5000 ^
--publish 11434:11434 ^
--gpus all ^
oobdev/sidecar-example
