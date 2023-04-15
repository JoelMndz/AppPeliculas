<template>
  <h2 class="text-center mt-3 ">Módulo de Generos</h2>
  <div class="row justify-content-center">
    <div class="col-8 col-md-5 mt-3">
      <div class="card">
        <div class="card-header">
          <h4 class="text-center">{{!id ?'Agregar':'Actualizar'}} Genero</h4>
        </div>
        <form @submit.prevent="procesarFormulario()" class="card-body">
          <div class="mb-2">
            <label class="form-label">Descripción</label>
            <input type="text" placeholder="Terror" class="form-control"
            v-model="descripcion">
          </div>
          <button class="btn btn-primary w-100" :disabled="cargando">Guardar</button>
        </form>
      </div>
    </div>
    <div class="col-10 col-md-6 mt-3">
      <table class="table">
  <thead>
    <tr>
      <th scope="row">Id</th>
      <th>Descripción</th>
      <th>Aciones</th>
    </tr>
  </thead>
    <tbody>
      <tr v-for="i in generos" :key="i.id">
        <th scope="row">{{ i.id }}</th>
        <td>{{i.descripcion}}</td>
        <td>
          <button @click="llenarCampos(i)" class="btn">✏</button>
          <button @click="eliminar(i)" class="btn">❌</button>
        </td>
      </tr>
    </tbody>
  </table>
    </div>
  </div>
</template>

<script lang="ts">
import { useContexto } from '@/stores/contexto'
import { mapActions, mapState } from 'pinia'
import { defineComponent, ref } from 'vue'

export default defineComponent({
  setup() {
    const descripcion = ref('');
    const id = ref(null);
    const cargando = ref(false);

    return{
      descripcion,
      id,
      cargando
    }
  },
  methods:{
    ...mapActions(useContexto,['obtenerGeneros','eliminarGenero','agregarGenero','actualizarGenero','protegerRuta']),
    eliminar(item:any){
      if(confirm(`¿Esta seguro de eliminar el genero "${item.descripcion}"?`)){
        this.eliminarGenero(item.id);
      }
    },
    llenarCampos(item:any){
      this.id = item.id;
      this.descripcion = item.descripcion;
    },
    async procesarFormulario(){
      this.cargando = true;
      if(this.id){
        await this.actualizarGenero(this.id, this.descripcion);
        this.id = null;
      }else{
        await this.agregarGenero(this.descripcion);
      }
      this.descripcion = '';
      this.cargando = false;
    }
  },
  computed:{
    ...mapState(useContexto,['generos'])
  },  
  async mounted(){
    await this.obtenerGeneros();
  },
  async created(){
    await this.protegerRuta();
  }
})
</script>


<style>

</style>