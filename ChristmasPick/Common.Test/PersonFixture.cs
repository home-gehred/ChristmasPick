using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Serialization;

namespace Common.Test
{
  [TestClass]
  public class PersonFixture : BaseFixture
  {
    public PersonFixture()
    {
    }

    [TestMethod]
    public void ReturnTheNumberOfYears()
    {
      DateTime bDay = new DateTime(1972, 7, 27);
      Person Bob = new Person("Bob", "Gehred", bDay, "21111111-2222-3333-4444-555555555555");
      DateTime testTime = new DateTime(2008, 7, 27);
      Age myAge = Bob.YearsOld(testTime);
      int expected = 36;
      Assert.AreEqual(expected, myAge.Year);
      Assert.AreEqual(0, myAge.Month);
      Assert.AreEqual(0, myAge.Day);
    }

    [TestMethod]
    public void TestPersonSerialization()
    {
        Person Bob = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
      Stream myStream = new BufferedStream(new MemoryStream(new byte[1024], true), 1024);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(Person));
        xml.Serialize(myStream, Bob);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader junk = new StreamReader(myStream);
      string actual = junk.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.AreEqual("<?xml version=\"1.0\"?>\r\n<Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />", actual);

    }

    [TestMethod]
    public void TestPersonDeserialization()
    {

      Person angie = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");
      Person actual = null;
      byte[] serializedPerson = ConvertStringToByteArray("<?xml version=\"1.0\"?>\r\n<Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />");
      Stream testData = new MemoryStream(serializedPerson);
      XmlSerializer xml = new XmlSerializer(typeof(Person));
      actual = (Person)xml.Deserialize(testData);
      Assert.AreEqual(angie, actual);    
    }

    [TestMethod]
    public void ShouldBeEquals()
    {
        Person personA = new Person("Bill", "Close", new DateTime(1990, 1, 1), "91111111-2222-3333-4444-555555555555");
        Person personB = new Person("Bill", "Close", new DateTime(1990, 1, 1), "91111111-2222-3333-4444-555555555555");
      Assert.IsTrue(personA == personB);
    }

    [TestMethod]
    public void ShouldNotBeEquals()
    {
        Person personA = new Person("Bill", "Close", new DateTime(1990, 1, 1), "91111111-2222-3333-4444-555555555555");
        Person personB = new Person("Jed", "Close", new DateTime(1990, 1, 1), "81111111-2222-3333-4444-555555555555");
      Assert.IsFalse(personA == personB);
    }

    [TestMethod]
    public void ComparingTwoPeopleShouldReturnNegativeOne()
    {
        Person personA = new Person("Nate", "Close", new DateTime(1990, 1, 1), "21111111-2222-3333-4444-555555555555");
        Person personB = new Person("Nate", "Albertson", new DateTime(1990, 1, 1), "31111111-2222-3333-4444-555555555555");
      Assert.AreEqual(1, personA.CompareTo(personB));
    }

    [TestMethod]
    public void PersonIsNotConsideredAChildBecauseAgeIsGreaterThenTwentyOneYearsOld()
    {
        Person personA = new Person("Rita", "Botkin", new DateTime(1932, 12, 23), "61111111-2222-3333-4444-555555555555");
      Assert.IsFalse(personA.IsConsideredAChild(DateTime.Now));
    }

    [TestMethod]
    public void PersonIsConsideredAChildBecauseAgeIsExactlyEqualToTwentyOneYearsOld()
    {
        Person personA = new Person("Rita", "Botkin", new DateTime(1932, 12, 23), "61111111-2222-3333-4444-555555555555");
      Assert.IsTrue(personA.IsConsideredAChild(new DateTime(1953, 12, 23)));
    }

    [TestMethod]
    public void PersonIsConsideredAChildBecauseAgeIsLessThenOneYearOld()
    {
        Person personA = new Person("Rita", "Botkin", new DateTime(1932, 12, 23), "61111111-2222-3333-4444-555555555555");
      Assert.IsTrue(personA.IsConsideredAChild(new DateTime(1932, 12, 25)));
    }

  }
}
