<template>
  <div class="q-pa-md">
    <q-table hide-pagination
      title="People"
      :rows="rows"
      :rows-per-page-options="[0]"
      :columns="columns"
    />
  </div>
</template>

<script setup lang="ts">

import { onMounted, ref } from 'vue';
import {fetchItems} from 'src/services/api-client'
import { QTableProps } from 'quasar';
import { showRequestError } from 'src/services/notifications';
import { formatDate } from 'src/services/dateutils';

interface Person
{
  FirstName : string,
  LastName : string,
  Address : string,
  Birthday : string
}

const rows = ref<Person[]>();

onMounted(async () => {
  rows.value = await fetchItems('people', showRequestError)
})

const columns : QTableProps["columns"] = [
    {name: 'FirstName',  field: 'FirstName', label : 'First Name', sortable : true},
    {name: 'LastName',  field: 'LastName', label : 'Last Name',  sortable : true},
    {name: 'Address',  field: 'Address', label : 'Address',  sortable : true},
    {name: 'Birthday',  field: 'Birthday', label : 'Birthday',  sortable : true, format : (val, row) => formatDate(val, 'yyyy-MM-dd') },
  ];

</script>
