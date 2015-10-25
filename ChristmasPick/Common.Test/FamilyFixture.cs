using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;

namespace Common.Test
{
  [TestClass]
  public class FamilyFixture : BaseFixture
  {

    public Family CreateMilwaukeeGehredFamily()
    {
        Person AngieG = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");
        Person BobG = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
        Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
        Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");


      PersonCollection parents = new PersonCollection(AngieG, BobG);
      PersonCollection kids = new PersonCollection();
      kids.Add(MaxG);
      kids.Add(CharlotteG);

      return new Family("Brew City Gehreds", parents, kids);

    }

    [TestMethod]
    public void TestFamilySerialization()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Stream myStream = new BufferedStream(new MemoryStream(new byte[1024], true), 1024);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(Family));
        xml.Serialize(myStream, testFamily);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader serializedStream = new StreamReader(myStream);
      string actual = serializedStream.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.AreEqual("<?xml version=\"1.0\"?>\r\n<Family>\r\n  <FamilyName>Brew City Gehreds</FamilyName>\r\n  <Parents>\r\n    <Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\r\n    <Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\r\n  </Parents>\r\n  <Children>\r\n    <Person firstname=\"Max\" lastname=\"Gehred\" birthday=\"9/30/2001\" id=\"31111111-2222-3333-4444-555555555555\" />\r\n    <Person firstname=\"Charlotte\" lastname=\"Gehred\" birthday=\"4/21/2005\" id=\"41111111-2222-3333-4444-555555555555\" />\r\n  </Children>\r\n</Family>", actual);

    }

    [TestMethod]
    public void TestFamilyDeserialization()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Family actual = null;
      byte[] serializedFamily = ConvertStringToByteArray("<?xml version=\"1.0\"?>\r\n<Family>\r\n  <FamilyName>Brew City Gehreds</FamilyName>\r\n  <Parents>\r\n    <Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\r\n    <Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\r\n  </Parents>\r\n  <Children>\r\n    <Person firstname=\"Max\" lastname=\"Gehred\" birthday=\"9/30/2001\" id=\"31111111-2222-3333-4444-555555555555\" />\r\n    <Person firstname=\"Charlotte\" lastname=\"Gehred\" birthday=\"4/21/2005\"  id=\"41111111-2222-3333-4444-555555555555\" />\r\n  </Children>\r\n</Family>");
      Stream testData = new MemoryStream(serializedFamily);
      XmlSerializer xml = new XmlSerializer(typeof(Family));
      actual = (Family)xml.Deserialize(testData);
      Assert.AreEqual(testFamily, actual);
    }

    [TestMethod]
    public void FamilyEquivelanceOperatorShouldreturnTrue()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Family anotherFamily = CreateMilwaukeeGehredFamily();
      bool actual = (testFamily == anotherFamily);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void IsFamilyMemberShouldReturnFalse()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person alfredENeuman = new Person("Alfred", "Nueman", new DateTime(1972, 7, 27), "11111111-2222-3333-6666-555555555555");
      Assert.IsFalse(testFamily.IsFamilyMember(alfredENeuman));
    }

    [TestMethod]
    public void IsFamilyMemberShouldReturnTrueBecausePersonIsInParentCollection()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person AngieG = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");
      Assert.IsTrue(testFamily.IsFamilyMember(AngieG));
    }

    [TestMethod]
    public void IsFamilyMemberShouldReturnTrueBecausePersonIsInChildrenCollection()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Assert.IsTrue(testFamily.IsFamilyMember(CharlotteG));
    }

    [TestMethod, ExpectedException(typeof(ArgumentException))]
    public void DetermineRelationshipShouldThrowExceptionSamePersonPassedIn()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person Charlotte = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      testFamily.DetermineRelationship(CharlotteG, Charlotte);
    }

    [TestMethod, ExpectedException(typeof(ArgumentException))]
    public void DetermineRelationshipShouldThrowExceptionNeitherPersonInFamily()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person Bill = new Person("Bill", "Wong", new DateTime(2005, 4, 21), "91111111-2222-3333-4444-555555555555");
      Person Ted = new Person("Ted", "Wong", new DateTime(2005, 4, 21), "81111111-2222-3333-4444-555555555555");
      testFamily.DetermineRelationship(Bill, Ted);
    }

    [TestMethod, ExpectedException(typeof(ArgumentException))]
    public void DetermineRelationshipShouldThrowExceptionMalformedFamily()
    {
      Person AngieG = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");
      Person BobG = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");

      PersonCollection parents = new PersonCollection(AngieG, BobG);
      PersonCollection kids = new PersonCollection();
      kids.Add(AngieG);
      kids.Add(BobG);

      Family testFamily = new Family("My Dad is myself", parents, kids);
      testFamily.DetermineRelationship(AngieG, BobG);
    }

    [TestMethod]
    public void DetermineRelationshipShouldReturnNoRelationPersonAIsInFamilyPersonBIsNot()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person Ted = new Person("Ted", "Wong", new DateTime(2005, 4, 21), "51111111-2222-3333-4444-555555555555");
      FamilyRelation actual = testFamily.DetermineRelationship(CharlotteG, Ted);
      Assert.AreEqual(FamilyRelation.NoRelation, actual);
    }

    [TestMethod]
    public void DetermineRelationshipShouldReturnNoRelationPersonBIsInFamilyPersonAIsNot()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person Ted = new Person("Ted", "Wong", new DateTime(2005, 4, 21), "51111111-2222-3333-4444-555555555555");
      FamilyRelation actual = testFamily.DetermineRelationship(Ted, CharlotteG);
      Assert.AreEqual(FamilyRelation.NoRelation, actual);
    }

    [TestMethod]
    public void DetermineRelationshipShouldReturnParentChildbecausePersonAIsParentPersonBIsChild()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person BobG = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
      FamilyRelation actual = testFamily.DetermineRelationship(BobG, CharlotteG);
      Assert.AreEqual(FamilyRelation.ParentChild, actual);
    }

    [TestMethod]
    public void DetermineRelationshipShouldReturnParentChildbecausePersonBIsParentPersonAIsChild()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person BobG = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
      FamilyRelation actual = testFamily.DetermineRelationship(CharlotteG, BobG);
      Assert.AreEqual(FamilyRelation.ParentChild, actual);
    }

    [TestMethod]
    public void DetermineRelationshipShouldReturnParentSiblingsbecausePersonAIsChildPersonBIsChild()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
      FamilyRelation actual = testFamily.DetermineRelationship(CharlotteG, MaxG);
      Assert.AreEqual(FamilyRelation.Siblings, actual);
    }

    [TestMethod]
    public void DetermineRelationshipShouldReturnSpouseBecausePersonAIsParentAndPersonBIsParent()
    {
      Family testFamily = CreateMilwaukeeGehredFamily();
      Person AngieG = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");
      Person BobG = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");
      FamilyRelation actual = testFamily.DetermineRelationship(BobG, AngieG);
      Assert.AreEqual(FamilyRelation.Spouse, actual);
    }
    
  }
}

