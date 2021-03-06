import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { Link, withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { loginUser } from '../../../store/actions/userActions';

const Wrapper = styled.div`
    .card-login{
        color: #fff;
        background-color: rgb(73, 84, 80);
        position: relative;
    }
    .card-login .form-control:focus{
        box-shadow: none;
    }
    .card-login .logo{
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

class LoginUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            user: { username: '', password: '' }
        };
    }

    componentDidMount = () => {
        const $ = window.$;
        $('body').addClass('bg-login-gradient-body');
    }

    componentWillReceiveProps = (nextProps) => {
        if(nextProps.user.isAuthenticated){
            this.props.history.push('/user');
        }
    }

    componentWillUnmount = () => {
        const $ = window.$;
        $('body').removeClass('bg-login-gradient-body');
    }

    onChange = e => {
        const { name, value } = e.target;
        let user = Object.assign({}, this.state.user);
        user[name] = value;
        this.setState({ user });
    }

    login = (e) => {
        e.preventDefault();
        let { user } = this.state;
        this.props.loginUser(user);
    }

    render() {
        return (
            <Wrapper>
                <div className="container">
                    <h2 className="text-center font-italic my-5 text-white">Đăng nhập vào tài khoan của bạn</h2>
                    <div className="row">
                        <div className="col-md-6 mx-auto">
                            <div className="card card-body card-login" style={{ marginTop: '50px'}}>
                                <div className="logo">
                                    <i className="fas fa-book"></i>
                                </div>
                                <form className="mt-3" autoComplete="off" onSubmit={this.login}>
                                    <div className="form-group">
                                        <label htmlFor="">Tên người dùng</label>
                                        <div className="input-group">
                                            <div className="input-group-prepend">
                                                <span className="input-group-text">
                                                    <i className="fas fa-at"></i>
                                                </span>
                                            </div>
                                            <input type="text" 
                                            className="form-control" 
                                            id="exampleInputEmail1" 
                                            placeholder="Nhập tên người dùng..."
                                            name="username"
                                            onChange={this.onChange}
                                            value={this.state.user.username}
                                            />
                                        </div>
                                    </div>
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
                                                value={this.state.user.password}
                                            />
                                        </div>
                                    </div>
                                    <div style={{ fontSize: '.8em',  marginBottom: '20px' }}>
                                        Nếu chưa có tài khoản, vui lòng <Link to="/user/register">đăng kí</Link> tài khoản mới
                                    </div>
                                    <button type="submit" className="btn btn-outline-success float-right">Đăng nhập</button>
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

LoginUser.propTypes = {
    loginUser: PropTypes.func.isRequired,
    user: PropTypes.object.isRequired
};

const mapStateToProps = state => ({
    user: state.user
})

export default connect(mapStateToProps, { loginUser })( withRouter(LoginUser) );
