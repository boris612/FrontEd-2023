<template>
  <q-form @submit="login">
    <div class="row">
      <div class="col-4">
        <q-list class="q-pt-xl q-pl-xl col-5" >
          <q-item>
            <q-item-section>
              <q-item-label class="q-pb-xs">Korisničko ime</q-item-label>
              <q-input dense outlined v-model="v$.Username.$model"
                                      :error="v$.Username.$error"
                                      :error-message="ErrorsToString(v$.Username.$errors)"
              />
            </q-item-section>
          </q-item>
          <q-item>
            <q-item-section>
              <q-item-label class="q-pb-xs">Zaporka</q-item-label>
              <q-input dense outlined v-model="v$.Password.$model"
                                      :error="v$.Password.$error"
                                      :error-message="ErrorsToString(v$.Password.$errors)"
              />
            </q-item-section>
          </q-item>
          <q-item>
            <q-item-section>
              <q-btn flat label="OK"  color="primary" dense type="submit"/>
            </q-item-section>
          </q-item>
        </q-list>
      </div>
    </div>

  </q-form>
</template>


<script setup lang="ts">

import { useVuelidate } from '@vuelidate/core'
import { required, maxLength } from '@vuelidate/validators'
import { ref } from 'vue';
import { ErrorsToString, showRequestError} from 'src/services/notifications'
import { login as apilogin}  from 'src/services/api-client'
import { useIsAuthenticatedStore } from 'src/stores/login-store';
import { useRouter } from 'vue-router';
const router = useRouter();

const loginModel = ref({
  Username : '',
  Password : ''
})


const rules = {
  Username : {required},
  Password : {required}
}

const v$ = useVuelidate(rules, loginModel)

async function login() {
  await v$.value.$validate();
  if (v$.value.$error) return;

  const ok = await apilogin(loginModel.value.Username, loginModel.value.Password, showRequestError );
  if (ok)
  {
    useIsAuthenticatedStore().storeisAuthenticated(true)
    router.push({name : 'home'})
  }


}

</script>
