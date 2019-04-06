import axios from 'axios';

export const SET_USER = 'SET_USER';

const endPoint = 'https://192.168.0.110:45456';

export function loginUser(user){
    const url = endPoint + '/api/account/login';
    console.log('login user')
    return function(dispatch){
        axios.post(url, user)
        .then(res => {
            console.log(res.data);
        })
        .catch(err => console.log(err.statusCode));
    }
}

function setUser(user){
    return { type: SET_USER, payload: user };
}