<template>
  <DropdownMenuRoot v-model:open="isOpen">
    <DropdownMenuTrigger
      aria-label="Menu actions"
      class="menu-trigger"
    >
      <Menu />
    </DropdownMenuTrigger>

    <DropdownMenuPortal class="portal">
      <DropdownMenuContent
        class="content"
        align="end"
      >
        <DropdownMenuItem class="menu-item">Manage tags</DropdownMenuItem>
        <DropdownMenuItem
          class="menu-item"
          @click="authService.logout"
          >Log out</DropdownMenuItem
        >
      </DropdownMenuContent>
    </DropdownMenuPortal>
  </DropdownMenuRoot>
</template>

<script setup>
import { Menu } from 'lucide-vue-next';
import {
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuPortal,
  DropdownMenuRoot,
  DropdownMenuTrigger,
} from 'radix-vue';
import { ref } from 'vue';
import authService from '@/services/auth/auth.service.js';

const isOpen = ref(true);
</script>

<style scoped lang="scss">
.menu-trigger {
  padding-inline: var(--size-2);
}

::v-deep(.content) {
  background-color: var(--surface-2);
  box-shadow: var(--shadow-2);
  min-width: var(--size-12);
}

.menu-item {
  text-align: right;
  padding-left: var(--size-3);
  padding-right: var(--size-2);
  padding-block: var(--size-1);

  //&:nth-child(even) {
  //  background-color: var(--surface-3);
  //}
}
</style>
