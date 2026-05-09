<template>
  <q-page class="page-wrap">
    <!-- HEADER -->
    <div class="header">
      <div class="block-container row items-center no-wrap">
        <div class="header-left">
          <div class="order-icon">
            <q-icon name="shopping_cart" size="1.8em" color="white" />
          </div>
          <div class="order-title">{{ orderId ? `ORDER # ${orderId}` : 'NEW ORDER' }}</div>
        </div>

        <div class="header-actions row items-center no-wrap">
          <span>|</span>
          <q-btn
            v-if="orderId"
            flat
            size="sm"
            class="btn-outline"
            label="NEW"
            @click="clearOrder"
          />
          <q-btn flat size="sm" class="btn-outline" label="Save" @click="saveOrder" />
          <q-btn flat size="sm" label="Cancel" class="btn-danger btn-outline" to="/" />
        </div>
      </div>

      <q-btn label="GENERATE INVOICE" class="btn-dark" />
    </div>

    <!-- ORDER INFO -->
    <div class="block-container">
      <div class="block-container-title">ORDER INFORMATION</div>

      <div class="grid">
        <!-- CUSTOMER -->
        <q-select
          v-model="form.customer"
          :options="customers"
          label="Customer"
          outlined
          dense
          emit-value
          map-options
        />

        <!-- ADDRESS -->
        <q-input
          v-model="form.address"
          label="Shipping Address"
          outlined
          dense
          @blur="validateAddress"
        >
          <template #append>
            <q-icon v-if="addressStatus === true" name="check_circle" color="green" />
            <q-icon v-else-if="addressStatus === false" name="cancel" color="red" />
          </template>
        </q-input>

        <!-- DATE -->
        <q-input v-model="form.date" label="Order Date" outlined dense readonly>
          <template #append>
            <q-icon name="event" class="cursor-pointer">
              <q-popup-proxy cover transition-show="scale" transition-hide="scale">
                <q-date v-model="form.date" mask="YYYY-MM-DD">
                  <div class="row items-center justify-end q-gutter-sm q-pa-sm">
                    <q-btn label="OK" color="primary" flat v-close-popup />
                  </div>
                </q-date>
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>

        <!-- MANAGER -->
        <q-select
          v-model="form.manager"
          :options="managers"
          label="Account Manager"
          outlined
          dense
          emit-value
          map-options
        />
      </div>
      <div class="grid-4 q-mt-sm">
        <!-- SHIPPER -->
        <q-select
          v-model="form.shipper"
          :options="shippers"
          label="Shipper"
          outlined
          dense
          emit-value
          map-options
          @update:model-value="onShipperChange"
        />

        <!-- SHIPPER DETAILS -->
        <q-input v-model="shipperDetails.id" label="Shipper ID" outlined dense disable />
        <q-input v-model="shipperDetails.name" label="Company Name" outlined dense disable />
        <q-input v-model="shipperDetails.phone" label="Phone" outlined dense disable />
      </div>
    </div>

    <!-- LINE ITEMS -->
    <div class="row justify-between items-center q-mt-lg q-mb-md">
      <div class="row items-center">
        <span class="item-line-title">Line Items</span>
        <span class="item-count font-bold">{{ items.length }} ITEMS</span>
      </div>

      <div class="row q-gutter-sm">
        <q-btn size="sm" flat label="ADD LINE" class="btn-outline" @click="addLine" />
        <q-btn size="sm" label="DELETE SELECTED" flat class="btn-dark" @click="deleteSelected" />
      </div>
    </div>

    <div class="block-container">
      <table class="custom-table">
        <thead>
          <tr class="header-row">
            <th class="center th-checkbox" style="width: 40px">
              <input type="checkbox" v-model="selectAll" @change="toggleAll" />
            </th>
            <th style="width: 400px; text-align: left; padding-left: 20px">PRODUCT</th>
            <th class="center th-desc">DESCRIPTION</th>
            <th class="center th-qty">QUANTITY</th>
            <th class="center th-unit">UNIT PRICE</th>
            <th class="center th-ext">EXT. TOTAL</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="(item, index) in items" :key="index" class="line-item-row">
            <td class="center td-checkbox">
              <input type="checkbox" v-model="selected" :value="index" />
            </td>

            <td class="center td-product">
              <q-select
                v-model="item.productId"
                :options="products"
                dense
                outlined
                emit-value
                map-options
                @update:model-value="(val) => updateProduct(index, val)"
              />
            </td>

            <td class="center td-desc">
              <div class="item-sub">{{ item.desc }}</div>
            </td>

            <td class="center td-qty">
              <input class="qty-input" type="number" v-model.number="item.qty" />
            </td>

            <td class="center td-unit">${{ item.price.toFixed(2) }}</td>
            <td class="center td-total total-cell">${{ (item.qty * item.price).toFixed(2) }}</td>
          </tr>
        </tbody>
      </table>

      <div class="total-row">
        ORDER TOTAL <span>${{ total }}</span>
      </div>
    </div>

    <!-- LOGISTICS -->
    <div class="block-container q-mt-lg">
      <div class="row justify-between items-center q-mb-md">
        <div class="block-container-title">VALIDATED LOGISTICS ADDRESS</div>

        <div
          class="verified"
          :class="{
            'verified-ok': addressStatus === true,
            'verified-bad': addressStatus === false,
          }"
        >
          {{
            addressStatus === true ? 'VERIFIED' : addressStatus === false ? 'UNVERIFIED' : 'PENDING'
          }}
        </div>
      </div>

      <div class="grid-3">
        <q-input v-model="logistics.street" label="Street" outlined dense />
        <q-input v-model="logistics.city" label="City" outlined dense />
        <q-input v-model="logistics.state" label="State/Prov." outlined dense />
        <q-input v-model="logistics.postal" label="Postal Code" outlined dense />
        <q-input v-model="logistics.country" label="Country" outlined dense />
        <q-input v-model="logistics.gps" label="GPS Coordinates" outlined dense />
      </div>

      <div class="map-box">
        <MapDisplay :result="logistics" />
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { useRoute } from 'vue-router'
import { watch } from 'vue'
import useCreateOrder from '../js/createOrder.js'
import '../css/createOrder.css'
import MapDisplay from '../components/MapDisplay.vue'

const route = useRoute()
const initialId = route.query.id || null
// normalize productIDs from query (may be string or array)
const rawProductIDs = route.query.productIDs || null
const initialProductIDs = rawProductIDs
  ? Array.isArray(rawProductIDs)
    ? rawProductIDs
    : [rawProductIDs]
  : null

const {
  customers,
  managers,
  products,
  shippers,
  addressStatus,
  form,
  shipperDetails,
  onShipperChange,
  validateAddress,
  saveOrder,
  loadOrder,
  orderId,
  clearOrder,
  items,
  addLine,
  updateProduct,
  selected,
  selectAll,
  toggleAll,
  deleteSelected,
  total,
  logistics,
} = useCreateOrder(initialId, initialProductIDs)

// react to query id changes when navigating from order list
watch(
  () => route.query.id,
  (val) => {
    if (val) {
      orderId.value = val
      loadOrder(val)
    }
  },
)
</script>
