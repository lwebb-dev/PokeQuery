FROM node:14 AS build

WORKDIR /app

COPY package.json .
COPY package-lock.json .
RUN npm install

COPY . .
RUN npm run build

EXPOSE 80

FROM nginx:1.23-alpine
COPY --from=build /app/dist /usr/share/nginx/html
