<template>
  <head>
    <Title>Azure Tutorial</Title>
    <Meta name="description" content="Following a tutorial that eventually will teach how to code and deploy an Azure Function" />
    <Meta name="keywords" content="youtube, tutorial, azure, function, nuxt" />
  </head>
  <div>
    <h1>Home</h1>
    <p>public.siteEnvironment: {{ config.public.siteEnvironment }}</p>
    <p>public.supportEmail: {{ config.public.supportEmail }}</p>
    <p>apiSecret: {{ config.apiSecret }} (should not be available)</p>
    <p>app.hello: {{ config.app.hello }}</p>
  </div>
</template>

<script setup>
const config = useRuntimeConfig()
</script>
