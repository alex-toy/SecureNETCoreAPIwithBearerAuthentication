# SecureNETCoreAPIwithBearerAuthentication

In this project, we will see how to secure a .NET Core API with JWT Bearer Authentication, using Azure Active Directory as the Identity and Access Management Layer,  how to write a “secure” API client to call and authenticate to the secured API endpoint. We will use the *Microsoft.Identity.Client* and *Microsoft.AspNetCore.Authentication.JwtBearer* packages amongst others.


## API side

- Install packages
```
Microsoft.AspNetCore.Authentication.JwtBearer
```

- create an *App Registration* for the API
<img src="/pictures/app_registration1.png" title="app registration"  width="500">

- fill in the AAD section in *appsettings.json*, using the overview section
<img src="/pictures/app_registration.png" title="app registration"  width="900">

- at that moment, you will receive a 401 Unauthorizedresponse
<img src="/pictures/app_registration2.png" title="app registration"  width="900">


## Client side

- create an *App Registration* for the client
<img src="/pictures/client_app_reg.png" title="client app registration"  width="900">

- in the *Certificates & Secrets* section, create a secret for the client
<img src="/pictures/client_app_reg2.png" title="client app registration"  width="900">

- in the *API Permissions* section, add a permission for our secure API
<img src="/pictures/client_app_reg3.png" title="client app registration"  width="900">
<img src="/pictures/client_app_reg4.png" title="client app registration"  width="900">