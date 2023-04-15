import {defineStore} from 'pinia';
import Axios, { AxiosError } from 'axios';
import router from '@/router';
import { convertBase64 } from '@/utils/convertBase64';

interface IState{
  usuario: null | any,
  peliculas: any[],
  peliculasFiltradas: any[],
  generos: any[],
  API: string,
  error: string | null
}

export const useContexto = defineStore('contexto', {
  state: ():IState => {
    return {
      peliculas: [],
      peliculasFiltradas: [],
      generos: [],
      usuario: null,
      API:'http://localhost:5080',
      error: null,
    }
  },
  
  actions:{
    async login(email:string, password:string){
      try {
        this.resetarError();
        await Axios({
          url: `${this.API}/login`,
          method:'POST',
          data:{
            email,
            password
          }
        })
        .then(x => {
          this.usuario = x.data;
          router.push('/peliculas')
        })
        .catch((x:any)=> {
          this.error = x.response?.data?.mensaje ?? 'Ocurrio un error'
        })
              
      } catch (error) {
        console.log(error);        
      }
    },

    async registro(usuario:any){
      try {
        await Axios({
          url: `${this.API}/registro`,
          method:'POST',
          data:usuario
        })
        .then(x => {
          this.usuario = x.data;
          router.push('/')
        })
        .catch((x:any)=> {
          this.error = x.response?.data?.mensaje ?? 'Ocurrio un error'
        })    
      } catch (error) {
        console.log(error);        
      }
    },

    resetarError(){
      this.error = null;
    },

    cerrarSesion(){
      this.usuario = null;
      router.push('/');
    },

    async obtenerPeliculas(){
      try {
        await Axios({
          url: `${this.API}/pelicula/${this.usuario?.id}`,
          method:'GET',
        })
        .then(x => {         
          this.peliculas = x.data;
          this.peliculasFiltradas = x.data;
        })
      } catch (error) {
        console.log(error);        
      }
    },

    async obtenerGeneros(){
      try {
        await Axios({
          url: `${this.API}/genero`,
          method:'GET',
        })
        .then(x => {
          this.generos = x.data;
        })
      } catch (error) {
        console.log(error);        
      }
    },

    async eliminarGenero(id:number){
      try {
        await Axios({
          url: `${this.API}/genero/${id}`,
          method:'DELETE',
        })
        .then(x => {
          this.generos = this.generos.filter(x => x.id !== id);
        })
      } catch (error) {
        console.log(error);        
      }
    },

    async agregarGenero(descripcion:string){
      try {
        await Axios({
          url: `${this.API}/genero`,
          method:'POST',
          data:{
            descripcion
          }
        })
        .then(x => {
          this.generos.push(x.data);
        })
      } catch (error) {
        console.log(error);        
      }
    },

    async actualizarGenero(id:number, descripcion:string){
      try {
        await Axios({
          url: `${this.API}/genero`,
          method:'PATCH',
          data:{
            id,
            descripcion
          }
        })
        .then(r => {
          this.generos = this.generos.map(x => x.id === id ? {...x,descripcion} : x);
        })
      } catch (error) {
        console.log(error);        
      }
    }, 

    async agregarPelicula(pelicula:any){
      try {
        pelicula.fotoBase64 = await convertBase64(pelicula.fotoBase64);      
        await Axios({
          url: `${this.API}/pelicula`,
          method:'POST',
          data:{
            titulo: pelicula.titulo,
            director: pelicula.director,
            anio: pelicula.anio,
            fotoBase64: pelicula.fotoBase64,
            idGenero: pelicula.idGenero,
            idUsuario: this.usuario.id
          }
        })
        .then(async r => {
          await this.obtenerPeliculas();
        })
      } catch (error) {
        console.log(error);        
      }
    },

    async actualizarPelicula(pelicula:any, id:number){
      try {  
        await Axios({
          url: `${this.API}/pelicula`,
          method:'PATCH',
          data: {
            id: id,
            titulo: pelicula.titulo,
            director: pelicula.director,
            anio: pelicula.anio,
            idGenero: pelicula.idGenero,
          }
        })
        .then(async r => {
          await this.obtenerPeliculas();
        })
      } catch (error) {
        console.log(error);        
      }
    },

    async eliminarPelicula(id:number){
      try {
        await Axios({
          url: `${this.API}/pelicula/${id}`,
          method:'DELETE',
        })
        .then(x => {
          this.peliculas = this.peliculas.filter(x => x.Id !== id);
        })
      } catch (error) {
        console.log(error);        
      }
    },

    filtrarPeliculas(filtro:any){
      this.peliculasFiltradas = this.peliculas;
      if(filtro.idGenero != ''){
        this.peliculasFiltradas = this.peliculas.filter((x:any) => x.idGenero === filtro.idGenero);
      }
      else if(filtro.titulo != ''){
        this.peliculasFiltradas = this.peliculas.filter((x:any) => x.titulo.toLowerCase().search(filtro.titulo.toLowerCase()) !== -1);
      }
    },

    async protegerRuta(){
      if(!this.usuario){
        await router.push('/') 
      }
    }
  }
})