# MinimalAPIsTemplate
Complete and working ASP.NET Core Minimal APIs template, with OAuth 2.0 Authentication using JSON Web Algorithms and Tokens (JWA, JWT, JWS, JWE) as Bearer.  
  
Examples of implemented APIs ready to go:  
- JWT unsigned and not encrypted for basic knowledge  
- JWS ("alg": "HS512") signed with HMAC SHA-512 using a symmetric key  
- JWS ("alg": "RS512") signed with RSA SHA-512 using a X509 Certificate asymmetric key  
- JWE ("enc": "A256CBC-HS512", "alg": "dir") encrypted with AES256 using a symmetric key and signed with HMAC SHA-512 using a symmetric key  
- JWE ("enc": "A256CBC-HS512", "alg": "dir") encrypted with AES256 using a symmetric key and signed with RSA SHA-512 using a X509 Certificate asymmetric key  
- Login (authentication) test with one of the generated Bearer token  

Other examples in this template:  
- Minimal APIs pattern with Handlers, Endpoints, Authentication and Authorization  
- Examples of asynchronous service calls with Dependency Injection (DI) and scoped services  
- Examples of support to Docker, Entity Framework, OpenAPI, Swagger  
- Examples of Hsts, Exception Handler, Json Console, Health Checks, Hangfire, Swashbuckle and other services and middlewares  
  
# How to use
1. Call a generate token method and copy the returned Bearer token *token*  
2. Log in clicking the Authorize green button in the Swagger UI and enter the value: "Bearer *token*" (without double quotes, replace *token* with its value)  
3. Call tryToken method. You will get 200 if authenticated, 401 otherwise  

# Sources and useful links
1. RFC 7518: JSON Web Algorithms (JWA) - https://www.rfc-editor.org/rfc/rfc7518  
