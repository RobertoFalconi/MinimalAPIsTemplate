import { Axios_Get, Axios_Post, Axios_Put, Axios_Delete } from "../axios.model.jsx";

const baseUrl = "http://localhost:5234/TempisticaCampagna";

export function getTempisticaCampagna(descrizione, startDate, endDate, pageSize, pageNumber, sortOrder, sortField) {

        const description = descrizione? `GRTCA_DENOM=${descrizione}` : '';
        const dataIniz = startDate? `&GRTCA_DATA_INIZ=${startDate}` : '';
        const dataFine = endDate? `&GRTCA_DATA_FINE=${endDate}` : '';
        const numPagine = pageNumber? `&PageNumber=${pageNumber}` : '';
        const dimPage = pageSize? `&PageSize=${pageSize}` : '';
        const ordineAscDesc = sortOrder? `&OrderAscDesc=${sortOrder}` : '';

        return Axios_Get(`${baseUrl}?${description}${dataIniz}${dataFine}${numPagine}${dimPage}${ordineAscDesc}&OrderColumnName=${sortField}`);


};

export function deleteTempisticaCampagna(id) {
     return Axios_Delete(`${baseUrl}?id=${id}`);
};

export function postTempisticaCampagna(formData) {

    return Axios_Post(`${baseUrl}`, formData);

}

export function putTempisticaCampagna(formData) {  
    return Axios_Put(`${baseUrl}`, formData);
}