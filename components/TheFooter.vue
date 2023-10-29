<template>
  <footer class="mt-auto bg-gray-400 p-2 text-sm">
    <div class="m-1 flex flex-col items-center justify-between">
      <div>&copy;{{ new Date().getFullYear() }} {{ useState.siteName }}</div>
      <div class="flex flex-row gap-2">
        <a :href="`tel:${useState.telephone}`">{{ useState.telephone }}</a>
        <a :href="`mailto:${config.public.supportEmail}`">Email us for support</a>
      </div>      
    </div>
  </footer>
</template>

<script lang="ts" setup>
import { useStateStore } from '@/stores/state'
const useState = useStateStore()
const config = useRuntimeConfig()
</script>
