#!/bin/sh

BASE_DIR=$(dirname $(pwd))

echo "up-ing firefly"
docker compose -f $BASE_DIR/finance/firefly.yml up -d

echo "waiting for docker container image pulls"
sleep 180

echo "restore backup data"
bash $BASE_DIR/finance/backuper.sh restore 2024-06-09.tar
