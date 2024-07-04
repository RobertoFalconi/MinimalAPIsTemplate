import { Breadcrumb } from 'antd';
import React from 'react';
import { Link } from 'react-router-dom';

function BreadCrumb() {
    return (
        <Breadcrumb
            items={[
                {
                    title: <Link to={'/'} >Home</Link>,
                },
                {
                    title: <Link to={'/definizioneTempistiche'}> Lorem</Link>,
                    path: '/definizioneTempistiche', 
                    children: [
                        {
                            title: <Link to={'/validitaCampagna'}> Ipsum</Link>,
                            path: '/validitaCampagna',
                        },
                        {
                            title: <Link to={'/tempisticaProcessi'}> Dolor</Link>,
                            path: '/tempisticaProcessi',
                        },
                    ]               
                },
            ]}
        />
    );
};
export default BreadCrumb;