CREATE TABLE regions (
    region_id SERIAL PRIMARY KEY,
    region_name CHARACTER VARYING (25)
);
 
CREATE TABLE countries (
    country_id CHARACTER (2) PRIMARY KEY,
    country_name CHARACTER VARYING (40),
    region_id INTEGER NOT NULL,
    FOREIGN KEY (region_id) REFERENCES regions (region_id)
);
 
CREATE TABLE locations (
    location_id SERIAL PRIMARY KEY,
    street_address CHARACTER VARYING (40),
    postal_code CHARACTER VARYING (12),
    city CHARACTER VARYING (30) NOT NULL,
    state_province CHARACTER VARYING (25),
    country_id CHARACTER (2) NOT NULL,
    FOREIGN KEY (country_id) REFERENCES countries (country_id)
);

-- Table: ctl.esquemas_tablas_databricks

-- DROP TABLE ctl.esquemas_tablas_databricks;

CREATE TABLE IF NOT EXISTS ctl.esquemas_tablas_databricks
(
    nombre_tabla character varying(250) COLLATE pg_catalog."default" NOT NULL,
    ddl character varying COLLATE pg_catalog."default" NOT NULL,
    columnas_particion character varying COLLATE pg_catalog."default",
    columnas_zorder character varying COLLATE pg_catalog."default",
    ruta_wasb_destino character varying COLLATE pg_catalog."default" NOT NULL,
    separador_columna character varying COLLATE pg_catalog."default" NOT NULL,
    archivo_contiene_header boolean NOT NULL,
    codificacion_archivo character varying COLLATE pg_catalog."default" NOT NULL,
    formato_destino character varying COLLATE pg_catalog."default" NOT NULL,
    modo_escritura character varying COLLATE pg_catalog."default" NOT NULL,
    idsubsistemaorigen integer,
    columnas_particion_calculadas character varying(500) COLLATE pg_catalog."default",
    nombre_bd_parquet character varying(255) COLLATE pg_catalog."default",
    aplicar_optimizer boolean
);
    ALTER TABLE ctl.esquemas_tablas_databricks
    OWNER to "Sqladmindwh";


-- Table: ctl.log_csvparquet

-- DROP TABLE ctl.log_csvparquet;

CREATE TABLE IF NOT EXISTS ctl.log_csvparquet
(
    id integer NOT NULL DEFAULT nextval('ctl.log_csvparquet_id_seq'::regclass),
    aplicativo character varying(8000) COLLATE pg_catalog."default",
    insercion text COLLATE pg_catalog."default",
    nivel character varying(8000) COLLATE pg_catalog."default",
    mensaje text COLLATE pg_catalog."default",
    origen text COLLATE pg_catalog."default"
);
    ALTER TABLE ctl.log_csvparquet
    OWNER to "Sqladmindwh";
