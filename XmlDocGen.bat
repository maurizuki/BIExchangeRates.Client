dotnet build src\XmlDocGen\XmlDocGen.csproj -c Release -o bin
bin\XmlDocGen.exe bin\BIExchangeRates.Client.dll docs --clean
