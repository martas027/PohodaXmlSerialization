using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PohodaXmlSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            //var taxDocuments = db.TaxDocuments.Where(x => x.Typ == TaxDocument.Enum_TaxDocumentType.Invoice && x.Zruseno == false && x.CreatedUtc > new DateTime(2018, 12, 05)).ToList();
            var dataPack = new dataPackType();
            var dataPackItems = new List<dataPackItemType>();

            var staList = new List<shipToAddressType>();
            staList.Add(new shipToAddressType
            {
                name = "Jmeno",
                city = "Obec",
                street = "Ulice 12/13",
                zip = "12345"
            });

            var account = new accountType();
            account.accountNo = "123456789";
            account.bankCode = "0800";

            //--- Invoice Items
            var invoiceItemList = new List<invoiceItemType>();
            invoiceItemList.Add(new invoiceItemType
            {
                text = "Dodávka",
                quantity = 1,
                rateVAT = vatRateType.high,
                homeCurrency = new typeCurrencyHomeItem
                {
                    unitPrice = "500"
                }
            });

            //--- DataPackItem
            var dataPackItem = new dataPackItemType
            {
                id = "AD002",
                version = dataPackItemVersionType.Item20,
                Item = new invoiceType
                {
                    invoiceHeader = new invoiceHeaderType
                    {
                        invoiceType = invoiceTypeType.issuedInvoice,
                        symVar = "2793", //TODO elektrina
                        number = new numberType
                        {
                            numberRequested = new numberTypeNumberRequested { Value = "1900010" }
                        },
                        date = DateTime.Now,
                        dateTax = DateTime.Now,
                        dateAccounting = DateTime.Now,
                        classificationVAT = new classificationVATType { classificationVATType1 = classificationVATTypeClassificationVATType.inland },
                        text = "Fakturace dodávky plynu",  //TODO elektrina
                        partnerIdentity = new address
                        {
                            address1 = new addressType
                            {
                                name = "Jmeno",
                                city = "Obec",
                                street = "Ulice",
                                zip = "12345"
                            }, //TODO elektrina
                            shipToAddress = staList.ToArray(), //TODO elektrina
                        },
                        account = account != null ? account : null,
                        note = "Načteno z XML",
                    },
                    invoiceDetail = new invoiceDetailType
                    {
                        Items = invoiceItemList.ToArray()
                    }
                },
                ItemElementName = ItemChoiceType6.invoice
            };

            dataPackItems.Add(dataPackItem);

            dataPack.dataPackItem = dataPackItems.ToArray();

            var serializer = new XmlSerializer(typeof(dataPackType));
            using (var stream = new StreamWriter("C:\\user\\test.xml"))
                serializer.Serialize(stream, dataPack);
        }
    }
}
