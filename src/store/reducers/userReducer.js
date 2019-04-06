import { SET_USER } from '../actions/userActions';
import { isEmpty } from '../../utils/validate';

const initialState = {
    userAccount: {},
    isAuthenticated: false,
};

export default function (state = initialState, action) {
    switch (action.type) {
        case SET_USER: 
            return { ...state, userAccount: action.payload, isAuthenticated: !isEmpty(action.payload) }

        default:
            return state;
    }
}

