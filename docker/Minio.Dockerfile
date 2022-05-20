FROM quay.io/minio/minio

ENTRYPOINT ["/bin/bash", " /usr/bin/mc alias set myminio http://minio:9000 minio minio123; \
                            /usr/bin/mc mb myminio/cfa; \
                            /usr/bin/mc policy set public myminio/cfa; \
                            exit 0;"]
