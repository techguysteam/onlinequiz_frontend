import React from 'react';
import styled from 'styled-components';

const Title = styled.h2`
    margin-top: 100px;
    display: inline-block;
    border: 5px solid var(--main-color);
    padding: 20px;
`;

const Header = ({ title }) => (
    <header id="home" className="">
        <div className="overlay"></div>
        <div className="content">
            <Title>{ title }</Title>
        </div>
    </header>
);

export default Header;
