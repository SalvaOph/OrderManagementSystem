<!-- src/pages/OrderDetails.vue -->

<script setup>
import useOrderDetails from '../js/orderDetails.js'
import '../css/orderDetails.css'

const {
  columns,
  rows,
  exportToExcel,
  exportToPDF,
  pagination,
  getInitials,

  // REGION
  selectedRegions,
  regionOptions,

  // DATE
  selectedYear,
  selectedMonth,
  selectedWeek,

  yearOptions,
  monthOptions,
  weekOptions,
} = useOrderDetails()
</script>

<template>
  <div class="orders-card q-pa-md">
    <!-- HEADER -->
    <div class="row items-center justify-between q-mb-md">
      <div class="row q-gutter-lg">
        <!-- TITLE -->
        <div class="text-h5 text-bold">Order Details</div>

        <!-- TABS -->
        <div class="row q-gutter-xs">
          <q-btn flat label="All Orders" size="sm" class="tab-btn active" />
          <q-btn flat label="Delayed" size="sm" class="tab-btn" />
          <q-btn flat label="Priority" size="sm" class="tab-btn" />
        </div>
      </div>

      <div class="row q-gutter-md items-center">
        <!-- FILTERS -->
        <div class="row table-action">
          <!-- DATE FILTER -->
          <q-btn-dropdown
            class="table-action-btn btn-no-round"
            rounded="false"
            label="Date Filter"
            icon="event"
            flat
          >
            <div class="q-pa-md row column q-gutter-md" style="min-width: 260px">
              <!-- YEAR -->
              <q-select
                v-model="selectedYear"
                :options="yearOptions"
                label="Year"
                outlined
                dense
                clearable
              />

              <!-- MONTH -->
              <q-select
                v-model="selectedMonth"
                :options="monthOptions"
                option-label="label"
                option-value="value"
                emit-value
                map-options
                label="Month"
                outlined
                dense
                clearable
                :disable="!selectedYear"
              />

              <!-- WEEK -->
              <q-select
                v-model="selectedWeek"
                :options="weekOptions"
                option-label="label"
                option-value="value"
                emit-value
                map-options
                label="Week"
                outlined
                dense
                clearable
                :disable="selectedMonth === null"
              />

              <!-- CLEAR -->
              <q-btn
                flat
                color="primary"
                label="Clear Filters"
                @click="((selectedYear = null), (selectedMonth = null), (selectedWeek = null))"
              />
            </div>
          </q-btn-dropdown>

          <!-- REGION FILTER -->
          <q-btn-dropdown
            class="table-action-btn btn-no-round"
            icon="filter_list"
            rounded="false"
            label="Regions"
            flat
          >
            <div class="q-pa-md" style="min-width: 250px">
              <q-option-group
                v-model="selectedRegions"
                :options="
                  regionOptions.map((r) => ({
                    label: r,
                    value: r,
                  }))
                "
                type="checkbox"
              />
            </div>
          </q-btn-dropdown>
        </div>

        <div class="separator"></div>

        <div class="row">
          <!-- EXPORT -->
          <q-btn
            class="table-action table-action-btn q-mr-sm"
            flat
            label="Export to Excel"
            icon="download"
            @click="exportToExcel"
          />

          <q-btn
            class="table-action table-action-btn"
            flat
            label="Export to PDF"
            icon="picture_as_pdf"
            @click="exportToPDF"
          />
        </div>
      </div>
    </div>

    <!-- TABLE -->
    <q-table
      :rows="rows"
      :columns="columns"
      row-key="orderID"
      flat
      bordered
      class="custom-table q-table--striped"
      v-model:pagination="pagination"
    >
      <!-- HEADER -->
      <template v-slot:header-cell="props">
        <q-th :props="props" class="th-left">
          {{ (props.header && props.header.label) || (props.col && props.col.label) || '' }}
        </q-th>
      </template>

      <!-- CUSTOMER -->
      <template v-slot:body-cell-customer="props">
        <q-td :props="props" class="td-left">
          <div class="row items-center">
            <div class="avatar">
              {{ getInitials(props.row.customerName) }}
            </div>

            <div class="q-ml-sm text-weight-medium">
              {{ props.row.customerName }}
            </div>
          </div>
        </q-td>
      </template>

      <!-- DATE -->
      <template v-slot:body-cell-orderDate="props">
        <q-td :props="props" class="td-left">
          <span class="text-grey-6">
            {{
              new Date(props.row.orderDate).toLocaleDateString('en-US', {
                month: 'long',
                year: 'numeric',
              })
            }}
          </span>
        </q-td>
      </template>

      <!-- PRODUCTS -->
      <template v-slot:body-cell-products="props">
        <q-td :props="props" class="td-left">
          <div>
            {{ props.row.products?.length || 0 }}
            Items

            <span class="text-grey-6">
              (
              {{ props.row.products?.slice(-2).join(', ') }}
              )
            </span>
          </div>
        </q-td>
      </template>

      <!-- REGION -->
      <template v-slot:body-cell-region="props">
        <q-td :props="props" class="td-left">
          <div class="row items-center">
            <span class="dot"></span>

            <span class="q-ml-xs">
              {{ props.row.region || 'Unknown' }}
            </span>
          </div>
        </q-td>
      </template>

      <!-- STATUS -->
      <template v-slot:body-cell-status="props">
        <q-td :props="props" class="td-left">
          <span :class="['status-badge', (props.row.status || 'unknown').toLowerCase()]">
            {{ props.row.status || 'Unknown' }}
          </span>
        </q-td>
      </template>

      <!-- TOTAL -->
      <template v-slot:body-cell-total="props">
        <q-td :props="props" class="td-left text-weight-medium">
          ${{ (props.row.totalAmount || 0).toLocaleString() }}
        </q-td>
      </template>

      <!-- ACTION -->
      <template v-slot:body-cell-action="props">
        <q-td :props="props" class="td-left">
          <q-btn
            flat
            dense
            round
            icon="chevron_right"
            :to="{
              path: '/create-order',
              query: { id: props.row.orderID },
            }"
          />
        </q-td>
      </template>
    </q-table>
  </div>
</template>
