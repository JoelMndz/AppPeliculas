<template>
  <nav class="navbar navbar-expand-sm navbar-light bg-light">
    <div class="container-fluid">
      <RouterLink class="navbar-brand" :to="rutaPrincipal()">Películas Favoritas</RouterLink>
      <ul v-if="usuario" class="navbar-nav">
        <li class="nav-item">
          <RouterLink class="nav-link active" to="/generos">Generos</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link active mx-1" to="/peliculas">Películas</RouterLink>
        </li>
        <li class="nav-item">
          <button class="btn btn-danger mx-2" to="/peliculas" @click="cerrarSesion()">Salir</button>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script lang="ts">
import { useContexto } from '@/stores/contexto';
import {mapActions, mapState} from 'pinia'
import { defineComponent } from 'vue'
import { RouterLink } from 'vue-router'
export default defineComponent({
  components:{
    RouterLink
  },
  setup() {
    
  },
  methods:{
    ...mapActions(useContexto,['cerrarSesion']),
    rutaPrincipal(){
      return !this.usuario ? '':'/peliculas';
    }
  },
  computed:{
    ...mapState(useContexto,['usuario']),
  }
})
</script>


<style>

</style>