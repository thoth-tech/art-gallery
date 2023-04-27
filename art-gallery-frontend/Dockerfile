FROM node:14.18.0-alpine3.14
WORKDIR /app
RUN npm install -g @vue/cli
COPY package*.json ./
RUN npm install --legacy-peer-deps
COPY . ./
RUN npm run build
EXPOSE 80
CMD ["npm", "run", "serve"]
