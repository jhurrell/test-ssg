// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
	css: [
		'~/assets/css/tailwind.css',
		'@fortawesome/fontawesome-svg-core/styles.css',
	],
	ssr: false,
	devtools: { enabled: false },
	modules: [
		// ...
		'@pinia/nuxt',
		'@nuxt/content',
	],
	postcss: {
		plugins: {
			tailwindcss: {},
			autoprefixer: {},
		},
	},
	runtimeConfig: {
		apiSecret: "123456",
		public: {
			siteEnvironment: process.env.SITE_ENVIRONMENT,
			supportEmail: process.env.SUPPORT_EMAIL,
		},
		app: {
			hello: "World",
		}
	},
	app: {
		head: {
			title: "Azure Tutorial",
			meta: [
				{ charset: "utf-8" },
				{ name: "viewport", content: "width=device-width, initial-scale=1" },
				{ hid: "description", name: "description", content: "" },
				{ name: "format-detection", content: "telephone=no" },
			],
			script: [
			],
			link: [
				{ rel: "preconnect", href: "https://fonts.google.com" },
				{ rel: "preconnect", href: "https://fonts.gstatic.com" },
				{ rel: "stylesheet", href: "https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,200;0,300;0,400;0,500;0,600;1,100;1,200;1,300;1,400&display=swap" },
			]
		}
	},
})
