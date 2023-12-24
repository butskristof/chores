<template>
  <template v-if="chore">
    <div class="header">
      <div class="left">
        <h1>{{ chore.name }}</h1>
        <p>Should happen every {{ chore.interval }} days</p>
        <p>Happened last on {{ formattedLastIteration }}, due again in {{ due }}</p>
      </div>
      <div class="actions">
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
import { addDays, format, formatDistance } from 'date-fns';

const choreId = useRouteParams('id');
const choreQuery = useChoresApiChore(choreId);
const chore = computed(() => choreQuery.data.value);

const lastIteration = computed(() => {
  if (chore.value.iterations.length === 0) return null;
  const last = chore.value.iterations.reduce(
    (max, current) => (new Date(max.date) > new Date(current.date) ? max : current),
    chore.value.iterations[0],
  );
  return new Date(last.date);
});
const formattedLastIteration = computed(() => {
  if (lastIteration.value == null) return null;
  return format(lastIteration.value, 'LLLL do');
});
const due = computed(() => {
  if (lastIteration.value == null) return 0;
  const expectedNextIteration = addDays(lastIteration.value, chore.value.interval);
  return formatDistance(lastIteration.value, expectedNextIteration);
});

const router = useRouter();

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

  .actions {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
    align-items: flex-start;
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
