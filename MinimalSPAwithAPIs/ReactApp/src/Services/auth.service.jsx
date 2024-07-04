import { UserManager, WebStorageStateStore, Log } from "oidc-client-ts";


export const AuthService = (() => {

    function AuthSrvcClass() {

        this.init = () => {
            return new Promise((resolve, reject) => {
                fetch("/config.json")
                    .then(res => res.json())
                    .then(data => {
                        this.config = data;
                        /* createUserManager(); */
                        resolve();
                    }).catch(err => {
                        console.log("ERROR - Config file not found.", err)
                        reject(err)
                    })
            })
        }
    }

    
    var instance;
    return {
        getInstance() {
            if (instance == null) {
                instance = new AuthSrvcClass();
                instance.constructor = null;
            }
            return instance;
        }
    }
})();