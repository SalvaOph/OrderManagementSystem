import { ref, onMounted, watch } from 'vue'
import { api } from 'src/boot/axios'
import pdfMake from 'pdfmake/build/pdfmake'
import pdfFonts from 'pdfmake/build/vfs_fonts'

import logo from 'src/assets/logo.png'

pdfMake.vfs = pdfFonts.vfs

export default function useOrderDetails() {
  const columns = ref([
    //{ name: 'orderID', label: 'ORDER ID', field: 'orderID', align: 'left' },
    { name: 'customer', label: 'CUSTOMER', field: 'customerName', align: 'left' },
    { name: 'orderDate', label: 'ORDER DATE', field: 'orderDate', align: 'left' },
    { name: 'products', label: 'PRODUCTS', field: 'products', align: 'left' },
    { name: 'region', label: 'REGION', field: 'region', align: 'left' },
    { name: 'status', label: 'STATUS', field: 'status', align: 'left' },
    { name: 'total', label: 'TOTALS', field: 'totalAmount', align: 'right' },
    { name: 'action', label: '', field: 'action', align: 'right' },
  ])

  const rows = ref([])
  const allRows = ref([])

  const pagination = ref({
    page: 1,
    rowsPerPage: 5,
  })

  // REGION FILTER
  const selectedRegions = ref([])
  const regionOptions = ref([])

  // DATE FILTER
  const selectedYear = ref(null)
  const selectedMonth = ref(null)
  const selectedWeek = ref(null)

  const yearOptions = ref([])

  const monthOptions = ref([
    { label: 'January', value: 0 },
    { label: 'February', value: 1 },
    { label: 'March', value: 2 },
    { label: 'April', value: 3 },
    { label: 'May', value: 4 },
    { label: 'June', value: 5 },
    { label: 'July', value: 6 },
    { label: 'August', value: 7 },
    { label: 'September', value: 8 },
    { label: 'October', value: 9 },
    { label: 'November', value: 10 },
    { label: 'December', value: 11 },
  ])

  const weekOptions = ref([
    { label: 'Week 1', value: 1 },
    { label: 'Week 2', value: 2 },
    { label: 'Week 3', value: 3 },
    { label: 'Week 4', value: 4 },
    { label: 'Week 5', value: 5 },
  ])

  // WEEK OF MONTH
  function getWeekOfMonth(date) {
    const firstDay = new Date(date.getFullYear(), date.getMonth(), 1)
    return Math.ceil((date.getDate() + firstDay.getDay()) / 7)
  }

  // APPLY FILTERS
  function applyFilters() {
    const filtered = allRows.value.filter((row) => {
      const orderDate = new Date(row.orderDate)

      // REGION FILTER
      if (selectedRegions.value.length > 0 && !selectedRegions.value.includes(row.region)) {
        return false
      }

      // YEAR FILTER
      if (selectedYear.value && orderDate.getFullYear() !== selectedYear.value) {
        return false
      }

      // MONTH FILTER
      if (selectedMonth.value !== null && orderDate.getMonth() !== selectedMonth.value) {
        return false
      }

      // WEEK FILTER
      if (selectedWeek.value && getWeekOfMonth(orderDate) !== selectedWeek.value) {
        return false
      }

      return true
    })

    rows.value = filtered
    pagination.value.page = 1
  }

  // WATCH FILTERS
  watch(
    [selectedRegions, selectedYear, selectedMonth, selectedWeek],
    () => {
      applyFilters()
    },
    { deep: true },
  )

  // FETCH ORDERS
  async function fetchRows() {
    try {
      const [ordersRes, productsRes] = await Promise.all([
        api.get('/order-details-view'),
        api.get('/products').catch(() => ({ data: [] })),
      ])

      const productsList = productsRes.data || []

      const productMap = new Map(productsList.map((p) => [p.productId, p.productName]))

      const formattedRows = (ordersRes.data || []).map((r) => {
        const rawProducts = Array.isArray(r.products) ? r.products : []

        const displayProducts = rawProducts.map((p) => {
          if (typeof p === 'string' || typeof p === 'number') {
            return productMap.get(p) || String(p)
          }

          return p.productName || ''
        })

        const productIDs = rawProducts
          .map((p) => {
            if (typeof p === 'string' || typeof p === 'number') {
              return p
            }

            return p.productId ?? null
          })
          .filter((x) => x != null)

        return {
          ...r,
          products: displayProducts,
          productIDs,
          totalAmount: r.totalAmount || 0,
          status: r.status || 'Unknown',
          region: r.region || 'Unknown',
        }
      })

      allRows.value = formattedRows
      rows.value = formattedRows

      regionOptions.value = [...new Set(formattedRows.map((r) => r.region).filter(Boolean))]

      yearOptions.value = [
        ...new Set(formattedRows.map((r) => new Date(r.orderDate).getFullYear())),
      ].sort((a, b) => b - a)
    } catch (error) {
      console.error('Error fetching order details:', error)
    }
  }

  function exportToExcel() {
    try {
      const companyName = 'Northwind Traders - Order Details Report'

      const header = [
        'Order ID',
        'Customer Name',
        'Customer ID',
        'Employee ID',
        'Ship Via',
        'Freight',
        'Ship Name',
        'Ship Address',
        'Ship City',
        'Ship Region',
        'Ship Postal Code',
        'Ship Country',
        'Order Date',
        'Required Date',
        'Shipped Date',
        'Products',
        'Region',
        'Total Amount',
      ]

      const lines = []
      lines.push([companyName, ...new Array(header.length - 1).fill('')])
      lines.push(new Array(header.length).fill(''))
      lines.push(header)

      function firstDefined(obj, keys) {
        for (const k of keys) {
          if (obj && obj[k] !== undefined && obj[k] !== null) return obj[k]
        }
        return null
      }

      rows.value.forEach((r) => {
        const productsArr = Array.isArray(r.products) ? r.products : []
        const productsText = productsArr.length
          ? `${productsArr.length} Items: ${productsArr.join(', ')}`
          : ''

        const rowValues = [
          firstDefined(r, ['orderID', 'orderId', '_id', 'id']) || '',
          firstDefined(r, ['customerName', 'customer']) || '',
          firstDefined(r, ['customerID', 'customerId', 'customer_id']) || '',
          firstDefined(r, ['employeeID', 'employeeId', 'employee_id']) || '',
          firstDefined(r, ['shipVia', 'ship_via', 'shipperID', 'shipperId']) || '',
          firstDefined(r, ['freight', 'Freight']) || '',
          firstDefined(r, ['shipName', 'ShipName']) || '',
          firstDefined(r, ['shipAddress', 'ShipAddress', 'ship_address']) || '',
          firstDefined(r, ['shipCity', 'ShipCity', 'ship_city']) || '',
          firstDefined(r, ['shipRegion', 'ShipRegion', 'ship_region']) || '',
          firstDefined(r, ['shipPostalCode', 'shipPostalCode', 'ship_postal_code']) || '',
          firstDefined(r, ['shipCountry', 'ShipCountry', 'ship_country']) || '',
          r.orderDate ? new Date(r.orderDate).toLocaleDateString() : '',
          r.requiredDate ? new Date(r.requiredDate).toLocaleDateString() : '',
          r.shippedDate ? new Date(r.shippedDate).toLocaleDateString() : '',
          productsText,
          r.region || '',
          r.totalAmount != null ? r.totalAmount : '',
        ]

        lines.push(rowValues)
      })

      // CSV stringify with proper escaping and comma separator
      const csvContent = lines
        .map((row) =>
          row
            .map((cell) => {
              if (cell === null || cell === undefined) return ''
              const s = String(cell)
              const escaped = s.replace(/"/g, '""')
              const needsQuotes = /[",\n]/.test(s)
              return needsQuotes ? `"${escaped}"` : escaped
            })
            .join(';'),
        )
        .join('\n')

      const BOM = '\uFEFF'
      const blob = new Blob([BOM + csvContent], { type: 'text/csv;charset=utf-8;' })
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = `orders-${new Date().toISOString().slice(0, 10)}.csv`
      document.body.appendChild(a)
      a.click()
      a.remove()
      URL.revokeObjectURL(url)
    } catch (err) {
      console.error('Failed to export to CSV', err)
    }
  }

  // EXPORT TO PDF
  async function exportToPDF() {
    try {
      const logoBase64 = await getBase64Image(logo)

      const header = [
        'Order ID',
        'Customer Name',
        'Customer ID',
        'Employee ID',
        'Ship Via',
        'Freight',
        'Ship Name',
        'Ship Address',
        'Ship City',
        'Ship Region',
        'Ship Postal Code',
        'Ship Country',
        'Order Date',
        'Required Date',
        'Shipped Date',
        'Products',
        'Region',
        'Total Amount',
      ]

      const body = [
        header.map((h) => ({
          text: h,
          style: 'tableHeader',
        })),
      ]

      function firstDefined(obj, keys) {
        for (const k of keys) {
          if (obj && obj[k] !== undefined && obj[k] !== null) return obj[k]
        }
        return ''
      }

      rows.value.forEach((r) => {
        const productsArr = Array.isArray(r.products) ? r.products : []
        const productsText = productsArr.length
          ? `${productsArr.length} Items: ${productsArr.join(', ')}`
          : ''

        body.push([
          firstDefined(r, ['orderID', 'orderId', '_id', 'id']),
          firstDefined(r, ['customerName', 'customer']),
          firstDefined(r, ['customerID', 'customerId', 'customer_id']),
          firstDefined(r, ['employeeID', 'employeeId', 'employee_id']),
          firstDefined(r, ['shipVia', 'ship_via', 'shipperID']),
          firstDefined(r, ['freight', 'Freight']),
          firstDefined(r, ['shipName', 'ShipName']),
          firstDefined(r, ['shipAddress', 'ShipAddress', 'ship_address']),
          firstDefined(r, ['shipCity', 'ShipCity', 'ship_city']),
          firstDefined(r, ['shipRegion', 'ShipRegion', 'ship_region']),
          firstDefined(r, ['shipPostalCode', 'ship_postal_code', 'shipPostalCode']),
          firstDefined(r, ['shipCountry', 'ShipCountry', 'ship_country']),
          r.orderDate ? new Date(r.orderDate).toLocaleDateString() : '',
          r.requiredDate ? new Date(r.requiredDate).toLocaleDateString() : '',
          r.shippedDate ? new Date(r.shippedDate).toLocaleDateString() : '',
          productsText,
          r.region || '',
          r.totalAmount != null ? r.totalAmount : '',
        ])
      })

      const docDefinition = {
        pageSize: 'A4',
        pageOrientation: 'landscape',

        pageMargins: [20, 20, 20, 20],

        content: [
          // ===== HEADER =====
          {
            columns: [
              {
                image: logoBase64,
                width: 70,
              },

              {
                stack: [
                  {
                    text: 'Northwind Traders - Order Details Report',
                    style: 'title',
                  },

                  {
                    text: 'Precision in Motion',
                    style: 'subtitle',
                  },

                  {
                    text: `Generated: ${new Date().toLocaleDateString()}`,
                    style: 'date',
                  },
                ],

                margin: [15, 10, 0, 0],
              },
            ],
          },

          // SPACE
          {
            text: '',
            margin: [0, 10],
          },

          // ===== TABLE =====
          {
            table: {
              headerRows: 1,

              // CUSTOM WIDTHS
              widths: [
                20, // Order ID
                55, // Customer Name
                20, // Customer ID
                20, // Employee ID
                20, // Ship Via
                25, // Freight
                45, // Ship Name
                70, // Address
                35, // City
                25, // Region
                35, // Postal
                35, // Country
                35, // Order Date
                35, // Required Date
                35, // Shipped Date
                85, // Products
                25, // Region
                30, // Total
              ],

              body,
            },

            layout: {
              fillColor: (rowIndex) => {
                return rowIndex === 0 ? '#0B5CAD' : null
              },

              hLineColor: () => '#D6D6D6',
              vLineColor: () => '#D6D6D6',

              paddingLeft: () => 4,
              paddingRight: () => 4,
              paddingTop: () => 3,
              paddingBottom: () => 3,
            },
          },
        ],

        styles: {
          title: {
            fontSize: 20,
            bold: true,
            color: '#1E293B',
          },

          subtitle: {
            fontSize: 11,
            italics: true,
            color: '#64748B',
            margin: [0, 3, 0, 0],
          },

          date: {
            fontSize: 9,
            color: 'gray',
            margin: [0, 6, 0, 0],
          },

          tableHeader: {
            color: 'white',
            bold: true,
            fontSize: 5,
          },
        },

        defaultStyle: {
          fontSize: 7,
        },
      }

      pdfMake
        .createPdf(docDefinition)
        .download(`orders-${new Date().toISOString().slice(0, 10)}.pdf`)
    } catch (err) {
      console.error('Failed to export PDF', err)
    }
  }

  // Helper to convert image to base64 for embedding in PDF
  function getBase64Image(imgUrl) {
    return new Promise((resolve) => {
      const img = new Image()

      img.setAttribute('crossOrigin', 'anonymous')

      img.onload = () => {
        const canvas = document.createElement('canvas')
        canvas.width = img.width
        canvas.height = img.height

        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0)

        resolve(canvas.toDataURL('image/png'))
      }

      img.src = imgUrl
    })
  }

  // INITIALS
  function getInitials(name) {
    if (!name) return ''

    return name
      .split(' ')
      .map((w) => w[0])
      .join('')
      .substring(0, 2)
      .toUpperCase()
  }

  onMounted(fetchRows)

  return {
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
  }
}
