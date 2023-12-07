<template>
  <template v-if="chore">
    <div class="header">
      <div class="left">
        <h1>{{ chore.title }}</h1>
        <p>Should happen every 10 days</p>
        <p>Happened last on November 28th, due again in {{ due }} days</p>
      </div>
      <div class="actions">
        <button type="button">edit</button>
        <button type="button">delete</button>
      </div>
    </div>
    <hr />
    <div class="header">
      <h2>Notes</h2>
      <div class="actions">
        <button type="button">edit</button>
      </div>
    </div>
    <hr />
    <div class="header">
      <h2>Iterations</h2>
      <div class="actions">
        <button type="button">add</button>
      </div>
    </div>
    <ul>
      <li>
        <div>date</div>
        <div>notes</div>
      </li>
      <li>
        <div>date</div>
        <div>notes</div>
      </li>
    </ul>
  </template>
</template>

<script setup>
import { useRouteParams } from '@vueuse/router';
import { computed } from 'vue';
import { useChoresApiChore } from '@/composables/queries/chores-api';

const choreId = useRouteParams('id');
const choreQuery = useChoresApiChore(choreId);
const chore = computed(() => choreQuery.data.value);

const due = Math.floor(Math.random() * 10) - 1;
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
