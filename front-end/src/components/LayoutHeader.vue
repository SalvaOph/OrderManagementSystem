<template>
  <q-header class="bg-white q-py-sm text-grey-9 header container">
    <q-toolbar class="q-px-lg">
      <!-- Logo -->
      <div>
        <!-- <router-link
          flat
          to="/"
          class="text-h6 text-weight-bold text-primary"
          label=""
        > -->
        <router-link class="text-h6 header-title" to="/"> Northwind Traders </router-link>
      </div>

      <!-- Navigation -->
      <q-tabs
        v-model="tab"
        dense
        class="q-ml-xl text-grey-7"
        active-color="primary"
        indicator-color="primary"
      >
        <q-tab name="dashboard" label="Dashboard" />
        <q-tab name="shipments" label="Shipments" />
        <q-tab name="inventory" label="Inventory" />
        <q-tab name="analytics" label="Analytics" />
        <q-tab name="fleet" label="Fleet" />
      </q-tabs>

      <q-space />

      <!-- Search -->
      <q-input dense outlined placeholder="Quick search manifests..." class="search-input q-mr-md">
        <template v-slot:prepend>
          <q-icon name="search" />
        </template>
      </q-input>

      <!-- Icons -->
      <q-btn flat round dense icon="notifications" />
      <q-btn flat round dense icon="settings" />
      <q-avatar size="32px" class="q-ml-sm">
        <img src="https://i.pravatar.cc/40" />
      </q-avatar>
    </q-toolbar>
  </q-header>
</template>

<script>
// import { ref } from 'vue'
// const tab = ref('shipments')

export default {
  name: 'LayoutHeader',
}
</script>

<style scoped>
.header {
  position: sticky;
  /* border-bottom: 1px solid #dcdcdc; */
}

.header-title {
  color: #0d3b66;
  font-weight: 800;
  text-decoration: none;
}

.search-input {
  width: 260px;
}
</style>
