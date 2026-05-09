<template>
  <div class="container">
    <ShipmentOrderInsights />
    <OrderDetails />
  </div>
</template>

<script>
import OrderDetails from 'src/components/OrderDetails.vue'
import ShipmentOrderInsights from 'src/components/ShipmentOrderInsights.vue'

export default {
  components: {
    OrderDetails,
    ShipmentOrderInsights,
  },
}
</script>

<style></style>
