import { useState } from "react";
import { useAppSelector } from "../hooks";
import type { LoginForUpdateDTO } from "../store/interfaces/Update/LoginForUpdateDTO";

const UpdateLogin = () => {
    const [loginModel, setLoginModel] = useState<LoginForUpdateDTO>({
        Username: "",
        Password: ""
    });
    const resources = useAppSelector(state => state.resources);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value } = e.target;

        console.log(e.target)
        console.log(name, value )

        setLoginModel(prev => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        

    }
    

    return (<div>
        <form onSubmit={handleSubmit}>
            <label className="font-bold mx-2" htmlFor="username">{resources?.Titles.Username} :</label>
            <input
                name="Username" 
                id="username" 
                className=" block border border-gray-300 rounded-md shadow-sm focus:border-blue-500 focus:outline-none m-2 px-4 py-2 w-100" 
                value={loginModel.Username}  
                onChange={handleChange}
                placeholder="Username"
            />
            <label className="font-bold mx-2" htmlFor="password">{resources?.Titles.Password} :</label>
            <input
                name="Password" 
                id="password" 
                className="block border border-gray-300 rounded-md shadow-sm focus:border-blue-500 focus:outline-none m-2 px-4 py-2 w-100" 
                value={loginModel.Password}
                onChange={handleChange}
                placeholder="Password"
            />
        </form>
    </div>);
}

export default UpdateLogin;