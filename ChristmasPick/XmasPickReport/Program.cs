using System;
using System.Collections.Generic;
using System.Text;
using Common.ChristmasPickList;
using Common;

namespace XmasPickReport
{
  class Program
  {
    static void Main(string[] args)
    {
        DateTime christmasThisYear = new DateTime(2020, 12, 25);
        string adultArchivePath = @"/Users/cgehrer/Code/ChristmasPick/Archive/Adult/Archive.xml";
        string kidArchivePath = @"/Users/cgehrer/Code/ChristmasPick/Archive/Kids/Archive.xml";
        IXMasArchivePersister adultPersister = new FileArchivePersister(adultArchivePath);
        IXMasArchivePersister kidPersister = new FileArchivePersister(kidArchivePath);
        IFamilyProvider familyProvider = new FileFamilyProvider(@"/Users/cgehrer/Code/ChristmasPick/Archive/Gehred/GehredFamily.xml");

        XMasArchive adultArchive = adultPersister.LoadArchive();
        XMasArchive kidArchive = kidPersister.LoadArchive();
        FamilyTree gehredFamily = familyProvider.GetFamilies();

        XMasPickList adultPickList = adultArchive.GetPickListForYear(christmasThisYear);
        XMasPickList kidPickList = kidArchive.GetPickListForYear(christmasThisYear);

      // Okay loop through each family.
      // For each family member
      // Determine if adult or child
      // Using the correct archive find that pick by subject
      // Write out a line as follows
      // For the [ChristmasDate] {Subject] will buy a ${Amount] for [Recipient]
      string pickMsg = "\tFor the Christmas of {0} {1} will buy a {2} gift for {3}";
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
            Console.WriteLine(string.Format(pickMsg, christmasThisYear.Year, person.ToString(), giftAmount.ToString("c"), recipient.ToString()));
          }
          else
          {
            decimal giftAmount = 5.00M;
            Person recipient = adultPickList.GetRecipientFor(person);
            Console.WriteLine(string.Format(pickMsg, christmasThisYear.Year, person.ToString(), giftAmount.ToString("c"), recipient.ToString()));
          }
        }
        Console.WriteLine(string.Format("List complete for {0}", family.Name));
        Console.WriteLine("");
      }

    }
  }
}
