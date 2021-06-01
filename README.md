# Scan QRCodes in Xamarin.Forms App
This is a Xamarin.Forms(Android and IOS) application that scans QRCodes and sends requests for unlocking some functionality to an azure function - API. 

This application uses Azure Directory B2C Authentication.

In Addition for the unlocking purpose in the API, it sends in the request a encrypted info about a custom attribute "ROLE" that comes from the user claims.
