<template>
  <div class="container mx-auto px-4 h-full flex items-center justify-center min-h-screen">
    <div class="flex content-center items-center justify-center h-full">
      <div class="w-full lg:w-6/12 px-4">
        <div class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-blueGray-200 border-0">
          <div class="rounded-t mb-0 px-6 py-6">
            <div class="text-center mb-3">
              <h1 class="text-blueGray-500 text-6xl font-bold">
                403
              </h1>
              <h6 class="text-blueGray-500 text-sm font-bold">
                Unauthorized Access
              </h6>
            </div>
            <hr class="mt-6 border-b-1 border-blueGray-300" />
          </div>
          <div class="flex-auto px-4 lg:px-10 py-10 pt-0">
            <div class="text-blueGray-400 text-center mb-3 font-bold">
              <p>You do not have permission to view this page.</p>
            </div>
            <div class="text-center mt-6">
              <router-link
                to="/"
                class="bg-blueGray-800 text-white active:bg-blueGray-600 text-sm font-bold uppercase px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 w-full ease-linear transition-all duration-150"
              >
                Go to Home
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// No additional logic needed for now
</script>
