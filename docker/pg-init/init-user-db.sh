#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER sensor_entry WITH PASSWORD 'sens0r';
    -- # CREATE DATABASE sensordb;
    GRANT ALL PRIVILEGES ON DATABASE sensordb TO sensor_entry;
    CREATE TABLE IF NOT EXISTS bme688sensor  (
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
EOSQL
