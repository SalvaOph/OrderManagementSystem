<template>
  <q-page padding>
    <q-input v-model="address" label="Enter address" outlined />

    <q-btn label="Validate Address" color="primary" @click="validateAddress" class="q-mt-md" />

    <div v-if="result" class="q-mt-md">
      <p><strong>Formatted:</strong> {{ result.formattedAddress }}</p>
      <p><strong>Latitude:</strong> {{ result.latitude }}</p>
      <p><strong>Longitude:</strong> {{ result.longitude }}</p>
    </div>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import axios from 'axios'

const address = ref('')
const result = ref(null)

const validateAddress = async () => {
  try {
    const res = await axios.post('http://localhost:5000/api/address/validate', {
      address: address.value,
    })

    result.value = res.data
  } catch (error) {
    console.error('Validation failed', error)
  }
}
</script>
