FROM python:slim-bullseye

ENV PREREQUISITES "bash python3-pip"

RUN apt-get update && \
    apt-get install -y $PREREQUISITES && \
    pip3 install --upgrade pip && \
    mkdir -p /work

COPY ./web-service.py /work/web-service.py
WORKDIR /work
EXPOSE 3010

#### ----- for debug entry point
ENTRYPOINT ["tail", "-f", "/dev/null"]

##### -----
#ENTRYPOINT ["python3"]
#CMD ["/work/web-service.py"]
