<template>
  <nav class="container-fluid">
    <ul>
      <li>&lt; back</li>
    </ul>
    <ul>
      <li>
        <button
          type="button"
          role="link"
        >
          Log in
        </button>
      </li>
      <li>
        <details
          role="list"
          dir="rtl"
        >
          <summary
            aria-haspopup="listbox"
            role="link"
          >
            Menu
          </summary>
          <ul role="listbox">
            <li>
              <router-link :to="{ name: routes.tags.name }">Manage tags</router-link>
            </li>
            <li>
              <button
                type="button"
                role="link"
                @click="authService.logout"
              >
                Log out
              </button>
            </li>
          </ul>
        </details>
      </li>
    </ul>
  </nav>
</template>

<script setup>
import { routes } from '@/router/routes.js';
import authService from '@/services/auth/auth.service.js';
</script>

<style scoped lang="scss"></style>
