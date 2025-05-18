import { useFetchResourcesQuery } from "./store";


function App() {
  const {data, error, isLoading} = useFetchResourcesQuery();

  if (data != null)
  {
    console.log(data.data.Titles.Cancel)
  }

  return (
    <h1 className="text-3xl font-bold">
      Hello world!
    </h1>
  );
}

export default App
