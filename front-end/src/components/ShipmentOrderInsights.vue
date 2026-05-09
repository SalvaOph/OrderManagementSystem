<template>
  <div class="q-mb-lg shipment-order-insights">
    <!-- Title -->
    <div class="row items-center justify-between">
      <div>
        <div class="text-h5 text-weight-bold">Shipment & Order Insights</div>
        <div class="text-grey-6 q-mb-md">
          Global logistics monitoring and regional performance analytics
        </div>
      </div>

      <div class="row items-center no-wrap">
        <q-btn size="md" outline label="Generate Branded Report" class="q-mr-sm" />
        <q-btn size="md" color="dark" label="Create Order" to="/create-order" />
      </div>
    </div>

    <div class="row q-col-gutter-lg items-stretch">
      <!-- BAR CHART -->
      <div class="col-8 full-height">
        <q-card flat bordered class="q-pa-md full-height">
          <div class="row items-center justify-between">
            <div class="text-subtitle1 text-weight-medium">Orders Over Time</div>

            <q-select
              dense
              outlined
              v-model="year"
              :options="years"
              label="Year"
              style="width: 100px"
            />
          </div>

          <apexchart
            type="bar"
            height="260"
            :options="barOptions"
            :series="barSeries"
            class="q-mt-lg"
          />
        </q-card>
      </div>

      <!-- DONUT CHART -->
      <div class="col-4 full-height">
        <q-card flat bordered class="q-pa-md full-height">
          <div class="row justify-between items-center">
            <div class="text-subtitle1 text-weight-medium">Shipments by Region</div>
            <q-badge color="primary" outline>Current Status</q-badge>
          </div>

          <apexchart type="donut" height="260" :options="donutOptions" :series="donutSeries" />

          <q-separator class="q-my-md" />

          <div class="text-caption text-grey-7">Western Europe Leading</div>
          <div class="text-positive text-weight-medium">+14.2% Growth vs Last Mo.</div>
        </q-card>
      </div>
    </div>
  </div>
</template>

<script setup>
import useShipmentOrderInsights from '../js/shipmentOrderInsights.js'
import '../css/shipmentOrderInsights.css'

const { year, years, barSeries, barOptions, donutSeries, donutOptions } = useShipmentOrderInsights()
</script>
