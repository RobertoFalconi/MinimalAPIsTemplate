import moment from'moment';


export const getMomentDate = (date) => {
    return date ? moment(date) : ""
}
 
export const getTodaysDate = () => {
    return moment().format("yyyy-MM-DD");
}