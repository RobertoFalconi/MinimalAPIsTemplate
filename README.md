# MinimalAPIsTemplate
Complete and working Minimal APIs template, with support to Docker, Entity Framework, OpenAPI Swagger, OAuth 2.0 Authentication and Bearer Token, JWT, JWS, JWE.  
  
Example of APIs:  
- JWT unsigned and not encrypted for basic knowledge  
- JWS signed with HMAC SHA-256 (HS256) and a symmetric key  
- JWS signed with RSA SHA-512 (RS512) and a X509 Certificate with an asymmetric key  
- JWE encrypted with AES256 with one of the just mentioned JWS nested in the JWE  
- Login (authentication) test with one of the generated Bearer token  

Other common examples with Minimal API pattern: 
- Example of asynchronous service call with Dependency Injection (DI)  
- Example of UseExceptionHandler Middleware usage  
