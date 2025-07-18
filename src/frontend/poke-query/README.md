
# PokeQuery React SPA

This is the React + TypeScript rewrite of the original Svelte SPA. It provides a fast, modern UI for querying Pokémon data via a RESTful API, matching the original design and feature set.

## Tech Stack

- [React](https://react.dev/) — UI library for building interactive interfaces
- [TypeScript](https://www.typescriptlang.org/) — Typed JavaScript for safer code
- [Vite](https://vitejs.dev/) — Lightning-fast build tool and dev server

## Setup Instructions

1. Ensure you have [Node.js](https://nodejs.org/) and [npm](https://docs.npmjs.com/) installed.
2. Clone the repository and navigate to this directory:
   ```powershell
   git clone https://github.com/lwebb-dev/PokeQuery.git
   cd PokeQuery/src/frontend/poke-query
   ```
3. Install dependencies:
   ```powershell
   npm install
   ```

## Build and Start

### Development

Start the development server:
```powershell
npm run dev
```
The app will be available at [http://localhost:5173](http://localhost:5173).

### Production Build

Build the app for production:
```powershell
npm run build
```
Preview the production build locally:
```powershell
npm run preview
```

## API

This app queries the same RESTful API endpoints as the original Svelte app. See the [root README](../../../../README.md) for backend setup and API details.

