<template>
  <template v-if="chore">
    <div class="header">
      <div class="left">
        <h1>{{ chore.name }}</h1>
      </div>
      <div class="actions">
        <button
          type="button"
          @click="showEditTags = true"
        >
          edit tags
        </button>
        <EditChoreTags
          v-if="showEditTags"
          :open="true"
          :chore="chore"
          @close="showEditTags = false"
        />
        <button
          type="button"
          @click="showEdit = true"
        >
          edit
        </button>
        <EditChore
          v-if="showEdit"
          :open="true"
          :chore="chore"
          @close="showEdit = false"
        />
        <button
          type="button"
          @click="showDelete = true"
        >
          delete
        </button>
        <DeleteChore
          v-if="showDelete"
          :open="true"
          :chore="chore"
          @close="closeDelete"
        />
      </div>
    </div>
    <p>Should happen every {{ chore.interval }} days</p>
    <ChoreLastIteration :chore="chore" />
    <p>Tags: {{ chore.tags.map((t) => t.name) }}</p>

    <hr />

    <ChoreNotes :chore="chore" />

    <hr />

    <ChoreIterations :chore="chore" />
  </template>
</template>

<script setup>
import { useRouteParams } from '@vueuse/router';
import { computed, ref } from 'vue';
import { useChoresApiChore } from '@/composables/queries/chores-api';
import EditChore from '@/components/chores/common/EditChore.vue';
import DeleteChore from '@/components/chores/detail/DeleteChore.vue';
import { useRouter } from 'vue-router';
import { routes } from '@/router/routes';
import ChoreNotes from '@/components/chores/detail/ChoreNotes.vue';
import ChoreIterations from '@/components/chores/detail/ChoreIterations.vue';
import ChoreLastIteration from '@/components/chores/common/ChoreLastIteration.vue';
import EditChoreTags from '@/components/chores/detail/EditChoreTags.vue';

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
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap-reverse;

  .actions {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
    align-items: flex-start;
    margin-left: auto;
  }
}

hr {
  margin-top: 0.5rem;
  margin-bottom: 0.5rem;
}

ul {
  list-style: circle;
  li {
    //display: block;
  }
}
</style>
