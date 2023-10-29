<template>
  <div class="h-screen w-screen bg-slate-100 flex flex-col">
    <TheHeader />
    <section id="MainSection" class="h-screen p-2 bg-gray-1 00">
      <slot></slot>
    </section>
    <TheFooter />
  </div>
</template>

<script setup>
</script>
