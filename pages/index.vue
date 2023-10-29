<template>
  <head>
    <Title>{{ data?.title }}</Title>
    <Meta name="description" :content="data?.description" />
    <Meta name="keywords" :content="data?.keywords" />
  </head>
  <div>
    <ContentDoc />    
  </div>
</template>

<script setup>
const { data } = await useAsyncData('index', () => queryContent('/').findOne())
</script>
