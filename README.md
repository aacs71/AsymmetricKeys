# AsymmetricKeys
Convert between PEM and RSA XML Params

#Pre requisites

.NET 5
Install from https://dotnet.microsoft.com/download

```
dotnet build
```

```
❯ .\AsymmetricKeys.exe

  ----------------------------------------------------------------------------------------------------------------------
    Available commands:
  ----------------------------------------------------------------------------------------------------------------------
     genrsakey -> Generate RSA Key in PEM format
    genrsakeys -> Generate RSA Keys pair (private and public) and save in PEM format
         topem -> Convert a private key file to the RSA XML equivalent
         toxml -> Convert a private key file to the RSA XML equivalent
  ----------------------------------------------------------------------------------------------------------------------
```
