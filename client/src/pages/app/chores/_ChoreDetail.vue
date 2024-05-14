<template>
  <template v-if="chore">
    <div class="card">
      <Toolbar class="header">
        <template #start>
          <h1>{{ chore.name }}</h1>
        </template>
        <template #end>
          <div class="actions">
            <Button
              type="button"
              label="Edit tags"
              icon="pi pi-tags"
              @click="showEditTags = true"
            />

            <Button
              type="button"
              label="Edit"
              icon="pi pi-pencil"
              @click="showEdit = true"
            />

            <Button
              type="button"
              label="Delete"
              icon="pi pi-trash"
              severity="danger"
              @click="showDelete = true"
            />
          </div>

          <EditChoreTags
            v-if="showEditTags"
            :chore="chore"
            @close="showEditTags = false"
          />

          <EditChore
            v-if="showEdit"
            :chore="chore"
            @close="showEdit = false"
          />

          <DeleteChore
            v-if="showDelete"
            :chore="chore"
            @close="closeDelete"
          />
        </template>
      </Toolbar>

      <p>Should happen every {{ chore.interval }} days</p>
      <ChoreLastIteration :chore="chore" />

      <div class="tags">
        <Tag
          v-for="tag in chore.tags"
          :key="tag.id"
          :value="tag.name"
        />
      </div>
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
import EditChore from '@/components/chores/common/_EditChore.vue';
import DeleteChore from '@/components/chores/detail/DeleteChore.vue';
import { useRouter } from 'vue-router';
import { routes } from '@/router/routes.js';
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
  margin-bottom: 2rem;
  gap: 1rem;

  :deep(.p-toolbar-group-right) {
    margin-left: auto;
  }

  h1 {
    //font-size: 1.75rem;
    margin-block: 0;
  }

  .actions {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    gap: 0.5rem;
    justify-content: flex-end;
  }
}

.tags {
  margin-top: 1rem;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  gap: 0.5rem;
}
</style>
