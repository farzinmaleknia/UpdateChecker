import { useFetchResourcesQuery } from "../store";



export const useResources: (args: string)=>void = (mm : string) => {
  const { data, error, isLoading } = useFetchResourcesQuery();

  console.log(data, mm)
  
};