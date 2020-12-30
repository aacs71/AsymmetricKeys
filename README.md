# AsymmetricKeys
Convert between PEM and RSA XML Params

# Pre requisites

.NET 5

Install from https://dotnet.microsoft.com/download (either Linux or Windows)

```
dotnet build
```

```
❯ .\AsymmetricKeys.exe

  ----------------------------------------------------------------------------------------------------------------------
    Available commands:
  ----------------------------------------------------------------------------------------------------------------------
    frombase64 -> Convert content file from base 64
     genrsakey -> Generate RSA Key in PEM format
    genrsakeys -> Generate RSA Keys pair (private and public) and save in PEM format
      tobase64 -> Convert content file to base 64
         topem -> Convert a private key file to the RSA XML equivalent
         toxml -> Convert a private key file to the RSA XML equivalent
  ----------------------------------------------------------------------------------------------------------------------
```
