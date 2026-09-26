<script setup lang="ts">
const metrics = [
  { name: 'O₂', value: '20.9', unit: '%', state: 'nominal' },
  { name: 'CO₂', value: '0.04', unit: '%', state: 'nominal' },
  { name: 'PRESSURE', value: '101.2', unit: 'kPa', state: 'nominal' },
  { name: 'TEMPERATURE', value: '21.4', unit: '°C', state: 'nominal' },
  { name: 'HUMIDITY', value: '43', unit: '%', state: 'nominal' },
]
</script>

<template>
  <section class="habitat-status-panel">
    <div class="panel-heading">
      <span>HABITAT</span>
      <span class="panel-code">HAB</span>
    </div>

    <div class="systems">
      <div v-for="metric in metrics" :key="metric.name" class="system-row habitat-row">
        <span class="status-dot" :class="metric.state"></span>

        <span class="system-name">{{ metric.name }}</span>

        <span class="measurement">
          <span class="measurement-value">{{ metric.value }}</span>
          <span class="measurement-unit">{{ metric.unit }}</span>
        </span>
      </div>
    </div>

    <div class="system-summary">
      <span>ENVIRONMENT</span>
      <strong>NOMINAL</strong>
    </div>
  </section>
</template>
