
docker build ^
--tag eliassen/sbert ^
--file DockerFile.sbert ^
.

docker run --rm -it ^
-p 5080:5000 ^
--gpus all ^
oobdev/sbert
