FROM postgres:15

EXPOSE 5432

COPY db_init.sql /docker-entrypoint-initdb.d/db_init.sql

ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=123456
