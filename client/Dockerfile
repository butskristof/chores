# build Vue app as SPA
FROM node:lts-hydrogen AS build
WORKDIR /app
RUN chown node:node ./
USER node

COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# set up nginx server
FROM nginx:stable-alpine
RUN apk add --no-cache bash
COPY docker/nginx.conf /etc/nginx/nginx.conf
WORKDIR /usr/share/nginx

COPY --from=build /app/dist /usr/share/nginx/html

# set env vars on container start
COPY docker/entrypoint.sh .
COPY docker/create_config_js.sh .
COPY .env .

ENTRYPOINT ["/usr/share/nginx/entrypoint.sh"]

CMD ["nginx", "-g", "daemon off;"]
