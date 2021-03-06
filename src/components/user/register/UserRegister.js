import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { endPointRoot } from '../../../App';
import axios from 'axios';

const Wrapper = styled.div`
    .card-register{
        color: #fff;
        background-color: rgb(73, 84, 80);
        margin-bottom: 50px;
    }

    .card-register .form-control:focus{
        box-shadow: none;
    }

    .card-register .logo{
        position: absolute;
        top: 0;
        left: 50%;
        transform: translate(-50%, -50%);
        width: 80px;
        height: 80px;
        text-align: center;
        line-height: 80px;
        font-size: 2.5em;
        border-radius: 50%;
        background-color: lightseagreen;
    }
`;

class UserRegister extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            user: { username: '', password: '', address: '', mail: '', phone: '' }
        };
    }

    componentDidMount = () => {
        const $ = window.$;
        $('body').addClass('bg-register-gradient-body');
    }

    componentWillUnmount = () => {
        const $ = window.$;
        $('body').removeClass('bg-register-gradient-body');
    }

    onChange = e => {
        const { name, value } = e.target;
        let user = Object.assign({}, this.state.user);
        user[name] = value;
        this.setState({ user });
    }

    register = e => {
        e.preventDefault();
        const { user } = this.state;
        const url = endPointRoot + '/api/account/register';
        console.log(user);
        axios.post(url, user)
        .then(res => {
            console.log(res);
            if(res.data.success){
                alert('Login successfully!');
                setTimeout(() => {
                    this.props.history.push('/user/login');
                }, 2000);
            }
        })
        .catch(err => {
            console.log(err);
        })
    }

    render() {
        const { username, password, fullname, mail, phone, address } = this.state.user;
        return (
            <Wrapper>
                <div className="container">
                    <h2 className="text-center font-italic my-5 text-white">Vui lòng đăng kí tài khoản của bạn</h2>
                    <div className="row">
                        <div className="col-md-8 mx-auto">
                            <div className="card card-body card-register" style={{ marginBottom: '50px' }}>
                                <div className="logo">
                                    <i className="fas fa-book"></i>
                                </div>
                                <form className="mt-4" onSubmit={this.register}>
                                    <div className="row">
                                        <div className="col-sm-6 pr-sm-2">
                                            <div className="form-group">
                                                <label htmlFor="txtUsername">Tên đăng nhập</label>
                                                <div className="input-group">
                                                    <div className="input-group-prepend">
                                                        <span className="input-group-text">
                                                            <i className="fas fa-user"></i>
                                                        </span>
                                                    </div>
                                                    <input 
                                                        type="text"
                                                        className="form-control" 
                                                        id="txtUsername" 
                                                        placeholder="Nhập tên đăng nhập..."
                                                        name="username"
                                                        onChange={this.onChange}
                                                        value={username}
                                                     />
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-sm-6 pl-sm-2">
                                            <div className="form-group">
                                                <label htmlFor="txtRePassword">Ho ten</label>
                                                <div className="input-group">
                                                    <div className="input-group-prepend">
                                                        <span className="input-group-text">
                                                            <i className="fas fa-user"></i>
                                                        </span>
                                                    </div>
                                                    <input 
                                                        type="text" 
                                                        className="form-control" 
                                                        id="txtRePassword" 
                                                        placeholder="Nhập ho ten..."
                                                        name="fullname"
                                                        onChange={this.onChange}
                                                        value={fullname}
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>
                                    <div className="row">
                                        <div className="col-sm-6 pr-sm-2">
                                            <div className="form-group">
                                                <label htmlFor="txtPhoneNum">Số điện thoại</label>
                                                <div className="input-group">
                                                    <div className="input-group-prepend">
                                                        <span className="input-group-text">
                                                            <i className="fas fa-phone"></i>
                                                        </span>
                                                    </div>
                                                    <input 
                                                        type="text" 
                                                        className="form-control" 
                                                        id="txtPhoneNum" 
                                                        placeholder="Nhập số điện thoại..."
                                                        name="phone"
                                                        onChange={this.onChange}
                                                        value={phone}
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-sm-6 pl-sm-2">
                                            <div className="form-group">
                                                <label htmlFor="txtAddress">Địa chỉ</label>
                                                <div className="input-group">
                                                    <div className="input-group-prepend">
                                                        <span className="input-group-text">
                                                            <i className="fas fa-map-marker-alt"></i>
                                                        </span>
                                                    </div>
                                                    <input 
                                                        type="text" 
                                                        className="form-control" 
                                                        id="txtAddress" 
                                                        placeholder="Nhập địa chỉ..."
                                                        name="address"
                                                        onChange={this.onChange}
                                                        value={address}
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="row">
                                    <div className="col-sm-6 pl-sm-2">
                                            <div className="form-group">
                                                <label>Email</label>
                                                <div className="input-group">
                                                    <div className="input-group-prepend">
                                                        <span className="input-group-text">
                                                            <i className="fas fa-at"></i>
                                                        </span>
                                                    </div>
                                                    <input 
                                                        type="email" 
                                                        className="form-control" 
                                                        id="exampleInputEmail1" 
                                                        placeholder="Email..."
                                                        name="mail"
                                                        onChange={this.onChange}
                                                        value={mail}
                                                     />
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-sm-6 pr-sm-2">
                                            <div className="form-group">
                                                <label htmlFor="txtPassword">Mật khẩu</label>
                                                <div className="input-group">
                                                    <div className="input-group-prepend">
                                                        <span className="input-group-text">
                                                            <i className="fas fa-lock"></i>
                                                        </span>
                                                    </div>
                                                    <input 
                                                        type="password" 
                                                        className="form-control" 
                                                        id="txtPassword" 
                                                        placeholder="Nhập mật khẩu..."
                                                        name="password"
                                                        onChange={this.onChange}
                                                        value={password}
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>
                                    
                                    <button type="submit" className="btn btn-outline-success float-right">Đăng kí</button>
                                    <button type="reset" className="btn btn-outline-secondary float-right mr-2">Xóa thông tin</button>
                                </form>
                            </div>
                        </div>
                    
                    </div>
                </div>
            </Wrapper>
        );
    }
}

UserRegister.propTypes = {};

export default UserRegister;
