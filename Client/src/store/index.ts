import { configureStore } from "@reduxjs/toolkit";
import { setupListeners } from "@reduxjs/toolkit/query";
import resourcesReducer from "./slices/resources";
import { setResources } from "./slices/resources";
import { resourcesApi } from "./apis/resourcesApi";
import { updateApi } from "./apis/updateApi";

export const store = configureStore({
  reducer: {
    [resourcesApi.reducerPath]: resourcesApi.reducer,
    resources: resourcesReducer,
    [updateApi.reducerPath]: updateApi.reducer,
  },
  middleware: (getDefaultMitddleware) => {
    return getDefaultMitddleware()
      .concat(resourcesApi.middleware)
      .concat(updateApi.middleware);
  },
});

setupListeners(store.dispatch);

export { useFetchResourcesQuery } from "./apis/resourcesApi";
export { useUpdateLoginMutation } from "./apis/updateApi";
export { setResources } from "./slices/resources";

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
