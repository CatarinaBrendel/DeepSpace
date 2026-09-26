<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { basicSetup } from 'codemirror'
import { EditorState } from '@codemirror/state'
import { EditorView } from '@codemirror/view'
import { python } from '@codemirror/lang-python'
import { HighlightStyle, syntaxHighlighting } from '@codemirror/language'
import { tags } from '@lezer/highlight'

const editorElement = ref<HTMLElement | null>(null)

let editor: EditorView | null = null

const initialScript = `from spacecraft import power

generator = power.generator('primary')
battery = power.battery('main')

while True:
    charge = battery.charge()

    if charge < 0.4:
        generator.start()
    elif charge > 0.8:
        generator.stop()

    sleep(60)
`

const highlightStyle = HighlightStyle.define([
  { tag: tags.keyword, color: '#d56bd8' },
  { tag: tags.string, color: '#dfbd62' },
  { tag: tags.number, color: '#dfbd62' },
  { tag: tags.comment, color: '#526a76' },
  { tag: tags.function(tags.variableName), color: '#55bde8' },
  { tag: tags.definition(tags.variableName), color: '#d5e1e7' },
])

onMounted(() => {
  if (!editorElement.value) return

  const state = EditorState.create({
    doc: initialScript,
    extensions: [
      basicSetup,
      python(),
      syntaxHighlighting(highlightStyle),
      EditorView.theme({
        '&': {
          height: '100%',
          backgroundColor: '#03090d',
          color: '#aabac4',
          fontSize: '13px',
        },
        '.cm-scroller': {
          fontFamily: "'SFMono-Regular', Consolas, 'Liberation Mono', monospace",
          lineHeight: '1.65',
        },
        '.cm-content': {
          padding: '12px 0',
          caretColor: '#32b9e8',
        },
        '.cm-gutters': {
          backgroundColor: '#03090d',
          color: '#405965',
          border: 'none',
          borderRight: '1px solid #132b38',
        },
        '.cm-activeLine': {
          backgroundColor: '#07141b',
        },
        '.cm-activeLineGutter': {
          backgroundColor: '#0a1921',
          color: '#78909c',
        },
        '.cm-selectionBackground, ::selection': {
          backgroundColor: '#12394a !important',
        },
        '&.cm-focused': {
          outline: 'none',
        },
      }),
    ],
  })

  editor = new EditorView({
    state,
    parent: editorElement.value,
  })
})

onBeforeUnmount(() => {
  editor?.destroy()
})
</script>

<template>
  <section class="editor-panel">
    <div class="editor-toolbar">
      <span class="editor-title">EDITOR</span>

      <div class="editor-location">
        <span class="editor-location-label">PATH</span>
        <span class="editor-path">/home/operator/scripts/power.py</span>
      </div>
    </div>

    <div class="editor-tabs">
      <button class="editor-tab active">
        <span class="tab-status"></span>
        <span class="tab-name">power.py</span>
        <span class="tab-close">×</span>
      </button>

      <button class="new-tab" title="Open file">+</button>

      <div class="editor-actions">
        <button disabled>STOP</button>
        <button class="run-button" disabled>RUN</button>
      </div>
    </div>

    <div ref="editorElement" class="editor"></div>

    <div class="editor-footer">
      <div class="editor-document-state">
        <span class="editor-state">SAVED</span>
        <span class="language-indicator">PY</span>
        <span class="cursor-position">LN 1, COL 1</span>
      </div>
    </div>
  </section>
</template>
