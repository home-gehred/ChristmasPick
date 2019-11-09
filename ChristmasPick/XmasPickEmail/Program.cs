using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.Email;
using Common;
using Common.ChristmasPickList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmasPickEmail
{
    class Program
    {
        static private PickAvailableMessage CreateEmailMessage(EmailAddress To, Person giftMaker, string pickMessage)
        {
            var plainTextMsg = $"Hello {giftMaker},\r\n\r\n As you can see there are some new changes this year!\r\n Santa's Lil' Helper is yours truly, Uncle Bob.\r\n IMPORTANT: I am changing the notification process and I expect problems.\r\nPlease, Please, Please reply to this email that you got this notification, AND send an email to bagehred@sbcglobal.net\r\nThe response is important for me to keep track of how this new notification process is working.\r\n";
            plainTextMsg += $"And as always, please notify family members you got your name for Christmas. Relay that, if they do not have a name, to get in touch with Uncle Bob.\r\n";
            plainTextMsg += "\r\n";
            plainTextMsg += $"This maybe a duplicate, if you have responded already I apologize and disregard, otherwise get to it!\r\n";
            plainTextMsg += "\r\n";
            plainTextMsg += $"Alright, on to the fun stuff,\r\n";
            plainTextMsg += "On the bright side, I have fixed a bug in the name picking program to make sure everyone gets a name they will find easy to make your $5 gift for.\r\n";
            plainTextMsg += "Patti had a great suggestion for donating to a charity, the discussion on that will happen on Super Saturday.The name you need to supply a gift for is located at the end of this email.\r\n";
            plainTextMsg += "Claire has been added to the numbers this year, so you maybe the lucky one to introduce her to the exhilaration of the $5 gift.\r\n";
            plainTextMsg += "Important Dates:\r\n";
            plainTextMsg += "Super Saturday: Nov. 30th after 2 pm dinner at 5 pm At Rick and Anne's";
            plainTextMsg += "\r\n";
            plainTextMsg += "Christmas Sing - A - Long: Dec. 21st after 3 pm At Bob & Angie's\r\n";
            plainTextMsg += "\r\n";
            plainTextMsg += "Gehred Nation Christmas: Dec. 28th 1-9 dinner 5:30 ish at Retzer Nature Center\r\n";
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
                Subject = "IMPORTANT: Gehred Nation Christmas Pick Information Notification Attempt 3",
                ToAddress = To
            };
            return content;
        }

        static void Main(string[] args)
        {
            var emailer = new SendGridNotifyPickIsAvalable("");
            DateTime christmasThisYear = new DateTime(2019, 12, 25);
            string adultArchivePath = @"C:\src\gehredproject\ChristmasPick\Archive\Adult\Archive.xml";
            string kidArchivePath = @"C:\src\gehredproject\ChristmasPick\Archive\Kids\Archive.xml";
            IXMasArchivePersister adultPersister = new FileArchivePersister(adultArchivePath);
            IXMasArchivePersister kidPersister = new FileArchivePersister(kidArchivePath);
            IFamilyProvider familyProvider = new FileFamilyProvider(@"C:\src\gehredproject\ChristmasPick\Archive\Gehred\GehredFamily.xml");
            JsonFileEmailAddressProvider emailAddressProvider = new JsonFileEmailAddressProvider(@"C:\src\gehredproject\ChristmasPick\Archive\Gehred\GehredFamily_Contacts.json");

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
