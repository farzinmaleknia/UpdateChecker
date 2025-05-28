
import { fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { BaseQueryFn } from "@reduxjs/toolkit/query";
import { useAppDispatch, useAppSelector } from '../../hooks';

export const dynamicBaseQuery: BaseQueryFn<any, any, any> = async ( args, api, extraOptions ) => {
  
  // const { resources } = useAppSelector((state) => {
  //   return state.resources;
  // })

  const state = api.getState() as any;
  const baseUrl = state.resources?.resources?.Defaults?.apiBaseUrl; 

  const rawBaseQuery = fetchBaseQuery({ baseUrl });

  return rawBaseQuery(args, api, extraOptions);
};
