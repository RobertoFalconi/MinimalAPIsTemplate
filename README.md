# MinimalAPIsTemplate
Complete and working Minimal APIs template, with support to Docker, Entity Framework, OpenAPI Swagger, OAuth 2.0 Authentication and Bearer Token, JWT, JWS, JWE.  
  
Example of APIs:  
- JWT unsigned and not encrypted for basic knowledge  
- JWS ("alg": "HS512") signed with HMAC SHA-512 using a symmetric key  
- JWS ("alg": "RS512") signed with RSA SHA-512 using a X509 Certificate asymmetric key  
- JWE ("enc": "A256CBC-HS512") encrypted with AES256 using a symmetric key and signed with HMAC SHA-512 ("alg": "dir") using a symmetric key  
- JWE ("enc": "A256CBC-HS512") encrypted with AES256 using a symmetric key and signed with RSA SHA-512 ("alg": "dir") using a X509 Certificate asymmetric key  
- Login (authentication) test with one of the generated Bearer token  

Other examples in this template:  
- Minimal APIs pattern with handlers
- Examples of asynchronous service call with Dependency Injection (DI)  
- Examples of UseExceptionHandler Middleware usage  

# How to use
1. Call a generate token method and copy the returned Bearer token *token*  
2. Login clicking the Authorize green button and enter the value: "Bearer *token*" (without double quotes, replace *token* with its value)  
3. Call tryToken method. You will get 200 if authenticated, 401 otherwise  

# Sources
1. https://www.rfc-editor.org/rfc/rfc7518
