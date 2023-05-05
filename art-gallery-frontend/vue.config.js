const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    proxy: {
      "^/api": {
        target: "http://host.docker.internal:7194",
        changeOrigin: true,
        secure: false,
        pathRewrite: { "^/api": "/api" },
      },
    },
  },
});
