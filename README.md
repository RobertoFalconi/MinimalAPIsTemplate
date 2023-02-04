# MinimalAPIsTemplate
Complete and working ASP.NET Core Minimal APIs template, with OAuth 2.0 Authentication using JSON Web Algorithms and Tokens (JWA, JWT, JWS, JWE) as Bearer.  

## Examples of services and middlewares in this template:  
- Authentication and Authorization for Minimal APIs architecture pattern with Endpoint Handlers  
- Asynchronous Services calls with Dependency Injection (DI) and Scoped Services  
- HSTS and HTTPS Redirection  
- Exception Handler 
- Health Checks  
- Docker  
- NLog and JSON Console  
- Hangfire  
- OpenAPI Swagger with Swashbuckle  
- Entity Framework Core and LINQ  

## AuthN and AuthZ examples ready to go:  
- JWT unsigned and not encrypted for basic knowledge  
- JWS ("alg": "HS512") signed with HMAC SHA-512 using a symmetric key  
- JWS ("alg": "RS512") signed with RSA SHA-512 using a X509 Certificate asymmetric key  
- JWE ("enc": "A256CBC-HS512", "alg": "dir") encrypted with AES256 using a symmetric key and signed with HMAC SHA-512 using a symmetric key  
- JWE ("enc": "A256CBC-HS512", "alg": "dir") encrypted with AES256 using a symmetric key and signed with RSA SHA-512 using a X509 Certificate asymmetric key  
- Test API for Login, Authentication and Authorization with one of the generated Bearer token  
  
# How to use
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
1. RFC 7518: JSON Web Algorithms (JWA) - https://www.rfc-editor.org/rfc/rfc7518  
