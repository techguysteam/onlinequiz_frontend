import axios from 'axios';
import { endPointRoot } from '../../App';

export const SET_USER = 'SET_USER';

export function loginUser(user){
    console.log('login user');
    return function(dispatch){
        const url = endPointRoot + '/api/account/login';
        axios.post(url, user, { withCredentials: false, headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
          } })
        .then(res => {
            console.log(res);
            dispatch(setUser(res.data));
        })
        .catch(err => {
            dispatch(setUser({}));
            console.log(err.message);
        });
    }
}

function setUser(user){
    return { type: SET_USER, payload: user };
}