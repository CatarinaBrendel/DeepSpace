<script setup lang="ts">
const scripts = [
  { name: 'power.py', modified: 'DAY 006 08:42', status: 'ACTIVE' },
  { name: 'thermal.py', modified: 'DAY 006 07:21', status: 'IDLE' },
  { name: 'navigation.py', modified: 'DAY 005 11:08', status: 'IDLE' },
  { name: 'survey.py', modified: 'DAY 004 20:33', status: 'IDLE' },
  { name: 'battery_watch.py', modified: 'DAY 004 09:14', status: 'ACTIVE' },
  { name: 'radiator_control.py', modified: 'DAY 003 13:27', status: 'IDLE' },
]

const navigation = [
  {
    title: 'SCRIPTS',
    count: 12,
    items: ['My Scripts', 'power.py', 'thermal.py', 'navigation.py', 'survey.py'],
  },
  {
    title: 'REFERENCE',
    count: 8,
    items: ['Ship Systems', 'Components', 'Operational Concepts'],
  },
  {
    title: 'API',
    count: 34,
    items: ['Getting Started', 'Runtime', 'Power', 'Thermal', 'Navigation', 'Sensors', 'Life Support'],
  },
  {
    title: 'GUIDES',
    count: 10,
    items: ['Your First Automation', 'Monitoring Systems', 'Responding to Events', 'Best Practices'],
  },
]
</script>

<template>
  <section class="database-panel">
    <header class="database-header">
      <div class="database-identity">
        <span class="database-title">DATABASE</span>
        <span class="database-breadcrumb">ONBOARD KNOWLEDGE BASE / SCRIPTS / power.py</span>
      </div>

      <div class="database-search">
        <span>SEARCH DATABASE...</span>
        <span class="database-search-icon">⌕</span>
      </div>
    </header>

    <div class="database-workspace">
      <aside class="database-navigation">
        <section v-for="section in navigation" :key="section.title" class="database-nav-section">
          <div class="database-nav-heading">
            <span class="database-nav-title">
              <span class="database-chevron">⌄</span>
              {{ section.title }}
            </span>

            <span class="database-count">{{ section.count }}</span>
          </div>

          <button
            v-for="item in section.items"
            :key="item"
            class="database-nav-item"
            :class="{ active: item === 'My Scripts' }"
          >
            <span class="database-nav-icon">{{ section.title === 'API' ? '{}' : '□' }}</span>
            <span>{{ item }}</span>
          </button>
        </section>
      </aside>

      <section class="database-list">
        <header class="database-section-header">
          <span>MY SCRIPTS</span>
          <button class="database-action">+ NEW</button>
        </header>

        <div class="script-table">
          <div class="script-table-header">
            <span>NAME</span>
            <span>MODIFIED</span>
            <span>STATUS</span>
          </div>

          <button
            v-for="script in scripts"
            :key="script.name"
            class="script-row"
            :class="{ active: script.name === 'power.py' }"
          >
            <span class="script-name">{{ script.name }}</span>
            <span class="script-modified">{{ script.modified }}</span>

            <span class="script-status" :class="script.status.toLowerCase()">
              <span class="status-dot" :class="script.status === 'ACTIVE' ? 'nominal' : ''"></span>
              {{ script.status }}
            </span>
          </button>
        </div>
      </section>

      <article class="database-detail">
        <header class="database-detail-header">
          <div class="database-document-title">
            <span class="database-document-icon">▤</span>

            <div>
              <div class="database-document-name">
                <strong>power.py</strong>

                <span class="script-status active">
                  <span class="status-dot nominal"></span>
                  ACTIVE
                </span>
              </div>

              <p>Controls the primary generator according to battery state of charge and current power demand.</p>
            </div>
          </div>

          <div class="database-detail-actions">
            <button class="database-action primary">OPEN IN EDITOR</button>
            <button class="database-action">DUPLICATE</button>
          </div>
        </header>

        <dl class="database-metadata">
          <div>
            <dt>PATH</dt>
            <dd>/home/operator/scripts/power.py</dd>
          </div>
          <div>
            <dt>TYPE</dt>
            <dd>Python Script</dd>
          </div>
          <div>
            <dt>MODIFIED</dt>
            <dd>DAY 006 08:42</dd>
          </div>
          <div>
            <dt>STATUS</dt>
            <dd class="nominal-text">Active (automation)</dd>
          </div>
        </dl>

        <section class="database-detail-section">
          <h2>DESCRIPTION</h2>
          <p>
            Monitors the main battery state of charge and manages the primary generator to maintain nominal power
            levels. Includes hysteresis to prevent rapid cycling.
          </p>
        </section>

        <section class="database-detail-section">
          <h2>USAGE</h2>
          <p>
            Designed to run continuously as part of the ship's automation suite. Can be started manually from the
            terminal or assigned to an automation task.
          </p>
        </section>

        <section class="database-detail-section">
          <h2>RELATED SYSTEMS</h2>

          <div class="database-tags">
            <span>Primary Generator</span>
            <span>Main Battery</span>
            <span>Power Distribution</span>
          </div>
        </section>

        <section class="database-detail-section code-section">
          <h2>CODE PREVIEW</h2>

          <pre
            class="database-code"
          ><code><span class="code-keyword">from</span> spacecraft <span class="code-keyword">import</span> power

generator = power.generator(<span class="code-string">'primary'</span>)
battery = power.battery(<span class="code-string">'main'</span>)

<span class="code-keyword">while</span> <span class="code-literal">True</span>:
    charge = battery.charge()

    <span class="code-keyword">if</span> charge &lt; <span class="code-number">0.4</span>:
        generator.start()</code></pre>
        </section>
      </article>
    </div>
  </section>
</template>
