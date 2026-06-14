<template>
  <nav
    class="md:left-0 md:block md:fixed md:top-0 md:bottom-0 md:overflow-y-auto md:flex-row md:flex-nowrap md:overflow-hidden shadow-xl bg-white flex flex-wrap items-center justify-between relative md:w-64 z-10 py-4 px-6"
  >
    <div
      class="md:flex-col md:items-stretch md:min-h-full md:flex-nowrap px-0 flex flex-wrap items-center justify-between w-full mx-auto"
    >
      <!-- Brand -->
      <router-link
        class="md:block text-left md:pb-2 text-blueGray-600 mr-0 inline-block whitespace-nowrap text-sm uppercase font-bold p-4 px-0"
        to="/"
      >
        FixMaster
      </router-link>
      
      <!-- Collapse -->
      <div
        class="md:flex md:flex-col md:items-stretch md:opacity-100 md:relative md:mt-4 md:shadow-none shadow absolute top-0 left-0 right-0 z-40 overflow-y-auto overflow-x-hidden h-auto items-center flex-1 rounded"
        v-bind:class="collapseShow"
      >
        <!-- Administrative Control -->
        <template v-if="isAdmin || isSuperAdmin">
          <hr class="my-4 md:min-w-full" />
          <h6 class="md:min-w-full text-blueGray-500 text-xs uppercase font-bold block pt-1 pb-4 no-underline">
            Administrative Control
          </h6>

          <ul class="md:flex-col md:min-w-full flex flex-col list-none">
            <li class="items-center">
              <router-link to="/admin/dashboard" v-slot="{ href, navigate, isActive }">
                <a :href="href" @click="navigate" class="text-xs uppercase py-3 font-bold block" :class="[isActive ? 'text-emerald-500' : 'text-blueGray-700']">
                  <i class="fas fa-tv mr-2 text-sm"></i> Overview
                </a>
              </router-link>
            </li>

            <li v-if="isSuperAdmin" class="items-center">
              <router-link to="/admin/users" v-slot="{ href, navigate, isActive }">
                <a :href="href" @click="navigate" class="text-xs uppercase py-3 font-bold block" :class="[isActive ? 'text-emerald-500' : 'text-blueGray-700']">
                  <i class="fas fa-users-cog mr-2 text-sm"></i> User Management
                </a>
              </router-link>
            </li>

            <li class="items-center">
              <router-link to="/admin/bids" v-slot="{ href, navigate, isActive }">
                <a :href="href" @click="navigate" class="text-xs uppercase py-3 font-bold block" :class="[isActive ? 'text-emerald-500' : 'text-blueGray-700']">
                  <i class="fas fa-gavel mr-2 text-sm"></i> Bids Management
                </a>
              </router-link>
            </li>
          </ul>
        </template>

        <!-- Master Section -->
        <template v-if="isMaster">
          <hr class="my-4 md:min-w-full" />
          <h6 class="md:min-w-full text-blueGray-500 text-xs uppercase font-bold block pt-1 pb-4 no-underline">
            Master Menu
          </h6>
          <ul class="md:flex-col md:min-w-full flex flex-col list-none">
            <li class="items-center">
              <router-link to="/dashboard" v-slot="{ href, navigate, isActive }">
                <a :href="href" @click="navigate" class="text-xs uppercase py-3 font-bold block" :class="[isActive ? 'text-emerald-500' : 'text-blueGray-700']">
                  <i class="fas fa-tv mr-2 text-sm"></i> My Bids
                </a>
              </router-link>
            </li>
            <li class="items-center">
              <router-link to="/browse-requests" v-slot="{ href, navigate, isActive }">
                <a :href="href" @click="navigate" class="text-xs uppercase py-3 font-bold block" :class="[isActive ? 'text-emerald-500' : 'text-blueGray-700']">
                  <i class="fas fa-search mr-2 text-sm"></i> Browse Requests
                </a>
              </router-link>
            </li>
          </ul>
        </template>

        <!-- Client Section -->
        <template v-if="isClient">
          <hr class="my-4 md:min-w-full" />
          <h6 class="md:min-w-full text-blueGray-500 text-xs uppercase font-bold block pt-1 pb-4 no-underline">
            Client Menu
          </h6>
          <ul class="md:flex-col md:min-w-full flex flex-col list-none">
            <li class="items-center">
              <router-link to="/dashboard" v-slot="{ href, navigate, isActive }">
                <a :href="href" @click="navigate" class="text-xs uppercase py-3 font-bold block" :class="[isActive ? 'text-emerald-500' : 'text-blueGray-700']">
                  <i class="fas fa-tv mr-2 text-sm"></i> My Requests
                </a>
              </router-link>
            </li>
            <li class="items-center">
              <router-link to="/post-request" v-slot="{ href, navigate, isActive }">
                <a :href="href" @click="navigate" class="text-xs uppercase py-3 font-bold block" :class="[isActive ? 'text-emerald-500' : 'text-blueGray-700']">
                  <i class="fas fa-plus mr-2 text-sm"></i> Post New Request
                </a>
              </router-link>
            </li>
          </ul>
        </template>
      </div>
    </div>
  </nav>
</template>

<script lang="ts">
import { useAuthStore } from "@/stores/auth";
import { mapState } from "pinia";

export default {
  data() {
    return {
      collapseShow: "hidden",
    };
  },
  computed: {
    ...mapState(useAuthStore, ["user", "isAuthenticated"]),
    isAdmin() {
      return this.user?.role === "Admin";
    },
    isSuperAdmin() {
      return this.user?.role === "SuperAdmin";
    },
    isMaster() {
      return this.user?.role === "Master";
    },
    isClient() {
      return this.user?.role === "Client";
    },
  },
  methods: {
    toggleCollapseShow: function (classes) {
      this.collapseShow = classes;
    },
  },
};
</script>
