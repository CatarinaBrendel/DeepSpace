<script setup lang="ts">
const events = [
  { time: '08:41:02', message: 'Primary reactor online' },
  { time: '08:41:08', message: 'Navigation solution accepted' },
  { time: '08:41:14', message: 'Cruise configuration established' },
]
</script>

<template>
  <section class="events-panel">
    <div class="panel-heading">
      <span>EVENTS</span>
      <span class="panel-code">LOG</span>
    </div>

    <div class="events">
      <div v-for="event in events" :key="event.time" class="event">
        <time>{{ event.time }}</time>
        <span>{{ event.message }}</span>
      </div>
    </div>
  </section>
</template>
