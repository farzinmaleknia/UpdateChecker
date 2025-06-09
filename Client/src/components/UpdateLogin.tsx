import type { LoginForUpdateDTO } from "../store/interfaces/Update/LoginForUpdateDTO";
import { useState } from "react";
import { useAppSelector } from "../hooks";
import { TextInput } from "./TextInput";
import { Button } from "./Button";

interface UpdateLoginProps {
  onHandleSubmit : (model: LoginForUpdateDTO) => void,
}

const UpdateLogin = ({onHandleSubmit}: UpdateLoginProps ) => {
  const [loginModel, setLoginModel] = useState<LoginForUpdateDTO>({
    Username: "",
    Password: "",
  });
  const resources = useAppSelector((state) => state.resources);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setLoginModel((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    onHandleSubmit(loginModel);
  };

  return (
    <div>
      <form onSubmit={handleSubmit} className="w-120 flex flex-col">
        <div className="flex flex-col self-center">
          <label className="font-bold mx-2" htmlFor="username">
            {resources?.Titles.Username} :
          </label>
          <TextInput
            name="Username"
            id="username"
            value={loginModel.Username}
            onChange={handleChange}
          ></TextInput>
          <label className="font-bold mx-2" htmlFor="password">
            {resources?.Titles.Password} :
          </label>
          <TextInput
            name="Password"
            id="password"
            value={loginModel.Password}
            onChange={handleChange}
          ></TextInput>
        </div>
        <div className="flex justify-end">
          <Button type="submit">{resources?.Titles.Submit}</Button>
        </div>
      </form>
    </div>
  );
};

export default UpdateLogin;
