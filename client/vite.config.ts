import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import mkcert from "vite-plugin-mkcert";
import tailwindcss from '@tailwindcss/vite';

// https://vite.dev/config/
export default defineConfig({
  build: {
    outDir: '../API/wwwroot',
  },
  server: {
    port: 3000,
  },
  plugins: [
    react(), 
    mkcert(),
    tailwindcss()
  ],
})
