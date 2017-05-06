# QuoteCreator
Web API to create random quote Images in standard 1366x768 size

## Usage
1. Change Image output directory to desired directory under Strings.cs
2. Build & Publish API to local IIS or IIS Express

## Route
http://domain[:port]/api/CreateQuote
* Each call generates a random quote image in output directory using QuotesOnDesign API
