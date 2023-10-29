<template>
  <div class="bg-gradient-to-t from-blue-200 to-blue-300 sticky">
    <nav class="h-full text-gray-500 text-sm">
      <ul class="flex flex-row items-center justify-center gap-4">
        <NuxtLink 
          to="/" 
          activeClass="bg-blue-300 text-white" 
          class="hover:bg-blue-300 hover:text-white py-2 px-4"
        >
          home
        </NuxtLink>
        <NuxtLink 
          to="/my-team" 
          activeClass="bg-blue-300 text-white" 
          class="hover:bg-blue-300 hover:text-white py-2 px-4"
        >
          the team
        </NuxtLink>
        <NuxtLink 
          to="/contact-us" 
          activeClass="bg-blue-300 text-white" 
          class="hover:bg-blue-300 hover:text-white py-2 px-4"
        >
          contact us
        </NuxtLink>
      </ul>
    </nav>
  </div>
</template>

<script lang="ts" setup>

</script>
