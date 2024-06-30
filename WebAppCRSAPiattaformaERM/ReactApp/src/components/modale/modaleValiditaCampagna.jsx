import React, { useState, useRef, useEffect } from 'react';
import { Button, Modal, DatePicker, Form, Input } from 'antd';
import { getMomentDate } from '../../utils/dateUtils';

const ModaleValiditaCampagna = ({ dataToUpdate, handleConferma, isVisible }) => {



    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isUpdate, setIsUpdate] = useState(false);
    const [formRef] = Form.useForm();


    useEffect(() => {
        isVisible && showModal()
        !dataToUpdate ? (function () { setIsUpdate(false); formRef.resetFields() })() : (function () {
            setIsUpdate(true); formRef.setFieldsValue({
                descrizione: dataToUpdate.grtcA_DENOM,
                data_Inizio: getMomentDate(dataToUpdate.grtcA_DATA_INIZ || null),
                data_Fine: getMomentDate(dataToUpdate.grtcA_DATA_FINE || null),
                note: dataToUpdate.grtcA_NOTE,
                cod_Utente: dataToUpdate.grtcA_COD_UTENTE,
                flag_Stato: dataToUpdate.grtcA_FLAG_STATO,
                data_Agg: getMomentDate(dataToUpdate.grtcA_DATA_AGGIORN || null),
                cod_Appl: dataToUpdate.grtcA_COD_APPL
            })
        })()
    }, [dataToUpdate, formRef]);

    const showModal = () => {
        setIsModalOpen(true);
    };


    const handleConfirm = async () => {
        const formValues = formRef.getFieldsValue();
        const formattedData = {
            grtcA_DENOM: formValues.descrizione,
            grtcA_DATA_INIZ: (formValues.data_Inizio).format('YYYY-MM-DD'),
            grtcA_DATA_FINE: (formValues.data_Fine).format('YYYY-MM-DD'),
            grtcA_NOTE: formValues.note,
            grtcA_FLAG_STATO: formValues.flag_Stato,
            grtcA_COD_UTENTE: formValues.cod_Utente,
            grtcA_DATA_AGGIORN: (formValues.data_Agg).format('YYYY-MM-DD'),
            grtcA_COD_APPL: formValues.cod_Appl,
        };
        if (isUpdate) {
            const submitObject = {
                ...formattedData,
                grtcA_SEQ_TEMPISTICA_PK: dataToUpdate.grtcA_SEQ_TEMPISTICA_PK,
            };
            handleConferma(submitObject);
            setIsModalOpen(false);
        } else {
            handleConferma(formattedData);
            setIsModalOpen(false);
        }
    };

    const handleOk = () => {
        formRef.submit();
    }

    const handleCancel = () => {
        setIsModalOpen(false);
        setIsUpdate(false);
        formRef.resetFields();
    };

    return (
        <>
            <Button type="primary" onClick={showModal}>
                Inserisci Validità
            </Button>
            <Modal okText={isUpdate ? 'Aggiorna' : 'Inserisci'} title={isUpdate ? 'Aggiorna validità' : 'Inserisci validità'} open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
                <Form
                    form={formRef}
                    onFinish={handleConfirm}
                    name="inserisci"
                    layout="vertical"
                    initialValues={{ layout: 'vertical' }}
                    style={{ maxWidth: 600, marginTop: 20 }}
                >
                    <Form.Item
                        className="mb-4"
                        label="Descrizione"
                        name="descrizione"
                        rules={[
                            {
                                required: true,
                                message: 'Please input your descrizione!',
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        name="data_Inizio"
                        label="Data da"
                        style={{
                            display: 'inline-block',
                            width: 'calc(50% - 12px)',
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Data da!',
                            },
                        ]}
                    >
                        <DatePicker />
                    </Form.Item>
                    <Form.Item
                        name="data_Fine"
                        label="Data a"
                        style={{
                            display: 'inline-block',
                            width: 'calc(50% - 12px)',
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Data a!',
                            },
                        ]}
                    >
                        <DatePicker />
                    </Form.Item>
                    <Form.Item
                        name="note"
                        label="Note"
                        style={{ maxWidth: 200, marginTop: 5, display: 'inline-block', width: 'calc(50% - 12px)' }}
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Data a!',
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        name="flag_Stato"
                        label="Flag Stato"
                        style={{ maxWidth: 100, marginTop: 5, display: 'inline-block', width: 'calc(50% - 12px)', marginLeft: 50 }}
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Data a!',
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        name="cod_Utente"
                        label="Codice Utente"
                        style={{
                            maxWidth: 200,
                            marginTop: 5,
                            display: 'inline-block',
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Data a!',
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        name="data_Agg"
                        label="Data Aggiornamento"
                        style={{
                            maxWidth: 200,
                            marginTop: 5,
                            display: 'inline-block',
                            marginLeft: 50
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Data a!',
                            },
                        ]}
                    >
                        <DatePicker />
                    </Form.Item>
                    <Form.Item
                        name="cod_Appl"
                        label="Codice Applicazione"
                        style={{
                            maxWidth: 200,
                            marginTop: 5,
                        }}
                        rules={[
                            {
                                required: true,
                                message: 'Please input your Data a!',
                            },
                        ]}
                    >
                        <Input />
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
};
export default ModaleValiditaCampagna;