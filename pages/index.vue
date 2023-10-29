<template>
  <head>
    <Title>{{ data?.title }}</Title>
    <Meta name="description" :content="data?.description" />
    <Meta name="keywords" :content="data?.keywords" />
  </head>
  <div>
    <ContentDoc />
    <p>public.siteEnvironment: {{ config.public.siteEnvironment }}</p>
    <p>public.supportEmail: {{ config.public.supportEmail }}</p>
    <p>apiSecret: {{ config.apiSecret }} (should not be available)</p>
    <p>app.hello: {{ config.app.hello }}</p>

  </div>
</template>

<script setup>
const { data } = await useAsyncData('index', () => queryContent('/').findOne())
const config = useRuntimeConfig()
</script>
