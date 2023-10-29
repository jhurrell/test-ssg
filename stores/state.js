import { defineStore } from 'pinia'

export const useStateStore = defineStore('siteSettings', () => {
  const siteName = ref("My Fancy Website")
  const telephone = ref('585.555.1212')

  return { siteName, telephone }
})
