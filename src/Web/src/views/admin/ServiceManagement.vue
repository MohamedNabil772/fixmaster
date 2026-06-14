<template>
  <div class="flex flex-wrap mt-4">
    <div class="w-full mb-12 px-4">
      <div
        class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded bg-white"
      >
        <div class="rounded-t mb-0 px-4 py-3 border-0">
          <div class="flex flex-wrap items-center">
            <div class="relative w-full px-4 max-w-full flex-grow flex-1">
              <h3 class="font-semibold text-lg text-blueGray-700">
                Service Management
              </h3>
            </div>
            <div class="relative w-full px-4 max-w-full flex-grow flex-1 text-right">
              <button
                class="bg-lightBlue-500 text-white active:bg-lightBlue-600 font-bold uppercase text-xs px-4 py-2 rounded shadow hover:shadow-md outline-none focus:outline-none mr-1 ease-linear transition-all duration-150"
                type="button"
                @click="showAddModal = true"
              >
                Add New Service
              </button>
            </div>
          </div>
        </div>
        <div class="block w-full overflow-x-auto">
          <!-- Projects table -->
          <table class="items-center w-full bg-transparent border-collapse">
            <thead>
              <tr>
                <th
                  class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100"
                >
                  Service Name
                </th>
                <th
                  class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100"
                >
                  Category
                </th>
                <th
                  class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100"
                >
                  Base Price
                </th>
                <th
                  class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100"
                >
                  Status
                </th>
                <th
                  class="px-6 align-middle border border-solid py-3 text-xs uppercase border-l-0 border-r-0 whitespace-nowrap font-semibold text-left bg-blueGray-50 text-blueGray-500 border-blueGray-100"
                ></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="service in services" :key="service.id">
                <th
                  class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 text-left flex items-center"
                >
                  <span class="ml-3 font-bold text-blueGray-600">
                    {{ service.name }}
                  </span>
                </th>
                <td
                  class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4"
                >
                  {{ service.category }}
                </td>
                <td
                  class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4"
                >
                  ${{ service.basePrice }}
                </td>
                <td
                  class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4"
                >
                  <i
                    class="fas fa-circle mr-2"
                    :class="[service.active ? 'text-emerald-500' : 'text-red-500']"
                  ></i>
                  {{ service.active ? 'Active' : 'Inactive' }}
                </td>
                <td
                  class="border-t-0 px-6 align-middle border-l-0 border-r-0 text-xs whitespace-nowrap p-4 text-right"
                >
                  <button class="text-blueGray-500 py-1 px-3" type="button">
                    <i class="fas fa-ellipsis-v"></i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Simple Add Modal Placeholder -->
    <div v-if="showAddModal" class="fixed z-50 inset-0 overflow-y-auto">
      <div class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div class="fixed inset-0 transition-opacity" aria-hidden="true">
          <div class="absolute inset-0 bg-gray-500 opacity-75"></div>
        </div>
        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>
        <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
             <h3 class="text-lg leading-6 font-medium text-gray-900">Add New Service</h3>
             <div class="mt-4">
                <input type="text" placeholder="Service Name" class="border px-3 py-2 w-full mb-2" v-model="newService.name" />
                <input type="text" placeholder="Category" class="border px-3 py-2 w-full mb-2" v-model="newService.category" />
                <input type="number" placeholder="Base Price" class="border px-3 py-2 w-full mb-2" v-model="newService.basePrice" />
             </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button type="button" @click="addService" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-lightBlue-600 text-base font-medium text-white hover:bg-lightBlue-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm">
              Save
            </button>
            <button type="button" @click="showAddModal = false" class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm">
              Cancel
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';

interface Service {
  id: number;
  name: string;
  category: string;
  basePrice: number;
  active: boolean;
}

export default defineComponent({
  name: "ServiceManagement",
  setup() {
    const showAddModal = ref(false);
    const services = ref<Service[]>([
      { id: 1, name: "Plumbing Repair", category: "Plumbing", basePrice: 50, active: true },
      { id: 2, name: "Electrical Wiring", category: "Electrical", basePrice: 75, active: true },
      { id: 3, name: "House Cleaning", category: "Cleaning", basePrice: 40, active: false },
    ]);

    const newService = ref({
        name: '',
        category: '',
        basePrice: 0
    });

    const addService = () => {
        services.value.push({
            id: services.value.length + 1,
            name: newService.value.name,
            category: newService.value.category,
            basePrice: newService.value.basePrice,
            active: true
        });
        showAddModal.value = false;
        newService.value = { name: '', category: '', basePrice: 0 };
    };

    return {
      services,
      showAddModal,
      newService,
      addService
    };
  },
});
</script>
