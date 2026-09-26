<template>
  <footer class="statusbar">
    <div class="ready">
      <span class="status-dot nominal"></span>
      <span>READY</span>
    </div>

    <div class="status-message">
      <span class="status-label">AUTOMATION</span>
      <strong>IDLE</strong>
    </div>

    <div class="simulation-controls">
      <span class="simulation-label">SIM</span>

      <div class="transport-controls">
        <button class="transport-button" title="Pause simulation">Ⅱ</button>
      </div>

      <div class="time-controls">
        <button class="active">1×</button>
        <button>10×</button>
        <button>100×</button>
      </div>
    </div>
  </footer>
</template>
