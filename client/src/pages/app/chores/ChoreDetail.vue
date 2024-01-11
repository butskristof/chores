<template>
  <template v-if="chore">
    <div class="card">
      <Toolbar class="header">
        <template #start>
          <h1>{{ chore.name }}</h1>
        </template>
        <template #end>
          <Button
            type="button"
            @click="showEditTags = true"
          >
            edit tags
          </Button>
          <Button
            type="button"
            @click="showEdit = true"
          >
            edit
          </Button>
          <Button
            type="button"
            @click="showDelete = true"
          >
            delete
          </Button>
          <EditChoreTags
            v-if="showEditTags"
            :open="true"
            :chore="chore"
            @close="showEditTags = false"
          />
          <EditChore
            v-if="showEdit"
            :open="true"
            :chore="chore"
            @close="showEdit = false"
          />
          <DeleteChore
            v-if="showDelete"
            :open="true"
            :chore="chore"
            @close="closeDelete"
          />
        </template>
      </Toolbar>
      <p>Should happen every {{ chore.interval }} days</p>
      <ChoreLastIteration :chore="chore" />
      <p>Tags: {{ chore.tags.map((t) => t.name) }}</p>
    </div>

    <div class="card">
      <ChoreNotes :chore="chore" />
    </div>

    <div class="card">
      <ChoreIterations :chore="chore" />
    </div>
  </template>
</template>

<script setup>
import { useRouteParams } from '@vueuse/router';
import { computed, ref } from 'vue';
import { useChoresApiChore } from '@/composables/queries/chores-api.js';
import EditChore from '@/components/chores/common/EditChore.vue';
import DeleteChore from '@/components/chores/detail/DeleteChore.vue';
import { useRouter } from 'vue-router';
import { routes } from '@/router/routes.js';
import ChoreNotes from '@/components/chores/detail/ChoreNotes.vue';
import ChoreIterations from '@/components/chores/detail/ChoreIterations.vue';
import ChoreLastIteration from '@/components/chores/common/ChoreLastIteration.vue';
import EditChoreTags from '@/components/chores/detail/EditChoreTags.vue';
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button';

const choreId = useRouteParams('id');
const choreQuery = useChoresApiChore(choreId);
const chore = computed(() => choreQuery.data.value);

const router = useRouter();

//#region edit tags
const showEditTags = ref(false);
//#endregion

//#region edit
const showEdit = ref(false);
//#endregion

//#region delete
const showDelete = ref(false);
const closeDelete = (deleted) => {
  showDelete.value = false;
  if (deleted === true) {
    router.push({ name: routes.chores.name });
  }
};
//#endregion
</script>

<style scoped lang="scss">
.header {
  margin-bottom: 2rem;

  h1 {
    font-size: 1.75rem;
    margin-block: 0;
  }
}

hr {
  margin-top: 0.5rem;
  margin-bottom: 0.5rem;
}
</style>
