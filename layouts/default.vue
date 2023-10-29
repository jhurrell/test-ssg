<template>
  <div class="h-screen w-screen bg-slate-100 flex flex-col">
    <TheHeader />
    <section id="MainSection" class="p-2">
      <slot></slot>
    </section>
    <TheFooter />
  </div>
</template>

<script lang="ts">
</script>
