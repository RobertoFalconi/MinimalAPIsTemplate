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
                    title: <Link to={'/definizioneTempistiche'}> Definizione tempistiche</Link>,
                    path: '/definizioneTempistiche', 
                    children: [
                        {
                            title: <Link to={'/validitaCampagna'}> Validità Campagna</Link>,
                            path: '/validitaCampagna',
                        },
                        {
                            title: <Link to={'/tempisticaProcessi'}> Tempistica Processi</Link>,
                            path: '/tempisticaProcessi',
                        },
                    ]               
                },
            ]}
        />
    );
};
export default BreadCrumb;