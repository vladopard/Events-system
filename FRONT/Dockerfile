# front/Dockerfile

# build stage
FROM node:20 AS builder
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# production stage
FROM nginx:stable-alpine
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80
