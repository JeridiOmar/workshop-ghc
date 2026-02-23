<script setup lang="ts">
import { computed, watch, onUnmounted } from 'vue'
import { useTimerStore } from '@/stores/timer'
import { Button } from '@/components/ui/button'
import { Play, Pause, SkipForward, RotateCcw } from 'lucide-vue-next'
import { formatTime } from '@/lib/utils'

const timerStore = useTimerStore()

const displayTime = computed(() => formatTime(timerStore.timeRemaining))

const modeLabel = computed(() => {
  switch (timerStore.currentMode) {
    case 'pomodoro':
      return 'Focus'
    case 'short_break':
      return 'Short Break'
    case 'long_break':
      return 'Long Break'
    default:
      return 'Focus'
  }
})

function handlePlayPause() {
  if (timerStore.isRunning) {
    timerStore.pause()
  } else {
    timerStore.start()
  }
}

// Cleanup on unmount
onUnmounted(() => {
  if (timerStore.isRunning) {
    timerStore.pause()
  }
})

// Watch for mode changes to update title
watch(() => timerStore.currentMode, () => {
  if (!timerStore.isRunning) {
    document.title = 'Pomodoro Timer'
  }
})
</script>

<template>
  <div class="flex flex-col items-center justify-center min-h-[calc(100vh-12rem)]">
    <div class="w-full max-w-2xl">
      <!-- Mode Selector -->
      <div class="flex justify-center gap-4 mb-12">
        <Button
          variant="ghost"
          size="lg"
          :class="[
            'text-white/80 hover:text-white hover:bg-white/20',
            timerStore.currentMode === 'pomodoro' && 'text-white bg-white/30 font-semibold'
          ]"
          @click="timerStore.switchMode('pomodoro')"
        >
          Pomodoro
        </Button>
        <Button
          variant="ghost"
          size="lg"
          :class="[
            'text-white/80 hover:text-white hover:bg-white/20',
            timerStore.currentMode === 'short_break' && 'text-white bg-white/30 font-semibold'
          ]"
          @click="timerStore.switchMode('short_break')"
        >
          Short Break
        </Button>
        <Button
          variant="ghost"
          size="lg"
          :class="[
            'text-white/80 hover:text-white hover:bg-white/20',
            timerStore.currentMode === 'long_break' && 'text-white bg-white/30 font-semibold'
          ]"
          @click="timerStore.switchMode('long_break')"
        >
          Long Break
        </Button>
      </div>

      <!-- Timer Display -->
      <div class="text-center mb-12">
        <div 
          class="text-9xl font-bold text-white mb-4 tabular-nums"
          :class="{ 'animate-pulse-slow': timerStore.isRunning }"
        >
          {{ displayTime }}
        </div>
        <div class="text-xl text-white/80">
          {{ modeLabel }}
        </div>
      </div>

      <!-- Controls -->
      <div class="flex justify-center gap-4 mb-8">
        <Button
          size="lg"
          variant="secondary"
          class="h-16 w-16 rounded-full bg-white hover:bg-white/90 p-0"
          style="color: #1f2937; position: relative;"
          @click="handlePlayPause"
        >
          <Play v-if="!timerStore.isRunning" :size="32" :stroke-width="2.5" style="stroke: #1f2937 !important; color: #1f2937 !important; display: block !important; opacity: 1 !important; width: 32px; height: 32px;" />
          <Pause v-else :size="32" :stroke-width="2.5" style="stroke: #1f2937 !important; color: #1f2937 !important; display: block !important; opacity: 1 !important; width: 32px; height: 32px;" />
        </Button>
        <Button
          size="lg"
          variant="outline"
          class="h-16 w-16 rounded-full border-white/40 hover:bg-white/20 p-0"
          style="color: white; position: relative;"
          @click="timerStore.skip"
          :disabled="!timerStore.isRunning && !timerStore.isPaused"
        >
          <SkipForward :size="28" :stroke-width="2.5" style="stroke: white !important; color: white !important; display: block !important; opacity: 1 !important; width: 28px; height: 28px;" />
        </Button>
        <Button
          size="lg"
          variant="outline"
          class="h-16 w-16 rounded-full border-white/40 hover:bg-white/20 p-0"
          style="color: white; position: relative;"
          @click="timerStore.reset"
          :disabled="!timerStore.isRunning && !timerStore.isPaused"
        >
          <RotateCcw :size="28" :stroke-width="2.5" style="stroke: white !important; color: white !important; display: block !important; opacity: 1 !important; width: 28px; height: 28px;" />
        </Button>
      </div>

      <!-- Progress Bar -->
      <div class="w-full bg-white/20 rounded-full h-2 mb-4">
        <div 
          class="bg-white h-2 rounded-full transition-all duration-1000"
          :style="{ width: `${timerStore.progress}%` }"
        />
      </div>

      <!-- Stats -->
      <div class="flex justify-center gap-8 text-white/80 text-sm">
        <div class="text-center">
          <div class="font-semibold text-white text-2xl">{{ timerStore.completedPomodoros }}</div>
          <div>Completed Today</div>
        </div>
      </div>
    </div>
  </div>
</template>
