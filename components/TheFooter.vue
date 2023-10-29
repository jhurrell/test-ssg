<template>
  <footer class="mt-auto bg-gray-400 p-2 text-sm">
    <div class="m-1 flex flex-col items-center justify-between">
      <div>&copy;{{ new Date().getFullYear() }} {{ useState.siteName }}</div>
      <div>
        <a href="`tel:${useState.telephone}`">{{ useState.telephone }}</a>
      </div>      
    </div>
  </footer>
</template>

<script lang="ts" setup>
import { useStateStore } from '@/stores/state'
const useState = useStateStore()
</script>
