<template>
  <head>
    <Title>{{ data?.title }}</Title>
    <Meta name="description" :content="data?.description" />
    <Meta name="keywords" :content="data?.keywords" />
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
