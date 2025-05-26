import { useAppDispatch, useAppSelector } from './hooks';
import { useEffect } from "react";
import { setResources } from "./store";
import { useResources } from "./hooks/useResources";
import { localResources } from "./data/Defaults.json";

function App() {
  const dispatch = useAppDispatch();
  const { resources } = useAppSelector((state) => {
    return state;
  })

  useEffect(() => {
    const ResourcesAction = setResources(localResources);
    dispatch(ResourcesAction);
  }, []);

  useResources();

  console.log(resources);

  return (
    <h1 className="text-3xl font-bold">
      Hello world!
    </h1>
  );
}

export default App
