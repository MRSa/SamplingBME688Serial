FROM python:slim-bullseye

ENV PREREQUISITES "bash libpq-dev python3-pip"

RUN apt-get update && \
    apt-get install -y $PREREQUISITES && \
    pip3 install --upgrade pip && \
    pip3 install sqlalchemy && \
    pip3 install flask && \
    pip3 install flask_sqlalchemy && \
    pip3 install flask_migrate && \
    pip3 install flask-cors && \
    pip3 install psycopg2 && \
    #pip3 install pandas  && \
    #pip3 install sklearn && \
    #pip3 install matplotlib && \
    mkdir -p /work

COPY ./web-service.py /work/web-service.py
COPY ./StorageAdapter.py /work/StorageAdapter.py
COPY ./sensordata.py /work/sensordata.py


RUN ls -l /work

WORKDIR /work
EXPOSE 3010

##### ----- for debug entry point
#ENTRYPOINT ["tail", "-f", "/dev/null"]

#### -----
ENTRYPOINT ["python3"]
CMD ["/work/web-service.py"]
