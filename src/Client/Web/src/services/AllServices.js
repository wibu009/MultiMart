import axios from './customize-axios';


const AllUser = () => {
    return axios.get('/api/users?page=1');
}

const LoginApi = (email, password) => {
    return axios.post("/api/login", {email, password})
}

export {AllUser, LoginApi};