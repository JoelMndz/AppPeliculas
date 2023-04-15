<template>
  <h2 class="text-center mt-3 ">Módulo de Películas</h2>
  <div v-if="agregar || id" class="row justify-content-center">
    <div class="col-8 col-md-5 mt-3">
      <div class="card">
        <div class="card-header">
          <h4 class="text-center">{{!id ?'Agregar':'Actualizar'}} Película</h4>
        </div>
        <form @submit.prevent="procesarFormulario()" class="card-body">
          <div class="mb-2">
            <input type="text" placeholder="Ingresa el título" class="form-control" v-model="pelicula.titulo" required>
          </div>
          <div class="mb-2">
            <input type="number" placeholder="Ingresa el Año" class="form-control" v-model="pelicula.anio" min="1900" max="2023" required>
          </div>
          <div class="mb-2">
            <input type="text" placeholder="Ingresa el director" class="form-control" v-model="pelicula.director" required>
          </div>
          <div class="mb-2">
            <select class="form-select" v-model="pelicula.idGenero" required>
            <option value="">Seleccione un Genero</option>
            <option v-for="i in generos" :value="i.id" :key="i.id">{{ i.descripcion }}</option>
            </select>
          </div>
          <div class="mb-2" v-if="!id">
            <input type="file" placeholder="Ingresa la" class="form-control" required @change="obtenerArchivo" accept="image/*">
          </div>
          <button class="btn btn-primary w-100 ,t-1" :disabled="cargando">Guardar</button>
        </form>
      </div>
    </div>
  </div>
  <div class="mt-3" v-else>
    <div>
      <button @click="agregar = true" class="btn btn-success">Agregar</button>
    </div>
    <div class="mt-2 row">
      <div class="col">
        <input class="form-control" type="text" placeholder="Título" v-model="filtro.titulo">
      </div>
      <div class="col">
        <select class="form-select" v-model="filtro.idGenero" required>
        <option value="">Seleccione un Genero</option>
        <option v-for="i in generos" :value="i.id" :key="i.id">{{ i.descripcion }}</option>
        </select>
      </div>
      <button @click="filtrarPeliculas(filtro)" class="btn btn-primary mx-3 col">Filtrar</button>
    </div>
    <div class="row row-cols-1 row-cols-md-3 g-4 mt-1 mb-3">
      <div class="col" v-for="i in peliculasFiltradas" :key="i.Id">
        <div class="card h-100">
          <img :src="API+'/'+i.fotoUrl" class="card-img-top" alt="Portada">
          <div class="card-body">
            <h5 class="card-title text-center">{{ i.titulo }}</h5>
            <p class="card-text">Director: {{ i.director }}</p>
            <p class="card-text">Año: {{ i.anio }}</p>
            <p class="card-text">Genero: {{ i.genero }}</p>
            <p>
              <span title="Editar" class="btn" @click="llenarCampos(i)">✏</span>
              <span title="Eliminar" class="btn mx-2" @click="eliminar(i)">❌</span>
            </p>
          </div>
        </div>
      </div>
      <p v-if="peliculasFiltradas.length == 0">
        No hay películas 😥
      </p>
    </div>
  </div>
</template>

<script lang="ts">
import { useContexto } from '@/stores/contexto'
import { mapActions, mapState } from 'pinia'
import { defineComponent, reactive, ref } from 'vue'

export default defineComponent({
  setup() {
    const agregar = ref(false);
    const pelicula = reactive({
      titulo:'',
      anio: '',
      director: '',
      fotoBase64: null,
      idGenero: ''
    });
    const filtro = reactive({
      idGenero: '',
      titulo: ''
    })
    const id = ref(null);
    const cargando = ref(false);

    return{
      id,
      cargando,
      agregar,
      pelicula,
      filtro
    }
  },
  methods:{
    ...mapActions(useContexto,['obtenerPeliculas','eliminarPelicula','agregarPelicula','actualizarPelicula','obtenerGeneros','filtrarPeliculas','protegerRuta']),
    obtenerArchivo(e:any){
      if(e.target.files.length == 1){
        this.pelicula.fotoBase64 = e.target.files[0];
      }      
    },
    eliminar(item:any){
      if(confirm(`¿Esta seguro de eliminar el la pelicula "${item.titulo}"?`)){
        this.eliminarPelicula(item.id);
      }
    },
    llenarCampos(item:any){
      this.id = item.id;
      this.pelicula.anio = item.anio;
      this.pelicula.titulo = item.titulo;
      this.pelicula.idGenero = item.idGenero;
      this.pelicula.director = item.director;
    },
    async procesarFormulario(){
      this.cargando = true;
      if(this.id){
        await this.actualizarPelicula(this.pelicula,this.id);
        this.id = null;
      }else{
        await this.agregarPelicula(this.pelicula);
      }
      this.pelicula.anio = '';
      this.pelicula.titulo = '';
      this.pelicula.idGenero = '';
      this.pelicula.director = '';
      this.agregar = false;
      this.cargando = false;
    }
  },
  computed:{
    ...mapState(useContexto,['API','generos', 'usuario','peliculasFiltradas'])
  },  
  async mounted(){
    await this.obtenerPeliculas();
    await this.obtenerGeneros();
  },
  async created(){
    await this.protegerRuta();
  }
})
</script>


<style>

</style>