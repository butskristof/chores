<template>
  <Menubar :model="menuBarItems">
    <template #item="{ item, props, hasSubmenu }">
      <router-link
        v-if="item.route"
        v-slot="{ href, navigate }"
        :to="{ name: item.route }"
        custom
      >
        <a
          :href="href"
          v-bind="props.action"
          @click="navigate"
        >
          <span :class="item.icon" />
          <span class="ml-2">{{ item.label }}</span>
        </a>
      </router-link>
      <a
        v-else
        :href="item.url"
        :target="item.target"
        v-bind="props.action"
      >
        <span :class="item.icon" />
        <span class="ml-2">{{ item.label }}</span>
        <span
          v-if="hasSubmenu"
          class="pi pi-fw pi-angle-down ml-2"
        />
      </a>
    </template>
    <template #end>
      <Avatar
        v-if="false"
        icon="pi pi-user"
        shape="circle"
      />
      <Button
        type="button"
        icon="pi pi-user"
        aria-haspopup="true"
        aria-controls="user_menu"
        @click="toggleUserMenu"
      />
      <Menu
        id="user_menu"
        ref="userMenu"
        :model="userMenuItems"
        :popup="true"
      />
    </template>
  </Menubar>
</template>

<script setup>
import { routes } from '@/router/routes.js';
import { ref } from 'vue';

const menuBarItems = [
  {
    label: 'Chores',
    icon: 'pi pi-check',
    route: routes.chores.name,
  },
  {
    label: 'Tags',
    icon: 'pi pi-tags',
    route: routes.tags.name,
  },
];
const userMenu = ref();
const userMenuItems = [
  {
    label: 'Log out',
    icon: 'pi pi-sign-out',
  },
];
const toggleUserMenu = (event) => userMenu.value.toggle(event);
</script>

<style scoped lang="scss">
a {
  display: flex;
  flex-direction: row;
  gap: 0.5rem;
}
</style>
