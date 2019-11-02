using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EmailAddress
    {
        private string emailAddress;
        public EmailAddress(string emailAddress)
        {
            this.emailAddress = (string.IsNullOrEmpty(emailAddress) == false) ? emailAddress : throw new ArgumentNullException(emailAddress);
        }

        public override string ToString()
        {
            return emailAddress;
        }

        public static implicit operator string(EmailAddress x)
        {
            return x.emailAddress;
        }
    }
    public interface IEmailAddressProvider
    {
        IEnumerable<EmailAddress> GetEmailAddresses(Person x);
    }

    public class JsonFileEmailAddressProvider : IEmailAddressProvider
    {
        public class Key
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime Birthday { get; set; }

            public static Key CreateFromPerson(Person x)
            {
                return new Key
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Birthday = x.BirthDay
                };
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }
                if (obj.GetType() == typeof(Key))
                {
                    var tmpObj = (Key)obj;
                    if ((string.Compare(FirstName, tmpObj.FirstName) == 0) &&
                        (string.Compare(LastName, tmpObj.LastName) == 0) &&
                        (Birthday == tmpObj.Birthday))
                    {
                        return true;
                    }
                }

                return false;
            }

            public override int GetHashCode()
            {
                return 0;
            }

        }
        public class ContactEntry
        {
            public Key Key { get; set; }
            public string[] Emails { get; set; }
        }

        private IDictionary<Key, EmailAddress[]> emails;
        public JsonFileEmailAddressProvider(string pathToFamilyContacts)
        {
            emails = new Dictionary<Key, EmailAddress[]>();
            var contacts = JsonConvert.DeserializeObject<ContactEntry[]>(File.ReadAllText(pathToFamilyContacts));
            foreach(var contact in contacts)
            {
                var contactEmails = new List<EmailAddress>();
                foreach(var email in contact.Emails)
                {
                    contactEmails.Add(new EmailAddress(email));

                }
                emails.Add(contact.Key, contactEmails.ToArray());
            }
        }
        public IEnumerable<EmailAddress> GetEmailAddresses(Person x)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            var key = Key.CreateFromPerson(x);
            if (emails.ContainsKey(key))
            {
                return emails[key];
            }
            throw new ApplicationException($"Cound not find {x} in contact list.");
        }

        // Delete all of this just for getting started
        //public void CreateTemplateJson(IFamilyProvider family)
        //{
        //    var contacts = new List<ContactEntry>();
        //    foreach(var unit in family.GetFamilies())
        //    {
        //        foreach(var person in unit)
        //        {
        //            // Create a new key
        //            var key = new Key()
        //            {
        //                FirstName = person.FirstName,
        //                LastName = person.LastName,
        //                Birthday = person.BirthDay
        //            };
        //            var emails = new string[] { "notset@junk.com" };
        //            contacts.Add(new ContactEntry
        //            {
        //                Emails = emails,
        //                Key = key
        //            });
        //        }
        //    }
        //    JsonSerializer serializer = new JsonSerializer();
        //    serializer.Formatting = Formatting.Indented;
        //    using (StreamWriter sw = new StreamWriter(@"C:\src\gehredproject\ChristmasPick\Archive\Gehred\GehredFamily_emails.json"))
        //    using (JsonWriter writer = new JsonTextWriter(sw))
        //    {
        //        serializer.Serialize(writer, contacts);
        //    }
        //}
        // End Delete
    }
}
