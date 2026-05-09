<template>
  <GoogleMap
    v-if="result && result.latitude != null && result.longitude != null"
    :center="{ lat: result.latitude, lng: result.longitude }"
    :zoom="15"
    style="width: 100%; height: 300px"
  >
    <Marker :options="{ position: { lat: result.latitude, lng: result.longitude } }" />
  </GoogleMap>
</template>

<script setup>
import { GoogleMap, Marker } from 'vue3-google-map'
import { defineProps } from 'vue'

defineProps({
  result: {
    type: Object,
    default: null,
  },
})
</script>
