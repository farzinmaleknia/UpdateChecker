import type {
  BaseQueryFn,
  FetchArgs,
  FetchBaseQueryError,
} from "@reduxjs/toolkit/query";
import type { ResultClass } from "../interfaces/ResultClass/ResultClass";
import type { LoginForUpdateDTO } from "../interfaces/Update/LoginForUpdateDTO";
import type { Update } from "../interfaces/Update/Update";
import { createApi } from "@reduxjs/toolkit/query/react";
import { dynamicBaseQuery } from "./dynamicBaseQuery";

const updateApi = createApi({
  reducerPath: "updateLogin",
  baseQuery: dynamicBaseQuery as BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
  >,
  endpoints(builder) {
    return {
      updateLogin: builder.mutation<ResultClass<Update>, LoginForUpdateDTO>({
        query: (auth: LoginForUpdateDTO) => {
          
          console.log(auth);
          return {
            url: "/Update/LoginForUpdate",
            method: "POST",
            body: auth,
          };
        },
      }),
    };
  },
});

export const { useUpdateLoginMutation } = updateApi;
export { updateApi };
