<template>
  <nav
    class="navbar is-fixed-top"
    :class="{ 'has-shadow': !isOpen }"
    role="navigation"
    aria-label="main navigation"
  >
    <div class="navbar-brand">
      <router-link
        class="navbar-item is-size-4 has-text-weight-semibold"
        :to="{ name: routes.home.name }"
      >
        Chores
      </router-link>

      <a
        role="button"
        class="navbar-burger"
        :class="{ 'is-active': isOpen }"
        aria-label="menu"
        aria-expanded="false"
        @click="isOpen = !isOpen"
      >
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
      </a>
    </div>

    <div
      class="navbar-menu"
      :class="{ 'is-active': isOpen }"
    >
      <div class="navbar-start">
        <router-link
          :to="{ name: routes.tags.name }"
          class="navbar-item"
          >Tags</router-link
        >
      </div>

      <div class="navbar-end is-pulled-right">
        <div class="navbar-item">
          <div class="buttons">
            <button
              v-if="authStore.isAuthenticated"
              type="button"
              class="button is-light"
              @click="authService.logout"
            >
              Log out
            </button>
            <button
              v-else
              type="button"
              class="button is-primary"
              @click="authService.login"
            >
              Log in
            </button>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { routes } from '@/router/routes.js';
import { useAuthStore } from '@/stores/auth.js';
import authService from '@/services/auth/auth.service.js';
import { ref } from 'vue';

const authStore = useAuthStore();
const isOpen = ref(false);
</script>

<style scoped lang="scss"></style>
