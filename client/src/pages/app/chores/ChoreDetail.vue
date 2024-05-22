<template>
  <div
    v-if="chore"
    class="chore-detail"
  >
    <div class="back-to-overview">
      <router-link :to="{ name: routes.chores.children.overview.name }">
        <PrimeButton
          icon="pi pi-arrow-left"
          label="Back to overview"
          severity="secondary"
        />
      </router-link>
    </div>

    <PageHeader
      :inline-padding="false"
      class="header"
    >
      <template #title>
        <div class="header-left">
          <h1>{{ chore.name }}</h1>
          <ChoreTags :tags="chore.tags" />
          <p class="chore-due"><i class="pi pi-clock"></i> every {{ chore.interval }} days</p>
        </div>
      </template>
      <template #actions>
        <div class="actions">
          <PrimeButton
            label="Edit tags"
            icon="pi pi-tags"
            @click="showEditTags = true"
          />
          <PrimeButton
            label="Edit"
            icon="pi pi-pencil"
            @click="showEdit = true"
          />
          <PrimeButton
            label="Delete"
            icon="pi pi-trash"
            severity="danger"
            @click="showDelete = true"
          />
        </div>
      </template>
    </PageHeader>

    <PrimeDivider />

    <ChoreNotes :chore="chore" />

    <PrimeDivider />

    <ChoreIterations :chore="chore" />

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
  </div>
</template>

<script setup>
import { useRouteParams } from '@vueuse/router';
import { useChoresApiChore } from '@/composables/queries/chores-api.js';
import { ref } from 'vue';
import PrimeButton from 'primevue/button';
import PageHeader from '@/components/common/PageHeader.vue';
import ChoreTags from '@/components/chores/common/ChoreTags.vue';
import EditChore from '@/components/chores/common/EditChore.vue';
import { routes } from '@/router/routes.js';
import { useRouter } from 'vue-router';
import DeleteChore from '@/components/chores/detail/DeleteChore.vue';
import EditChoreTags from '@/components/chores/detail/EditChoreTags.vue';
import ChoreNotes from '@/components/chores/detail/ChoreNotes.vue';
import PrimeDivider from 'primevue/divider';
import ChoreIterations from '@/components/chores/detail/ChoreIterations.vue';

const choreId = useRouteParams('id');
const { chore } = useChoresApiChore(choreId);

const router = useRouter();

const showEditTags = ref(false);
const showEdit = ref(false);
const showDelete = ref(false);
const closeDelete = (deleted) => {
  showDelete.value = false;
  if (deleted === true) router.push({ name: routes.chores.name });
};
</script>

<style scoped lang="scss">
@import '@/styles/_custom-vars.scss';
@import '@/styles/_utilities.scss';

.back-to-overview {
  @include media-min-width($md) {
    margin-bottom: 1rem;
  }
}

.header {
  align-items: flex-start;
}

.header-left {
  @include flex-column;
  gap: 1rem;
}

.chore-due {
  @include flex-row;
  align-items: center;

  i {
    margin-right: 0.5rem;
  }
}

.actions {
  @include flex-row;
  flex-wrap: wrap;
  justify-content: flex-end;
  gap: 1rem;
}
</style>
