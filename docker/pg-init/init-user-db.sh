#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER $SENSOR_DB_USER WITH PASSWORD '$SENSOR_DB_PASS';
    -- # CREATE DATABASE sensordb;
    GRANT ALL PRIVILEGES ON DATABASE $POSTGRES_DB TO $SENSOR_DB_USER;
    CREATE TABLE IF NOT EXISTS $SENSOR_DB_TABLE  (
        serial SERIAL PRIMARY KEY,
        datetime TIMESTAMP NOT NULL,
        sensor_id SMALLINT NOT NULL,
        category VARCHAR(128) NOT NULL,
        gas_index SMALLINT NOT NULL,
        temperature REAL NOT NULL,
        humidity REAL NOT NULL,
        pressure REAL NOT NULL,
        gas_registance REAL NOT NULL,
        gas_registance_log REAL NOT NULL,
        comment VARCHAR(128)
    );
    GRANT ALL PRIVILEGES ON $SENSOR_DB_TABLE TO $SENSOR_DB_USER;
EOSQL
