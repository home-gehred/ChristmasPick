using ChristmasPickMessages;
using ChristmasPickMessages.Messages;
using ChristmasPickNotifier.Notifier.Email;
using Common;
using Common.ChristmasPickList;
using System;
using System.IO;
using SecurityUtil.SecretsProvider;
using ChristmasPickNotifier.Notifier;

namespace XmasPickEmail
{
    class Program
    {
        static private PickAvailableMessage CreateEmailMessage(EmailAddress To, Person giftMaker, string pickMessage)
        {
            var plainTextMsg = $@"Merry Christmas {giftMaker},
[[newline]]
IMPORTANT: I expect problems as this is the second time I have used the automatic email process. Please, Please, Please reply to this email that you got this notification, AND send an email to bagehred@sbcglobal.net
[[newline]]
The response is important for me to keep track of how this new notification process is working.
And as always, notify family members you got your name for Christmas. Relay that, if they do not have a name, to get in touch with Uncle Bob. (bagehred@sbcglobal.net)
[[newline]]
This could be duplicate email, if you have responded already I apologize and disregard, otherwise please read on.
[[newline]]
I for one am happy that 2020 is coming to an end! What a year.
I truly hope this letter finds you safe, healthy and doing well.
As acting Supreme Master Overlord of Christmas we are allowing you {giftMaker} a choice,
you may either make a donation of $10 to a charity or your choice on behalf of the recipient
or you may stick with the standard $5 gift.
(Except for the person who has to buy Uncle Bob a present, that person is expected to have a $5.50 gift delivered in the mail to <<4959 N Idlewild Ave>>  in time for him to open on Christmas day.)
[[newline]]
In either case you must be sure that your person will have “something” to open by Christmas day.
A more with it Supreme Master Overlord of Christmas should have sent this earlier knowing that USPS is under some stress,
but tradition is tradition and emails come out the weekend of trick-or-treat.
[[newline]]
Sadly, my favorite part of Christmas, getting together with the whole family is not going to happen this year.
I will drown my sorrows in a half gallon of Egg Nog.
[[newline]]
For those of you that are a lucky enough to buy a present for anyone in the Hoorneart family;
they will be in Wisconsin Dec. 25th to Dec 31st, I’m hoping during the holidays we can do some
social distant gatherings in small groups in the hopes of seeing everyone that way.
[[newline]]
Love,
[[newline]]
Supreme Master Overlord of Christmas a.k.a Bob
[[newline]]       
Important Dates:
[[newline]]
Super Saturday: Cancelled due to Covid-19
[[newline]]
Christmas Sing - A - Long: Cancelled due to Covid-19
[[newline]]
Gehred Nation Christmas: Cancelled due to Covid-19
[[newline]]
{pickMessage}";
            var htmlMsg = plainTextMsg.Replace("[[newline]]", "<p>");
            htmlMsg = htmlMsg.Replace("\n", string.Empty);
            plainTextMsg = plainTextMsg.Replace("[[newline]]", string.Empty);
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
                                //var emailSendStatus = NotifierResultFactory.CreateFailed("Just testing");
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
                                //var emailSendStatus = NotifierResultFactory.CreateFailed("Just testing");
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
