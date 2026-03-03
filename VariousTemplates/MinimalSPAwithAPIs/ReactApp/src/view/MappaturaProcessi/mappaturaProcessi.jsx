import React, { useEffect, useState } from "react";
import { Button, Form, Pagination, Table, Spin, Select, Space } from "antd";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash, faPencil } from "@fortawesome/free-solid-svg-icons";
import BreadCrumb from "../../components/common/BreadCrumb";

function MappaturaProcessi() {

    const dataSource = [
        {
            key: '1',
            Name: 'John Brown',
            area_dirigenziale: 'Fiscal code',
            area_prodotto: 'New York No. 1 Lake Park',
            Process: 'Process 1',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '2',
            Name: 'John Brown',
            area_dirigenziale: 'Fiscal code',
            area_prodotto: 'New York No. 1 Lake Park',
            Process: 'Process 2',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '3',
            Name: 'John Brown',
            area_dirigenziale: 'Fiscal code',
            area_prodotto: 'New York No. 1 Lake Park',
            Process: 'Process 3',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '4',
            Name: 'John Brown',
            area_dirigenziale: 'Fiscal code',
            area_prodotto: 'New York No. 1 Lake Park',
            Process: 'Process 4',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '5',
            Name: 'John Brown',
            area_dirigenziale: 'Fiscal code',
            area_prodotto: 'New York No. 1 Lake Park',
            Process: 'Process 5',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },
        {
            key: '5',
            Name: 'John Brown',
            area_dirigenziale: 'Fiscal code',
            area_prodotto: 'New York No. 1 Lake Park',
            Process: 'Process 6',
            data_inizio: '2021-01-01',
            data_fine: '2021-01-31',
        },

    ]

    const columns = [
        {
            title: 'Name',
            dataIndex: 'Name',
            key: 'Name',
            sorter: (a, b) => a.Name.localeCompare(b.Name),
        },
        {
            title: 'Fiscal code',
            dataIndex: 'area_dirigenziale',
            key: 'area_dirigenziale',
            sorter: (a, b) => a.Name.localeCompare(b.Name),
        },
        {
            title: 'Place',
            dataIndex: 'area_prodotto',
            key: 'area_prodotto',
            sorter: (a, b) => a.Name.localeCompare(b.Name),
        },
        {
            title: 'Process',
            dataIndex: 'Process',
            key: 'Process',
            sorter: (a, b) => a.Name.localeCompare(b.Name),
        },
        {
            title: 'Data inizio',
            dataIndex: 'data_inizio',
            key: 'data_inizio',
            sorter: (a, b) => a.Name.localeCompare(b.Name),
        },
        {
            title: 'Data fine',
            dataIndex: 'data_fine',
            key: 'data_fine',
            sorter: (a, b) => a.Name.localeCompare(b.Name),
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
                <Form
                    name="ricerca"
                    layout="vertical"
                    initialValues={{ layout: 'vertical' }}
                    style={{ maxWidth: 600, marginTop: 20 }}
                >
                    <Form.Item
                        className="mb-4"
                        label="Name"
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
                        label="Fiscal code"
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
                        label="Place"
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
                        name="Process"
                        label="Process"
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