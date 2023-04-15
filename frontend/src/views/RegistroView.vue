<template>
  <div class="row justify-content-center">
    <div class="col-10 col-md-5 mt-5">
      <div class="card">
        <div class="card-header">
          <h4 class="text-center">Registro</h4>
        </div>
        <form @submit.prevent="procesarFormulario" class="card-body">
          <Error v-if="error" />
          <div class="mb-3">
            <label class="form-label">Nombres</label>
            <input type="text" class="form-control" autocomplete="off" required v-model="nombres">
          </div>
          <div class="mb-3">
            <label class="form-label">Apellidos</label>
            <input type="text" class="form-control" autocomplete="off" required v-model="apellidos">
          </div>
          <div class="mb-3">
            <label  class="form-label">Email</label>
            <input type="email" class="form-control"
              placeholder="example@gmail.com" autocomplete="off" required v-model="email">
          </div>
          <div class="mb-3">
            <label class="form-label">Password</label>
            <input type="password" class="form-control" minlength="8"
              required v-model="password">
          </div>
          <button :disabled="cargando" type="submit" class="btn btn-primary w-100">Registrarse</button>
          <div class="mt-3">
            <RouterLink to="/">Regresar al login</RouterLink>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { useContexto } from '@/stores/contexto';
import { mapActions, mapState } from 'pinia';
import { defineComponent, ref } from 'vue'
import Error from '@/components/Error.vue'

export default defineComponent({
  components:{Error},
  setup() {
    const nombres = ref('');
    const apellidos = ref('');
    const email = ref('');
    const password = ref('');
    const cargando = ref(false);

    return {
      email,
      password,
      cargando,
      nombres,
      apellidos
    }
  },
  methods:{
    ...mapActions(useContexto, ['registro']),
    async procesarFormulario(){
      this.cargando = true;
      this.registro({nombres:this.nombres, apellidos:this.apellidos, email:this.email, password:this.password});
      this.cargando = false;
    }
  },
  computed:{
    ...mapState(useContexto,['error']),
  }
})
</script>


<style></style>