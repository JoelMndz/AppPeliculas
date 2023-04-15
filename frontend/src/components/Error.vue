<template>
  <div class="alert alert-warning alert-dismissible fade show" role="alert">
    <strong>{{error}}</strong>
    <button type="button" class="btn-close" @click="resetarError()" aria-label="Close"></button>
  </div>
</template>

<script lang="ts">
import { useContexto } from '@/stores/contexto'
import {mapState, mapActions} from 'pinia'
import { defineComponent } from 'vue'

export default defineComponent({
  setup() {

    return{

    }
  },
  methods:{
    ...mapActions(useContexto, ['resetarError']),

  },
  computed:{
    ...mapState(useContexto, ['error'])
  }
})
</script>
