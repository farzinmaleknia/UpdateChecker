
import { fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { BaseQueryFn } from "@reduxjs/toolkit/query";
import type { FetchArgs, FetchBaseQueryError } from '@reduxjs/toolkit/query';

export const dynamicBaseQuery: BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> = async (
  args,
  api,
  extraOptions
) => {
  const state = api.getState() as any;
  const baseUrl = state.resources?.resources?.Defaults?.apiBaseUrl; 

  console.log(state.resources)
  const rawBaseQuery = fetchBaseQuery({ baseUrl });

  return rawBaseQuery(args, api, extraOptions);
};
