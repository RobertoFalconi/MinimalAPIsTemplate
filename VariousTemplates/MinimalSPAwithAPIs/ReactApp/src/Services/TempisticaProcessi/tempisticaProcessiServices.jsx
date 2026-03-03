import { Axios_Get, Axios_Post, Axios_Put, Axios_Delete } from "../axios.model.jsx";

const baseUrl = "http://localhost:5234/TempisticaProcessi";

export function getTempisticaProcessi(descrizione, startDate, endDate, pageSize, pageNumber, sortOrder, sortField) {

    /* if (descrizione && startDate && endDate && pageSize && pageNumber && sortOrder && sortField) { */

        const description = descrizione? `GRTPR_DENOM=${descrizione}` : '';
        const dataIniz = startDate? `&GRTPR_DATA_INIZ=${startDate}` : '';
        const dataFine = endDate? `&GRTPR_DATA_FINE=${endDate}` : '';
        const numPagine = pageNumber? `&PageNumber=${pageNumber}` : '';
        const dimPage = pageSize? `&PageSize=${pageSize}` : '';
        const ordineAscDesc = sortOrder? `&OrderAscDesc=${sortOrder}` : '';

        return Axios_Get(`${baseUrl}?${description}${dataIniz}${dataFine}${numPagine}${dimPage}${ordineAscDesc}&OrderColumnName=${sortField}`);

    /* } */

};

export function deleteTempisticaProcessi(id) {
     return Axios_Delete(`${baseUrl}?id=${id}`);
};

export function postTempisticaProcessi(formData) {

    return Axios_Post(`${baseUrl}`, formData);

}

export function putTempisticaProcessi(formData) {  
    return Axios_Put(`${baseUrl}`, formData);
}