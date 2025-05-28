import { useAppSelector } from "../hooks";

const Login = () => {
    const { resources } = useAppSelector(state => state.resources);

    console.log(resources)

    return (<div>
        login
    </div>)
}

export default Login;