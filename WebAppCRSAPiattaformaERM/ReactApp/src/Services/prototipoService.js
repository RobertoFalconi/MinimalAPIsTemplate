import axios from "axios";

export const getTempisticaMappatura = (id) => {
  return axios
    .get(`http://localhost:5234/TempisticaMappatura/GetById?id=${id}`)
    .then((response) => {
      return response;
    })
    .catch((error) => console.log(error));
};
