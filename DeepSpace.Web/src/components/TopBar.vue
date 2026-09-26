<script setup lang="ts">
type Section = 'SHIP' | 'SCIENCE' | 'MISSIONS' | 'DATABASE'

defineProps<{
  activeSection: Section
}>()

const emit = defineEmits<{
  selectSection: [section: Section]
}>()

const navigation: Section[] = ['SHIP', 'SCIENCE', 'MISSIONS', 'DATABASE']
</script>

<template>
  <header class="topbar">
    <div class="identity">
      <div class="mark">DS</div>
      <span class="product-name">DEEPSPACE</span>
    </div>

    <nav class="navigation">
      <button
        v-for="item in navigation"
        :key="item"
        class="nav-item"
        :class="{ active: item === activeSection }"
        @click="emit('selectSection', item)"
      >
        {{ item }}
      </button>
    </nav>

    <div class="clock">
      <span>MISSION DAY 001</span>
      <strong>08:41:26</strong>
    </div>
  </header>
</template>
