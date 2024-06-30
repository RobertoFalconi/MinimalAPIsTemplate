
import { Input, Menu } from 'antd';
import React, { useState } from 'react';
import inpsLogo from "../../Assets/img/logo-inps.svg";



const items = [
    {
        label: "Definizione tempistiche",
        key: 'tempistiche',
    },
    {
        label: "Mappatura Processi",
        key: 'mapProcessi',
    },
    {
        label: "Gestione configurazioni campagna CRSA",
        key: 'compagnaCRSA',
    },
    {
        label: "Campagna di control Risk Self Assessment",
        key: 'CRSA',
    },
    {
        label: "Monitoraggio utenti",
        key: 'monUtenti',
    },
    {
        label: "Monitoraggio campagne",
        key: 'monCampagne',
    },
    {
        label: "Statistiche e reportistica",
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
                <img src={inpsLogo} alt="INPS logo" />
                <div>
                    <Input styles={{ width: "20px" }} />
                </div>
            </div>
            <Menu theme="light" style={{display:"flex", alignItems:"center", justifyContent:"center"}} onClick={onClick} selectedKeys={[current]} mode="horizontal" items={items} />
        </>
    );
};

export default BarraRicerca;