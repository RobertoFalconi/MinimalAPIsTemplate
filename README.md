# MinimalAPIsTemplate
A comprehensive and fully functional ASP.NET Core Minimal APIs template, with lots of ready to go examples, including lots of middlewares, the most famous third-parties packages and OAuth 2.0 Authentication using JSON Web Algorithms and Tokens (JWA, JWT, JWS, JWE) as Bearer.  

## Examples of services and middlewares in this template:  
- Architecture based on Minimal APIs pattern with endpoints and route builders  
- Asynchronous Service calls with Dependency Injection (DI) for Singleton, Transient and Scoped Services  
- Authentication with JWT, JWS, JWE and JOSE  
- Authorization with additional custom policies using Authorization Handler  
- Dapper ORM for mapping from SQL to objects and viceversa  
- Docker container deployment  
- Entity Framework Core for mapping SQL to objects and viceversa, with LINQ and DbContextFactory  
- Exception Handler and error handling  
- GZipStream compression and decompression  
- Hangfire batch and job automations  
- HealthChecks health monitoring  
- HSTS and HTTPS redirection  
- HttpClientFactory with HTTP connection pooling  
- MediatR for decoupling, reduce boilerplate code, and easy implement CQRS and/or Event-Sourcing  
- Multiple environments usage (Development, Staging, Production and custom)  
- NLog and JSON console and DB logs  
- OpenAPI Swagger with Swashbuckle, Schemas, API definitions, authentication button etc.  
- StopWatch for benchmark and timing running methods  
- Validation of models and JSON body input with FluentValidation  

## AuthN and AuthZ examples ready-to-go:  
- JWT unsigned and not encrypted for basic knowledge  
- JWS ("alg": "HS512") signed with HMAC SHA-512 using a symmetric key  
- JWS ("alg": "RS512") signed with RSA SHA-512 using a X509 Certificate asymmetric key  
- JWE ("enc": "A256CBC-HS512", "alg": "dir") encrypted with AES256 using a symmetric key and signed with HMAC SHA-512 using a symmetric key  
- JWE ("enc": "A256CBC-HS512", "alg": "dir") encrypted with AES256 using a symmetric key and signed with RSA SHA-512 using a X509 Certificate asymmetric key  
- JOSE ("enc": "A256CBC-HS512", "alg": "RSA-OAEP") encrypted with RSA-OAEP using a X509 Certificate asymmetric key and signed with RSA-SSA-PSS-SHA512 using another X509 Certificate asymmetric key  
- Test API for Login, Authentication and Authorization with one of the generated Bearer token  
  
# How to use token samples  
1. Call a generate token method and copy the returned Bearer token *token*  
2. Log in clicking the Authorize green button in the Swagger UI and enter the value: "Bearer *token*" (without double quotes, replace *token* with its value)  
3. Call tryToken method. You will get 200 if authenticated, 401 otherwise  

## How to generate a PFX certificate
If you want to manually generate a .pfx file, you can use the following OpenSSL commands, then copy it to the Certificates folder of the project:  

```
openssl genrsa -aes256 -out sign-key.pem 2048  
openssl req -new -x509 -sha256 -outform pem -key sign-key.pem -days 365 -out sign-cert.pem  
openssl pkcs12 -export -out certificate.pfx -inkey privateKey.key -in certificate.pem  
```

## How to generate NLog table  
You can use the following T-SQL command to generate a table working with this NLog sample:  

```
CREATE TABLE [dbo].[NLog] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Logged]    DATETIME       NULL,
    [Level]     NVARCHAR (MAX) NULL,
    [Message]   NVARCHAR (MAX) NULL,
    [Logger]    NVARCHAR (MAX) NULL,
    [Callsite]  NVARCHAR (MAX) NULL,
    [Exception] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
```

# Sources and useful links
1. RFC 8259: The JavaScript Object Notation (JSON) Data Interchange Format - https://www.rfc-editor.org/rfc/rfc8259
2. RFC 7518: JSON Web Algorithms (JWA) - https://www.rfc-editor.org/rfc/rfc7518  
3. RFC 7520: Examples of Protecting Content Using JSON Object Signing and Encryption (JOSE) - https://www.rfc-editor.org/rfc/rfc7520.html
4. RFC 8017: PKCS #1: RSA Cryptography Specifications Version 2.2 - https://www.rfc-editor.org/rfc/rfc8017

# FAQ
### When sending a JWT (more properly a JWS) from microservice A to microservice B, should microservice A sign the token using: its own public key, or its own private key, or microservice B's public key, or microservice B's private key?

Microservice A should sign the token using its own private key. The digital signature is used to verify the integrity of the token and to prove that it was sent from a trustworthy source. When microservice A sends the JWT to B, it signs the token with its own private key to prove that it was sent by A and that the contents of the token have not been altered during transmission. Microservice B can then verify the signature using A's public key. This way, B can be sure that the token was sent by A and that the contents of the token have not been altered during transmission.

### When sending a JWT (more properly a JWE) from microservice A to microservice B, should microservice A encrypt the token using: its own public key, or its own private key, or microservice B's public key, or microservice B's private key?

Microservice A should encrypt the token using microservice B's public key. Encryption is used to protect sensitive data inside the token during transmission from A to B. In an asymmetric encryption system, the public key is used to encrypt data and verify the signature, while the private key is used to decrypt data and sign the token. When microservice A sends the JWT to B, it should encrypt the token using B's public key so that only B, possessing the corresponding private key, can decrypt the sensitive data.

In summary, microservice A should sign the JWT using its own private key, and microservice B should verify the signature using A's public key. Microservice A should encrypt the JWT using B's public key and only B, possessing the corresponding private key, will be able to decrypt the sensitive data.
