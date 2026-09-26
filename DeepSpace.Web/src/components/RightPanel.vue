<script setup lang="ts">
import EventsPanel from './EventsPanel.vue'
import MissionPanel from './MissionPanel.vue'
import TelemetryPanel from './TelemetryPanel.vue'
</script>

<template>
  <aside class="right-panel">
    <MissionPanel />
    <TelemetryPanel />
    <EventsPanel />
  </aside>
</template>
