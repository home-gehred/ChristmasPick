using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.Email;
using Common;
using Common.ChristmasPickList;
using System;
using System.IO;
using SecurityUtil.SecretsProvider;

namespace XmasPickEmail
{
    class Program
    {
        static private PickAvailableMessage CreateEmailMessage(EmailAddress To, Person giftMaker, string pickMessage)
        {
            var plainTextMsg = $"Hello {giftMaker},\r\n\r\n I for one is looking forward to the end of the year 2020!\r\n Santa's Lil' Helper has been hard at work.\r\n IMPORTANT: I expect problems as this is the second time I have used this process.\r\nPlease, Please, Please reply to this email that you got this notification, AND send an email to bagehred@sbcglobal.net\r\nThe response is important for me to keep track of how this new notification process is working.\r\n";
            plainTextMsg += $"And as always, notify family members you got your name for Christmas. Relay that, if they do not have a name, to get in touch with Uncle Bob. (bagehred@sbcglobal.net)\r\n";
            plainTextMsg += "\r\n";
            plainTextMsg += $"This maybe a duplicate, if you have responded already I apologize and disregard, otherwise get to it!\r\n";
            plainTextMsg += "\r\n";
            plainTextMsg += $"Alright, on to the fun stuff,\r\n";
            plainTextMsg += "On the bright side, ...\r\n";
            plainTextMsg += "Important Dates:\r\n";
            plainTextMsg += "Super Saturday: Cancelled due to Covid-19";
            plainTextMsg += "\r\n";
            plainTextMsg += "Christmas Sing - A - Long: Cancelled due to Covid-19\r\n";
            plainTextMsg += "\r\n";
            plainTextMsg += "Gehred Nation Christmas: Cancelled due to Covid-19\r\n";
            plainTextMsg += "\r\n";
            plainTextMsg += $"{pickMessage}";
            plainTextMsg += "\r\n";
            var htmlMsg = plainTextMsg.Replace("\r\n", "</br>");
            var content = new PickAvailableMessage
            {
                HtmlBody = $"<!DOCTYPE html><html><body>{htmlMsg}</body></html> ",
                PlainTextBody = plainTextMsg,
                Name = "C",
                NotificationType = NotifyType.Email,
                Subject = "IMPORTANT: Gehred Nation Christmas Pick for 2020",
                ToAddress = To
            };
            return content;
        }

        static void Main(string[] args)
        {
            var secretsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "secrets.json");
            var secretsProvider = new JsonFileProvider(secretsPath);
            var sendGridApiKey = secretsProvider.GetSecret("sendgridApiKey");
            var emailer = new SendGridNotifyPickIsAvalable(sendGridApiKey);
            DateTime christmasThisYear = new DateTime(2020, 12, 25);
            string adultArchivePath = @"/Users/cgehrer/Code/ChristmasPick/Archive/Adult/Archive.xml";
            string kidArchivePath = @"/Users/cgehrer/Code/ChristmasPick/Archive/Kids/Archive.xml";
            IXMasArchivePersister adultPersister = new FileArchivePersister(adultArchivePath);
            IXMasArchivePersister kidPersister = new FileArchivePersister(kidArchivePath);
            IFamilyProvider familyProvider = new FileFamilyProvider(@"/Users/cgehrer/Code/ChristmasPick/Archive/Gehred/GehredFamily.xml");
            JsonFileEmailAddressProvider emailAddressProvider = new JsonFileEmailAddressProvider(@"/Users/cgehrer/Code/ChristmasPick/Archive/GehredFamily_Contacts.json");

            XMasArchive adultArchive = adultPersister.LoadArchive();
            XMasArchive kidArchive = kidPersister.LoadArchive();
            FamilyTree gehredFamily = familyProvider.GetFamilies();

            XMasPickList adultPickList = adultArchive.GetPickListForYear(christmasThisYear);
            XMasPickList kidPickList = kidArchive.GetPickListForYear(christmasThisYear);

            string pickMsg = "\tFor the Christmas of {0} {1} will buy a {2} gift for {3}";
            var emailCount = 0;
            foreach (Family family in gehredFamily)
            {
                Console.WriteLine("");
                Console.WriteLine(string.Format("Master List for {0}", family.Name));
                foreach (Person person in family)
                {
                    if (person.IsConsideredAChild(christmasThisYear))
                    {
                        decimal giftAmount = 20.00M;
                        Person recipient = kidPickList.GetRecipientFor(person);
                        var giftMessage = string.Format(pickMsg, christmasThisYear.Year, person.ToString(), giftAmount.ToString("c"), recipient.ToString());

                        if (emailAddressProvider.ShouldBeContacted(person))
                        {
                            // Using the Person object lookup email.
                            var emailAddresses = emailAddressProvider.GetEmailAddresses(person);

                            Console.WriteLine($"Attempting to email {person} using...");
                            foreach (var emailAddress in emailAddresses)
                            {
                                emailCount++;
                                var content = CreateEmailMessage(emailAddress, person, giftMessage);
                                var testEnvelope = new Envelope(content);
                                var emailSendStatus = emailer.Notify(testEnvelope).GetAwaiter().GetResult();
                                if (!emailSendStatus.IsSuccess())
                                {
                                    Console.WriteLine($"{emailAddress} Status: Error: {emailSendStatus.Message}");
                                }
                                else
                                {
                                    Console.WriteLine($"{emailAddress} Status: Ok");
                                }
                            }
                            Console.WriteLine(giftMessage);
                        }
                    }
                    else
                    {
                        decimal giftAmount = 5.00M;
                        Person recipient = adultPickList.GetRecipientFor(person);
                        var giftMessage = string.Format(pickMsg, christmasThisYear.Year, person.ToString(), giftAmount.ToString("c"), recipient.ToString());

                        if (emailAddressProvider.ShouldBeContacted(person))
                        {
                            // Using the Person object lookup email.
                            var emailAddresses = emailAddressProvider.GetEmailAddresses(person);

                            Console.WriteLine($"Attempting to email {person} using...");
                            foreach (var emailAddress in emailAddresses)
                            {
                                emailCount++;
                                var content = CreateEmailMessage(emailAddress, person, giftMessage);
                                var testEnvelope = new Envelope(content);
                                var emailSendStatus = emailer.Notify(testEnvelope).GetAwaiter().GetResult();
                                if (!emailSendStatus.IsSuccess())
                                {
                                    Console.WriteLine($"{emailAddress} Status: Error: {emailSendStatus.Message}");
                                }
                                else
                                {
                                    Console.WriteLine($"{emailAddress} Status: Ok");
                                }
                            }
                            Console.WriteLine(giftMessage);
                        }

                    }
                }
                Console.WriteLine($"Sent {emailCount} for {family.Name}");
            }
            Console.WriteLine($"Sent {emailCount} emails");
        }
    }
}
