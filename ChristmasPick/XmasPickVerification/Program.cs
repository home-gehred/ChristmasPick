using System;
using System.Collections.Generic;
using Common;
using Common.ChristmasPickList;


namespace XmasPickVerification
{
    class Program
    {
        static void OutputDiagnosticInfo(IDictionary<Person,ExchangeCheckSum> checkList, string subjectList)
        {
            int total = 0;
            int success = 0;
            int failures = 0;
            Console.WriteLine("Verification information for {0}", subjectList);
            foreach (KeyValuePair<Person, ExchangeCheckSum> checkListItem in checkList)
            {
                total++;
                if (checkListItem.Value.isValid())
                {
                    success++;
                }
                else
                {
                    Console.WriteLine("{0} failed validation: {1}", checkListItem.Key, checkListItem.Value.DiagnosticMessage());
                    failures++;
                }
            }
            Console.WriteLine("Out of {0} there were {1} successes and {2} failures", total, success, failures);
        }
        static void Main(string[] args)
        {
            DateTime christmasThisYear = new DateTime(2014, 12, 25);
            string adultArchivePath = @"C:\Users\cgehrer\Documents\Visual Studio 2013\Projects\ChristmasPick\Archive\Adult\Archive.xml";
            IXMasArchivePersister adultPersister = new FileArchivePersister(adultArchivePath);
            string kidArchivePath = @"C:\Users\cgehrer\Documents\Visual Studio 2013\Projects\ChristmasPick\Archive\Kids\Archive.xml";
            IXMasArchivePersister kidPersister = new FileArchivePersister(kidArchivePath);
           
            IFamilyProvider familyProvider = new FileFamilyProvider(@"C:\Users\cgehrer\Documents\Visual Studio 2013\Projects\ChristmasPick\Archive\Gehred\GehredFamily.xml");
            FamilyTree gehredFamily = familyProvider.GetFamilies();

            XMasArchive kidArchive = kidPersister.LoadArchive();
            PersonCollection kids = gehredFamily.CreateChristmasKidList(christmasThisYear);
            XMasPickList kidPickList = kidArchive.GetPickListForYear(christmasThisYear);

            XMasArchive adultArchive = adultPersister.LoadArchive();
            PersonCollection adults = gehredFamily.CreateChristmasAdultList(christmasThisYear);
            XMasPickList adultPickList = adultArchive.GetPickListForYear(christmasThisYear);


            XMasPickListValidator validator = new XMasPickListValidator();

            var kidCheckList = validator.PickListToValidateWithPeopleList(kids, kidPickList);
            var adultCheckList = validator.PickListToValidateWithPeopleList(adults, adultPickList);

            OutputDiagnosticInfo(kidCheckList, "Kid Pick List");
            OutputDiagnosticInfo(adultCheckList, "Adult Pick List");

 /*           IDictionary<Person, ExchangeCheckSum> checkList = new Dictionary<Person, ExchangeCheckSum>();
            // For each adult create an entry, in that entry store two ints number of people buying a present for, and a number of presents being recieved.
            foreach (Person person in adults) {
                checkList.Add(person, new ExchangeCheckSum());
            }

            foreach (XMasPick pick in adultPickList)
            {
                if (checkList.ContainsKey(pick.Recipient))
                {
                    checkList[pick.Recipient].updatePresentsIn();
                }
                else
                {
                    throw new Exception(string.Format("The recipient {0} is not found in adult list", pick.Recipient));
                }

                if (checkList.ContainsKey(pick.Subject))
                {
                    checkList[pick.Subject].updatePresentsOut();
                }
                else
                {
                    throw new Exception(string.Format("The subject {0} is not found in adult list", pick.Subject));
                }
            }
*/
        }
    }
}
