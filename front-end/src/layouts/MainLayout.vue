<template>
  <q-layout class="layout" view="lHh Lpr lFf">
    <LayoutHeader />

    <q-page-container class="page-container">
      <router-view />
    </q-page-container>

    <LayoutFooter />
  </q-layout>
</template>

<script setup>
import LayoutHeader from 'src/components/LayoutHeader.vue'
import LayoutFooter from 'src/components/LayoutFooter.vue'
</script>

<style>
.page-container {
  padding-top: 0 !important;
}
</style>
