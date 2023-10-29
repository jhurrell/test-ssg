const GlobalSettings = {
    development: {
        siteEnvironment: "Dev",
        supportEmail: "test-ssg+dev@gmail.com",
    },
    staging: {
        siteEnvironment: "Test",
        supportEmail: "test-ssg+test@gmail.com",
    },
    production: {
        siteEnvironment: "",
        supportEmail: "test-ssg@gmail.com",
    }
}

export { GlobalSettings }