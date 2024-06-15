
@REM docker compose --project-name sidecar-example --file docker-compose.yml up --force-recreate

@REM --detach 

docker build --tag oobdev/sidecar-example  --file .\DockerFile .
docker run ^
--rm --interactive --tty ^
--name sidecar ^
--publish 8080:80 ^
--gpus all ^
oobdev/sidecar-example
