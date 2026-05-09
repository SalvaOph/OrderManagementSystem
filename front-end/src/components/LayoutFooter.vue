<template>
  <q-footer class="bg-white text-grey-7 q-pa-md footer container">
    <div class="row justify-between items-center">
      <div>
        <div class="text-weight-medium">Northwind Traders</div>
        <div class="text-caption">
          © 2024 Northwind Traders Global Logistics. Precision in Motion.
        </div>
      </div>

      <div class="row q-gutter-md text-caption">
        <a href="#">Privacy Policy</a>
        <a href="#">Terms of Service</a>
        <a href="#">API Documentation</a>
        <a href="#">Support</a>
      </div>
    </div>
  </q-footer>
</template>

<script>
export default {
  name: 'LayoutFooter',
}
</script>

<style scoped>
.footer {
  position: relative;
  padding: 24px;
  border-top: 1px solid #ccc;
}

a {
  text-decoration: none;
  color: #6b7280;
}

a:hover {
  text-decoration: underline;
}
</style>
