<script setup lang="ts">
import { ref } from 'vue'

import DatabasePanel from './components/DatabasePanel.vue'
import EditorPanel from './components/EditorPanel.vue'
import LeftPanel from './components/LeftPanel.vue'
import RightPanel from './components/RightPanel.vue'
import StatusBar from './components/StatusBar.vue'
import TerminalPanel from './components/TerminalPanel.vue'
import TopBar from './components/TopBar.vue'

type Section = 'SHIP' | 'SCIENCE' | 'MISSIONS' | 'DATABASE'

const activeSection = ref<Section>('SHIP')
</script>

<template>
  <div class="workstation">
    <TopBar :active-section="activeSection" @select-section="activeSection = $event" />

    <main class="workspace">
      <LeftPanel />

      <section class="primary">
        <div v-if="activeSection === 'SHIP'" class="ship-workspace">
          <EditorPanel />
          <TerminalPanel />
        </div>

        <DatabasePanel v-else-if="activeSection === 'DATABASE'" />

        <div v-else class="workspace-placeholder">
          {{ activeSection }}
        </div>
      </section>

      <RightPanel />
    </main>

    <StatusBar />
  </div>
</template>
