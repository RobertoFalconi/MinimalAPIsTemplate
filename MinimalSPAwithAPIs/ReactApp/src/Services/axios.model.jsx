import axios from "axios";
import { AuthService } from "../Services/auth.service.jsx";

const authService = AuthService.getInstance();

function buildCompleteUrl(url, data) {

    if (data) {
        url += "?";
        Object.keys(data).forEach((key) => {
            if (data[key]) url += `${key}=${data[key]}&`;
        })
        url = url.slice(0, -1);
        console.log("URL: " + url);
    }

    let host = `${authService.config.api.host}:${authService.config.api.port}`;

    if (authService.config.api.port) {
        url = `/${url.split("/").splice(1).join("/")}`;
    }

    if (url.includes("localhost"))
        host = "";

    return host + url
}


export function Axios_Get(url, data) {
    let promise = new Promise((resolve, reject) => {
        axios
            .get(url, data)
            .then(function (response) {
                if (!response.data.errorMessage) {
                    resolve(response);
                } else {
                    throw (response.data && response.data.errorMessage);
                }
            })
            .catch(function (error) {
                console.log(`Axios_Get: ${url} => ${error.message}`);
                reject(error);
            });
    });
    return promise;
}

export function Axios_Post(url, formData) {
    let promise = new Promise((resolve, reject) => {
        axios
            .post(url, formData, { headers: { "content-type": "application/json" } })
            .then(function (response) {
                if (!response.data.errorMessage) {
                    resolve(response);
                } else {
                    throw (response.data && response.data.errorMessage);
                }
            })
            .catch(function (error) {
                reject(error);
            });
    });
    return promise;
}

export function Axios_Put(url, formData) {
/*     console.log(`*** ROB:Axios_Put: url=${url} => ${buildCompleteUrl(url)}`);
 */    let promise = new Promise((resolve, reject) => {
        axios
            .put(url, formData)
            .then(function (response) {
                if (!response.data.errorMessage) {
                    resolve(response);
                } else {
                    throw (response.data && response.data.errorMessage);
                }
            })
            .catch(function (error) {
                reject(error);
            });
    });
    return promise;
}

export function Axios_Delete(url) {
    /* console.log(`*** ROB:Axios_Delete: url=${url} => ${buildCompleteUrl(url)}`); */
    let promise = new Promise((resolve, reject) => {
        axios
            .delete(url)
            .then(function (response) {
                resolve(response);
            })
            .catch(function (error) {
                reject(error);
            });
    });
    return promise;
}