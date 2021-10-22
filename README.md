# TaxRates
Very simple web application to count gross and net VAT tax based on Polish VAT system.

Migration files was left for purpose, just fire command from powershell "dotnet ef database update" in solution folder. 
Sample xlxs file with around 30 different VAT tax categories both with VAT stacks, based on real data. Feel free to expand. 

To Import them form xlxs just fire at **[GET] URL: <Host>/api/Seed/Import**

# Qucik Description
Raw data are stored in xlxs file, which later is loaded to database tables created with code-first approach. 
When possible some methods from controllers are moved to separate extension classes. 

Calculation itself is moved to separate project and offers gross and net VAT tax calculation type. Calculator offers both sync and async methods which are ( at least partially )
covered with UT.

Main point of interest:
**[POST] URL: <Host>/api/VatCalculation/Calculate**
Requires json as example below:

    {
        "amount": 100000,
        "calculatedVatType": "gross",
        "category": "Construction"
    }
 
* **amount** - decimal input value to compute
* **calculatedVatType** - "gross" (default value) or "net" 
* **category** -  if specific vatCategory is registered in database, application will search for this category and get VAT rate bounded to it. Otherwise, when empty or provided category
which do not exist in database, default VAT rate is used ( 0.23 ) and category for this calculation is set to empty string.

If everything went well, output should be sent as example below:

    {
        "valueType": "gross",
        "caclucaltedTaxValue": 108000.00,
        "vatCategory": "Construction",
        "errorMessage": null
    }
    
* **valueType** - calculated valueType: "gross" or "net"
* **caclucaltedTaxValue** - computed result
* **errorMessage** - If error is present errorMessage should be filled with... an error message rest of the values should be defaults ( "" or 0 ) 
 ##
- **[GET] /api/VatCalculation/Categories** - Retrives list of avalible categories with VAT tax rates.
- **[GET] api/VatCalculation/Help**  - might be helpful to get to know with application. 
