# syntax=docker/dockerfile:1

FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
ARG API_BASE_URL=http://localhost:8080
RUN printf "export const environment = {\n  production: true,\n  apiBaseUrl: '%s',\n};\n" "$API_BASE_URL" \
    > src/environments/environment.ts
RUN npm run build

FROM nginx:1.27-alpine AS runtime
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/vehicle-explorer/browser /usr/share/nginx/html
EXPOSE 80
