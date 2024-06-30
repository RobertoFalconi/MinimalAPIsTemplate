import React, { useEffect, useState } from "react";
import { Button, Form, Pagination, Table, Spin, Select, Space } from "antd";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash, faPencil } from "@fortawesome/free-solid-svg-icons";
import BreadCrumb from "../../components/common/BreadCrumb";

function MappaturaProcessi() {

    const dataSource = [
        {
            key: '1',
            direzione: 'John Brown',
            area_dirigenziale: 'area dirigenziale',
            area_prodotto: 'New York No. 1 Lake Park',
            processo: 'processo 1',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '2',
            direzione: 'John Brown',
            area_dirigenziale: 'area dirigenziale',
            area_prodotto: 'New York No. 1 Lake Park',
            processo: 'processo 2',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '3',
            direzione: 'John Brown',
            area_dirigenziale: 'area dirigenziale',
            area_prodotto: 'New York No. 1 Lake Park',
            processo: 'processo 3',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '4',
            direzione: 'John Brown',
            area_dirigenziale: 'area dirigenziale',
            area_prodotto: 'New York No. 1 Lake Park',
            processo: 'processo 4',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '5',
            direzione: 'John Brown',
            area_dirigenziale: 'area dirigenziale',
            area_prodotto: 'New York No. 1 Lake Park',
            processo: 'processo 5',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '5',
            direzione: 'John Brown',
            area_dirigenziale: 'area dirigenziale',
            area_prodotto: 'New York No. 1 Lake Park',
            processo: 'processo 6',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },

    ]

    const columns = [
        {
            title: 'Direzione',
            dataIndex: 'direzione',
            key: 'direzione',
            sorter: (a, b) => a.direzione.localeCompare(b.direzione),
        },
        {
            title: 'Area dirigenziale',
            dataIndex: 'area_dirigenziale',
            key: 'area_dirigenziale',
            sorter: (a, b) => a.direzione.localeCompare(b.direzione),
        },
        {
            title: 'Area di prodotto',
            dataIndex: 'area_prodotto',
            key: 'area_prodotto',
            sorter: (a, b) => a.direzione.localeCompare(b.direzione),
        },
        {
            title: 'Processo',
            dataIndex: 'processo',
            key: 'processo',
            sorter: (a, b) => a.direzione.localeCompare(b.direzione),
        },
        {
            title: 'Data inizio',
            dataIndex: 'data_inizio',
            key: 'data_inizio',
            sorter: (a, b) => a.direzione.localeCompare(b.direzione),
        },
        {
            title: 'Data fine',
            dataIndex: 'data_fine',
            key: 'data_fine',
            sorter: (a, b) => a.direzione.localeCompare(b.direzione),
        },
        {
            title: 'Azioni',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <Button type="text"><FontAwesomeIcon icon={faTrash} /></Button>
                    <Button type="text"><FontAwesomeIcon icon={faPencil} /></Button>
                </Space>
            ),
        },
    ];


    return (
        <div className="row">
            <div className="col-12">
                <BreadCrumb />
                <div className="d-flex justify-content-center mt-5">
                    <Button type="primary" className="mr-2">Aggiungi Mappatura</Button>
                </div>
                <Form
                    name="ricerca"
                    layout="vertical"
                    initialValues={{ layout: 'vertical' }}
                    style={{ maxWidth: 600, marginTop: 20 }}
                >
                    <Form.Item
                        className="mb-4"
                        label="Direzione di riferimento"
                        style={{
                            display: 'inline-block',
                            width: 'calc(50% - 12px)',
                            marginRight: '20px',
                        }}
                        name="descrizione"
                        rules={[
                            {
                                required: true,
                                message: 'Please select',
                            },
                        ]}
                    >
                        <Select>
                            <Select.Option value="demo">Seleziona</Select.Option>
                        </Select>
                    </Form.Item>
                    <Form.Item
                        name="area_dirigenziale"
                        label="Area dirigenziale"
                        style={{
                            display: 'inline-block',
                            width: 'calc(50% - 12px)',
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please select',
                            },
                        ]}
                    >
                        <Select>
                            <Select.Option value="demo">Seleziona</Select.Option>
                        </Select>
                    </Form.Item>
                    <Form.Item
                        name="area_prodotto"
                        label="Area di prodotto"
                        style={{
                            display: 'inline-block',
                            width: 'calc(50% - 12px)',
                            marginRight: '20px',
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please select',
                            },
                        ]}
                    >
                        <Select>
                            <Select.Option value="demo">Seleziona</Select.Option>
                        </Select>
                    </Form.Item>
                    <Form.Item
                        name="processo"
                        label="Processo"
                        style={{
                            display: 'inline-block',
                            width: 'calc(50% - 12px)',
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please select',
                            },
                        ]}
                    >
                        <Select>
                            <Select.Option value="demo">Seleziona</Select.Option>
                        </Select>
                    </Form.Item>
                    <Form.Item
                        style={{ display: 'flex', justifyContent: 'center' }}>
                        <Button type="primary" htmlType="submit">Cerca</Button>
                    </Form.Item>
                </Form>

                <Table dataSource={dataSource} columns={columns} pagination={{ pageSize: 5, position: ["bottomCenter"] }} />
            </div>
        </div>
    )

}

export default MappaturaProcessi