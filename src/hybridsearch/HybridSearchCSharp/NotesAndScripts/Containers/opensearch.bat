
docker run ^
-it ^
-p 9200:9200 ^
-p 9600:9600 ^
-e OPENSEARCH_INITIAL_ADMIN_PASSWORD=%OPENSEARCH_ADMIN_PASSWORD% ^
-e "discovery.type=single-node" ^
-e "plugins.security.disabled=true" ^
--name opensearch ^
opensearchproject/opensearch:latest