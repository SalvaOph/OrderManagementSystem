import { ref, onMounted, watch } from 'vue'
import { api } from 'src/boot/axios'

export default function useShipmentOrderInsights() {
  const year = ref(null)
  const allOrders = ref([])
  const years = ref([])

  const barSeries = ref([{ name: 'Orders', data: [] }])

  const barOptions = ref({
    chart: {
      type: 'bar',
      toolbar: { show: false },
    },
    colors: ['#0d3b66'],
    dataLabels: { enabled: false },
    xaxis: {
      categories: [],
    },
  })

  const donutSeries = ref([])

  const donutOptions = ref({
    labels: [],
    colors: ['#0d3b66', '#4dabf7', '#90caf9', '#e0e0e0'],
    dataLabels: { enabled: false },
    plotOptions: {
      pie: {
        donut: {
          size: '60%',
          labels: {
            show: true,
            value: {
              show: true,
              offsetY: -2,
              fontSize: '22px',
              fontWeight: 600,
            },
            name: {
              show: true,
              color: '0d3b66',
              fontSize: '14px',
            },
            total: {
              show: true,
              label: 'TOTAL',
              fontSize: '12px',
              fontWeight: 600,
              formatter: function (w) {
                return w.globals.seriesTotals.reduce((a, b) => a + b, 0)
              },
            },
          },
        },
      },
    },
    legend: {
      position: 'bottom',
    },
  })

  const fetchOrders = async () => {
    try {
      const res = await api.get('/order-details-view')
      allOrders.value = res.data || []

      buildYears()
      setDefaultYear()

      // allow reactive updates to propagate
      setTimeout(() => {
        updateChart()
        updateDonutChart()
      }, 0)
    } catch (err) {
      console.error('Failed to fetch orders:', err)
    }
  }

  const buildYears = () => {
    const uniqueYears = new Set()

    allOrders.value.forEach((order) => {
      const date = new Date(order.orderDate)
      if (!isNaN(date)) {
        uniqueYears.add(date.getFullYear())
      }
    })

    years.value = [...uniqueYears].map(String).sort((a, b) => b - a)
  }

  const setDefaultYear = () => {
    if (years.value.length) year.value = years.value[0]
  }

  const updateChart = () => {
    if (!year.value) return

    const monthly = {
      Jan: 0,
      Feb: 0,
      Mar: 0,
      Apr: 0,
      May: 0,
      Jun: 0,
      Jul: 0,
      Aug: 0,
      Sep: 0,
      Oct: 0,
      Nov: 0,
      Dec: 0,
    }

    allOrders.value.forEach((order) => {
      const date = new Date(order.orderDate)
      if (isNaN(date)) return

      if (date.getFullYear() === Number(year.value)) {
        const month = date.toLocaleString('en-US', { month: 'short' })
        monthly[month]++
      }
    })

    barOptions.value = {
      ...barOptions.value,
      xaxis: {
        categories: Object.keys(monthly),
      },
    }

    barSeries.value = [
      {
        name: 'Orders',
        data: Object.values(monthly),
      },
    ]
  }

  const updateDonutChart = () => {
    if (!year.value) return

    const regionCount = {}

    allOrders.value.forEach((order) => {
      const date = new Date(order.orderDate)
      if (isNaN(date)) return

      if (date.getFullYear() === Number(year.value)) {
        let region = order.region
        if (!region) region = 'Unknown'

        regionCount[region] = (regionCount[region] || 0) + 1
      }
    })

    donutOptions.value = {
      ...donutOptions.value,
      labels: Object.keys(regionCount),
    }

    donutSeries.value = Object.values(regionCount)
  }

  watch(year, (val) => {
    if (val) {
      updateChart()
      updateDonutChart()
    }
  })

  onMounted(() => {
    fetchOrders()
  })

  return {
    year,
    years,
    barSeries,
    barOptions,
    donutSeries,
    donutOptions,
  }
}
