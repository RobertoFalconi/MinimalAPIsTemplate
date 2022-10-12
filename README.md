# MinimalAPIsTemplate
Complete and working Minimal APIs template, with support to Docker, Entity Framework, OpenAPI Swagger, OAuth 2.0 Authentication and Bearer Token, JWT, JWS, JWE.  
  
Example of APIs:  
- JWT unsigned and not encrypted for basic knowledge  
- JWS signed with HMAC SHA-256 (HS256) and a symmetric key  
- JWS signed with RSA SHA-512 (RS512) and a X509 Certificate asymmetric key  
- JWE encrypted with AES256 and signed with one of the just mentioned signature methods  
- Login (authentication) test with one of the generated Bearer token  

Other examples in this template:  
- Minimal APIs pattern with handlers
- Examples of asynchronous service call with Dependency Injection (DI)  
- Examples of UseExceptionHandler Middleware usage  

# How to use
1. Call a generate token method and copy the returned Bearer token *token*  
2. Login with value: "Bearer *token*" (without double quotes, replace *token* with its value)  
3. Call tryToken method. You will get 200 if authenticated, 401 otherwise  
