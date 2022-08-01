FROM debian

ENV REPO_DATA=/api-data/data/api/v2

RUN apt-get update \ 
    && apt-get install -y git

COPY ./get-data.sh /
CMD ["bash", "get-data.sh"]
