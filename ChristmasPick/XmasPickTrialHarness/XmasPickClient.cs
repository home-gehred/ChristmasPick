using System;
using System.Collections.Generic;
using Common.ChristmasPickList;
using Common.ChristmasPickList.Rules;
using Common;

namespace XmasPickTrialHarness
{
    class XmasPickClient
    {
        static void Main(string[] args)
        {
                  DateTime christmasThisYear = new DateTime(2016, 12, 25);
                  string adultArchivePath = @"C:\Users\gehredbo\Source\Hobby\ChristmasPick\Archive\Adult\Archive.xml";
                  string kidArchivePath = @"C:\Users\gehredbo\Source\Hobby\ChristmasPick\Archive\Kids\Archive.xml";
                  IXMasArchivePersister adultPersister = new FileArchivePersister(adultArchivePath);
                  IXMasArchivePersister kidPersister = new FileArchivePersister(kidArchivePath);
                  IFamilyProvider familyProvider = new FileFamilyProvider(@"C:\Users\gehredbo\Source\Hobby\ChristmasPick\Archive\Gehred\GehredFamily.xml");
                  // Go ahead and get family
                  FamilyTree gehredFamily = familyProvider.GetFamilies();
                  XMasArchive adultArchive = adultPersister.LoadArchive();
                  XMasArchive kidArchive = kidPersister.LoadArchive();
                  // Create two poeple collections
                  PersonCollection kidList = gehredFamily.CreateChristmasKidList(christmasThisYear);
                  PersonCollection adultList = gehredFamily.CreateChristmasAdultList(christmasThisYear);

                  IPickListRuleProvider kidRules = new KidListRuleProvider(gehredFamily, kidArchive, 5);
                  IPickListService picker = new PickListServiceAdvanced(new RandomNumberGenerator(kidList.Count), kidRules, kidList);
                  XMasPickList kidPicklist = picker.CreateChristmasPick(christmasThisYear);
                  XMasPickListValidator validation = new XMasPickListValidator();
                  try
                  {
                      var checkList = validation.PickListToValidateWithPeopleList(kidList, kidPicklist);
                      if (validation.isPickListValid(checkList))
                      {
                          kidArchive.Add(christmasThisYear.Year, kidPicklist);
                          kidPersister.SaveArchive(kidArchive);
                      }
                      else
                      {
                          Console.WriteLine("The kid pick list has errors. Nothing was saved.");
                      }
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine("An exception occurred validating kids picklist: {0}", ex.ToString());
                  }


                  IPickListRuleProvider adultRules = new AdultListRuleProvider(gehredFamily, adultArchive, 5);
                  IPickListService adultPicker = new PickListServiceAdvanced(new RandomNumberGenerator(adultList.Count), adultRules, adultList);
                  XMasPickList adultPickList = adultPicker.CreateChristmasPick(christmasThisYear);

                  try
                  {
                      var checkList = validation.PickListToValidateWithPeopleList(adultList, adultPickList);
                      if (validation.isPickListValid(checkList))
                      {
                          adultArchive.Add(christmasThisYear.Year, adultPickList);
                          adultPersister.SaveArchive(adultArchive);
                      } 
                      else
                      {
                          Console.WriteLine("The adult pick list has errors. Nothing was saved.");
                      }
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine("An exception occurred: {0}", ex.ToString());
                  }
        }
 
    }

    public class KidListRuleProvider : IPickListRuleProvider
    {
        private FamilyTree mFamily;
        private XMasArchive mArchive;
        private int mYearsBack;

        public KidListRuleProvider(FamilyTree family, XMasArchive archive, int years)
        {
            mFamily = family;
            mArchive = archive;
            mYearsBack = years;
        }

        public IList<IPickListRule> GetRulesForPickList()
        {
            List<IPickListRule> testRules = new List<IPickListRule>();
            testRules.Add(new SiblingRule(mFamily));
            testRules.Add(new ChristmasPastRule(mArchive, mYearsBack));
            return testRules;
        }
    }

    public class AdultListRuleProvider : IPickListRuleProvider
    {
        private FamilyTree mFamily;
        private XMasArchive mArchive;
        private int mYearsBack;

        public AdultListRuleProvider(FamilyTree family, XMasArchive archive, int years)
        {
            mFamily = family;
            mArchive = archive;
            mYearsBack = years;
        }

        public IList<IPickListRule> GetRulesForPickList()
        {
            List<IPickListRule> testRules = new List<IPickListRule>();
            testRules.Add(new SiblingRule(mFamily));
            testRules.Add(new ParentChildRule(mFamily));
            testRules.Add(new SpouseRule(mFamily));
            testRules.Add(new ChristmasPastRule(mArchive, mYearsBack));
            return testRules;
        }
    }

    public class BosmaPickListRuleProvider : IPickListRuleProvider
    {
        private FamilyTree mFamily;
        private XMasArchive mArchive;
        private int mYearsBack;

        public BosmaPickListRuleProvider(FamilyTree family, XMasArchive archive, int years)
        {
            mFamily = family;
            mArchive = archive;
            mYearsBack = years;
        }

        public IList<IPickListRule> GetRulesForPickList()
        {
            List<IPickListRule> testRules = new List<IPickListRule>();
            testRules.Add(new SpouseRule(mFamily));
            testRules.Add(new ChristmasPastRule(mArchive, mYearsBack));
            return testRules;
        }
    }

}
