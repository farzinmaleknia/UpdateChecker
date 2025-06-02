import { useEffect } from "react";
import { useUpdateLoginMutation } from "../store";
import type { LoginForUpdateDTO } from "../store/interfaces/Update/LoginForUpdateDTO";
import UpdateLogin from "./UpdateLogin";

const Update = () => {
  const [updateLogin, results] = useUpdateLoginMutation();

  const onLoginFormSubmited = (auth : LoginForUpdateDTO) => {
    updateLogin(auth);

  }

  useEffect(() => {
    console.log(results);
  }, [results])

  return (
    <div>
      <UpdateLogin onHandleSubmit={onLoginFormSubmited} />
    </div>
  );
};

export default Update;
