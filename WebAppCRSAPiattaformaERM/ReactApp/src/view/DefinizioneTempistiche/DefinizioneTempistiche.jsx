import React from 'react';
import { Card } from 'antd';
import myLogo from '../../Assets/img/logo-my-mobile.svg';
import BreadCrumb from '../../components/common/BreadCrumb';
import { Link } from 'react-router-dom';


function DefinizioneTempistiche() {

    return (
        <>
            <BreadCrumb />
            <div className='container'>
                <div className='row'>
                    <div className='col-12'>
                        <div className='d-flex justify-content-center mt-5'>
                            <Link to="/definizioneTempistiche/validitaCampagna">
                                <Card
                                    hoverable
                                    style={{
                                        width: 240,
                                        display: 'inline-block',
                                        marginRight: 50,
                                    }}
                                    cover={<img alt="example" src={myLogo} />}
                                >
                                    <Card.Meta title="Validità Campagna" />
                                </Card>
                            </Link>
                            <Link to="/definizioneTempistiche/tempisticaProcessi">
                                <Card
                                    hoverable
                                    style={{
                                        display: 'inline-block',
                                        width: 240,
                                    }}
                                    cover={<img alt="example" src={myLogo} />}
                                >
                                    <Card.Meta title="Tempistica Processi" />
                                </Card>
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </>

    )
}
export default DefinizioneTempistiche;