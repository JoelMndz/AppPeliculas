<template>
  <div class="row justify-content-center">
    <div class="col-10 col-md-5 mt-5">
      <div class="card">
        <div class="card-header">
          <h4 class="text-center">Iniciar sesión</h4>
        </div>
        <form @submit.prevent="procesarFormulario" class="card-body">
          <Error v-if="error" />
          <div class="mb-3">
            <label for="exampleInputEmail1" class="form-label">Email</label>
            <input type="email" class="form-control" id="exampleInputEmail1" placeholder="example@gmail.com" autocomplete="off" required v-model="email">
          </div>
          <div class="mb-3">
            <label for="exampleInputPassword1" class="form-label">Password</label>
            <input type="password" class="form-control" id="exampleInputPassword1" minlength="8" autocomplete="off"
              required v-model="password">
          </div>
          <button :disabled="cargando" type="submit" class="btn btn-primary w-100">Ingresar</button>
          <div class="mt-3">
            <RouterLink to="/registro">Registrarse</RouterLink>
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
  components:{
    Error
  },
  setup() {
    const email = ref('');
    const password = ref('');
    const cargando = ref(false);

    return {
      email,
      password,
      cargando
    }
  },
  methods:{
    ...mapActions(useContexto, ['login']),
    async procesarFormulario(){
      this.cargando = true;
      this.login(this.email, this.password);
      this.cargando = false;
    }
  },
  computed:{
    ...mapState(useContexto, ['error']),
  }
})
</script>


<style></style>