
import { Input, Menu } from 'antd';
import React, { useState } from 'react';
import myLogo from "../../Assets/img/logo-my.svg";



const items = [
    {
        label: "Definizione",
        key: 'tempistiche',
    },
    {
        label: "Mappatura",
        key: 'mapProcessi',
    },
    {
        label: "MyMinimalSPAwithAPIs",
        key: 'MyMinimalSPAwithAPIs',
    },
    {
        label: "Campagna",
        key: 'MyMinimalSPAwithAPIs',
    },
    {
        label: "Monitoraggio 1",
        key: 'monUtenti',
    },
    {
        label: "Monitoraggio 2",
        key: 'monCampagne',
    },
    {
        label: "Statistiche",
        key: 'statistiche',
    }
];

function BarraRicerca() {
    const [current, setCurrent] = useState('mail');

    const onClick = (e) => {
        console.log('click ', e);
        setCurrent(e.key);
    };

    return (
        <>
            <div className='d-flex mt-4 mx-4'>
                <img src={myLogo} alt="my logo" />
                <div>
                    <Input styles={{ width: "20px" }} />
                </div>
            </div>
            <Menu theme="light" style={{display:"flex", alignItems:"center", justifyContent:"center"}} onClick={onClick} selectedKeys={[current]} mode="horizontal" items={items} />
        </>
    );
};

export default BarraRicerca;