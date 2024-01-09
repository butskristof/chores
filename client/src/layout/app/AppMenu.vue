<template>
  <ul class="layout-menu">
    <template
      v-for="(item, i) in menuItems"
      :key="item"
    >
      <app-menu-item
        v-if="!item.separator"
        :item="item"
        :index="i"
      ></app-menu-item>
      <li
        v-if="item.separator"
        class="menu-separator"
      ></li>
    </template>
  </ul>
</template>

<script setup>
import { ref } from 'vue';

import AppMenuItem from './AppMenuItem.vue';

const menuItems = ref([
  {
    items: [
      {
        label: 'Chores',
        icon: 'pi pi-check',
        to: '/chores',
      },
      {
        label: 'Tags',
        icon: 'pi pi-tags',
        to: '/tags',
      },
    ],
  },
]);
</script>
