<script setup lang="ts">
const systems = [
  { name: 'BATTERY', value: '87%', state: 'nominal', level: 87 },
  { name: 'THERMAL', value: 'NOMINAL', state: 'nominal' },
  { name: 'FUEL', value: '94%', state: 'nominal', level: 94 },
  { name: 'HULL', value: '100%', state: 'nominal' },
  { name: 'DATA', value: '2%', state: 'nominal', level: 2 },
  { name: 'COMPUTE', value: '4%', state: 'nominal', level: 4 },
]
</script>

<template>
  <section class="ship-systems-panel">
    <div class="panel-heading">
      <span>SHIP SYSTEMS</span>
      <span class="panel-code">SYS</span>
    </div>

    <div class="systems">
      <div
        v-for="system in systems"
        :key="system.name"
        class="system-row"
        :class="{ 'has-level': system.level != null }"
      >
        <span class="status-dot" :class="system.state"></span>

        <div class="system-info">
          <span class="system-name">{{ system.name }}</span>

          <div v-if="system.level != null" class="system-meter">
            <div class="system-meter-fill" :class="system.state" :style="{ width: `${system.level}%` }"></div>
          </div>
        </div>

        <span class="system-value">{{ system.value }}</span>
      </div>
    </div>

    <div class="system-summary">
      <span>ALL SYSTEMS</span>
      <strong>NOMINAL</strong>
    </div>
  </section>
</template>
