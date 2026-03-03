import { Button, DatePicker, Form, Input, Pagination, Space, Table, Spin } from "antd";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTrash, faPencil, faSearch } from '@fortawesome/free-solid-svg-icons'
import React, { useEffect, useState } from "react";
import { getTempisticaCampagna, postTempisticaCampagna, putTempisticaCampagna, deleteTempisticaCampagna } from "../Services/TempisticaCampagna/validitaCampagnaServices";
import BreadCrumb from "../components/common/BreadCrumb";
import moment from "moment/moment";
import { LoadingOutlined } from '@ant-design/icons';
import ModaleValiditaCampagna from "../components/modale/modaleValiditaCampagna";


function ValiditàCampagna() {

  const [listaTempisticaProcessi, setListaTempisticaProcessi] = useState([]);
  const [dataToUpdate, setDataToUpdate] = useState(null);
  const [loading, setLoading] = useState(false);
  const [isVisible, setIsVisible] = useState(false)
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalElements, setTotalElements] = useState(0);
  const [formRef] = Form.useForm();



  useEffect(() => {
    listaTabellaProcessi()
  }, []);


  /* const deleteTabellaTempisticaProcessi = async () => {
    const deleteResponse = await deleteTempisticaMappatura()
    console.log('riga eliminata con successo');
  } */

  const onFinish = (values) => {
    setLoading(true);
    listaTabellaProcessi(values.descrizione, (values.data_da).format('YYYY-MM-DD'), (values.data_a).format('YYYY-MM-DD'), null, null, null, 'grtcA_DENOM')
  };

  const listaTabellaProcessi = async (descrizione, dataInizio, dataFine, pageSize, pageNumber, sortOrder, sortField) => {
    try {
      setLoading(true);
      const response = await getTempisticaCampagna(descrizione || null, dataInizio || null, dataFine || null, pageSize, pageNumber, sortOrder || null, 'grtcA_DENOM')
      setListaTempisticaProcessi(response.data.results)
      setCurrentPage(pageNumber)
      setTotalElements(response.data.totalCount)
     } catch (error) {
    }
    finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    const filteredData = listaTempisticaProcessi.filter((item) => item.grtcA_SEQ_TEMPISTICA_PK !== id);
    try {
      setLoading(true);
      await deleteTempisticaCampagna(id);
      setListaTempisticaProcessi(filteredData);
    } catch (error) {
      setLoading(false);
      console.error('Errore durante l eliminazione:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleConferma = async (values) => {
    setIsVisible(true);
    try {
      setLoading(true);
      if (dataToUpdate) {
        await putTempisticaCampagna(values);
      } else {
        await postTempisticaCampagna(values);
      }
      listaTabellaProcessi()
    } catch (error) {
      setLoading(false);
      console.error('Errore durante l inserimento:', error);
    } finally {
      setLoading(false);
    }
  }

  const handlePageChange = (page) => {
    listaTabellaProcessi( formRef.getFieldValue("descrizione") || null,  formRef.getFieldValue("data_da") || null, formRef.getFieldValue("data_a") || null, pageSize, page, null, 'grtcA_DENOM')
    
  };

  const handleUpdate = (record) => {
    setIsVisible(true);
    setDataToUpdate(record);
  };

/*   const { Search } = Input;
 */  const columns = [
    {
      title: 'Descrizione',
      dataIndex: 'grtcA_DENOM',
      key: 'grtcA_DENOM',
      sorter: (a, b) => a.grtcA_DENOM.localeCompare(b.grtcA_DENOM),
    },
    {
      title: 'Data inizio',
      dataIndex: 'grtcA_DATA_INIZ',
      key: 'grtcA_DATA_INIZ',
      sorter: (a, b) => a.grtcA_DATA_INIZ.localeCompare(b.grtcA_DATA_INIZ),
    },
    {
      title: 'Data fine',
      dataIndex: 'grtcA_DATA_FINE',
      key: 'grtcA_DATA_FINE',
      sorter: (a, b) => a.grtcA_DATA_FINE.localeCompare(b.grtcA_DATA_FINE),
    },
    {
      title: 'Azioni',
      key: 'action',
      render: (_, record) => (
        <Space size="middle">
          <Button type="text" onClick={() => handleDelete(record.grtcA_SEQ_TEMPISTICA_PK)}><FontAwesomeIcon icon={faTrash} /></Button>
          <Button type="text" onClick={() => handleUpdate(record)}><FontAwesomeIcon icon={faPencil} /></Button>
          <FontAwesomeIcon icon={faSearch} />
        </Space>
      ),
    },
  ];




  const dataSource = listaTempisticaProcessi?.map((dato, index) => (
    {
      key: index,
      grtcA_SEQ_TEMPISTICA_PK: dato.grtcA_SEQ_TEMPISTICA_PK,
      grtcA_DENOM: dato.grtcA_DENOM,
      grtcA_DATA_INIZ: dato.grtcA_DATA_INIZ,
      grtcA_DATA_FINE: dato.grtcA_DATA_FINE,
      grtcA_NOTE: dato.grtcA_NOTE,
      grtcA_FLAG_STATO: dato.grtcA_FLAG_STATO,
      grtcA_COD_UTENTE: dato.grtcA_COD_UTENTE,
      grtcA_DATA_AGGIORN: dato.grtcA_DATA_AGGIORN,
      grtcA_COD_APPL: dato.grtcA_COD_APPL,
    }
  )
  );
  return (
    <>
      <BreadCrumb />
      <div className="d-flex justify-content-center mt-5">
        <ModaleValiditaCampagna isVisible={isVisible} dataToUpdate={dataToUpdate} handleConferma={handleConferma} />
      </div>


      <Form
      form={formRef}
        name="ricerca"
        layout="vertical"
        onFinish={onFinish}
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
          name="data_da"
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
          name="data_a"
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

        <Form.Item>
          <Button type="primary" htmlType="submit">Cerca</Button>
        </Form.Item>
      </Form>
      <div className="container-fluid mb-4">
        {/*   <Search placeholder="Risultati" style={{ width: 200 }} /> */}


        {loading ? <Spin indicator={<LoadingOutlined style={{ fontSize: 24, }} spin />} /> :
          <div>
            <Table columns={columns} dataSource={dataSource} pagination={false}  locale={{triggerDesc: 'Clicca per ordinare in discendenza', triggerAsc: 'Clicca per ordinare in ascendenza', cancelSort: 'Clicca per annullare l\'ordinamento'}} />
            <Pagination className="mt-5" current={currentPage} total={totalElements} onChange={(page, pageSize) => handlePageChange(page, pageSize)} />
          </div>}

      </div >
    </>
  );
  }
  
  export default ValiditàCampagna;
  