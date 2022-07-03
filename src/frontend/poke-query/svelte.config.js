/** @type {import('@sveltejs/kit').Config} */
import sveltePreprocess from 'svelte-preprocess'
import dotenv from 'dotenv';

dotenv.config();

export default {
  // Consult https://github.com/sveltejs/svelte-preprocess
  // for more information about preprocessors
  preprocess: sveltePreprocess({
    replace: [
      ["process.env.API_BASE_URI", JSON.stringify(process.env.API_BASE_URI)]
    ]
  })
};
