﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;
using System;

namespace Common.Test
{
  [TestClass]
  public class FamilyTreeFixture : BaseFixture
  {

    public Family CreateTestGehredFamily()
    {
        Person AnnG = new Person("test", "Gehred", new DateTime(1962, 9, 26), "11121111-2222-3333-4444-555555555555");
        Person JohnG = new Person("test2", "Gehred", new DateTime(1962, 2, 13), "11131111-2222-3333-4444-555555555555");
        Person MadelineG = new Person("test3", "Gehred", new DateTime(1988, 4, 15), "11141111-2222-3333-4444-555555555555");
        Person CecilaG = new Person("test4", "Gehred", new DateTime(1990, 6, 21), "11151111-2222-3333-4444-555555555555");


      PersonCollection parents = new PersonCollection(AnnG, JohnG);
      PersonCollection kids = new PersonCollection();
      kids.Add(MadelineG);
      kids.Add(CecilaG);

      return new Family("Test Gehreds", parents, kids);

    }

    public Family CreateTest2GehredFamily()
    {
        Person AnnG = new Person("test5", "Gehred", new DateTime(1962, 9, 26), "11311111-2222-3333-4444-555555555555");
        Person JohnG = new Person("test6", "Gehred", new DateTime(1962, 2, 13), "11411111-2222-3333-4444-555555555555");
        Person MadelineG = new Person("test7", "Gehred", new DateTime(1988, 4, 15), "11511111-2222-3333-4444-555555555555");
        Person CecilaG = new Person("test8", "Gehred", new DateTime(1990, 6, 21), "11611111-2222-3333-4444-555555555555");


      PersonCollection parents = new PersonCollection(AnnG, JohnG);
      PersonCollection kids = new PersonCollection();
      kids.Add(MadelineG);
      kids.Add(CecilaG);

      return new Family("Test Gehreds", parents, kids);

    }

    public Family CreateTosaGehredFamily()
    {
      Person AnnG = new Person("Ann", "Gehred", new DateTime(1962, 9, 26), "12111111-2222-3333-4444-555555555555");
      Person JohnG = new Person("John", "Gehred", new DateTime(1962, 2, 13), "13111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");


      PersonCollection parents = new PersonCollection(AnnG, JohnG);
      PersonCollection kids = new PersonCollection();
      kids.Add(MadelineG);
      kids.Add(CecilaG);

      return new Family("WaWa Tosa Gehreds", parents, kids);

    }


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

    protected FamilyTree CreateFamilyTree()
    {
      FamilyTree testFamilyTree = new FamilyTree();
      testFamilyTree.Add(CreateMilwaukeeGehredFamily());
      testFamilyTree.Add(CreateTosaGehredFamily());
      return testFamilyTree;
    }

    [TestMethod]
    public void TestFamilyTreeSerialization()
    {
      FamilyTree testFamilyTree = CreateFamilyTree();

      Stream myStream = new BufferedStream(new MemoryStream(new byte[2048], true), 2048);
      if (myStream != null)
      {
        XmlSerializer xml = new XmlSerializer(typeof(FamilyTree));
        xml.Serialize(myStream, testFamilyTree);
      }

      long length = myStream.Position;
      myStream.Position = 0;
      StreamReader serializedStream = new StreamReader(myStream);
      string actual = serializedStream.ReadToEnd();
      actual = actual.Substring(0, (int)length);
      Assert.AreEqual("<?xml version=\"1.0\"?>\r\n<FamilyTree>\r\n  <Family>\r\n    <FamilyName>Brew City Gehreds</FamilyName>\r\n    <Parents>\r\n      <Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\r\n    </Parents>\r\n    <Children>\r\n      <Person firstname=\"Max\" lastname=\"Gehred\" birthday=\"9/30/2001\" id=\"31111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"Charlotte\" lastname=\"Gehred\" birthday=\"4/21/2005\" id=\"41111111-2222-3333-4444-555555555555\" />\r\n    </Children>\r\n  </Family>\r\n  <Family>\r\n    <FamilyName>WaWa Tosa Gehreds</FamilyName>\r\n    <Parents>\r\n      <Person firstname=\"Ann\" lastname=\"Gehred\" birthday=\"9/26/1962\" id=\"12111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"John\" lastname=\"Gehred\" birthday=\"2/13/1962\" id=\"13111111-2222-3333-4444-555555555555\" />\r\n    </Parents>\r\n    <Children>\r\n      <Person firstname=\"Madeline\" lastname=\"Gehred\" birthday=\"4/15/1988\" id=\"14111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"Cecila\" lastname=\"Gehred\" birthday=\"6/21/1990\" id=\"15111111-2222-3333-4444-555555555555\" />\r\n    </Children>\r\n  </Family>\r\n</FamilyTree>", actual);

    }

    [TestMethod]
    public void TestFamilyTreeDeserialization()
    {
      FamilyTree testFamilyTree = CreateFamilyTree();
      FamilyTree actual = null;
      byte[] serializedFamily = ConvertStringToByteArray("<?xml version=\"1.0\"?>\r\n<FamilyTree>\r\n  <Family>\r\n    <FamilyName>Brew City Gehreds</FamilyName>\r\n    <Parents>\r\n      <Person firstname=\"Angie\" lastname=\"Gehred\" birthday=\"9/26/1971\" id=\"11111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"Bob\" lastname=\"Gehred\" birthday=\"7/27/1972\" id=\"21111111-2222-3333-4444-555555555555\" />\r\n    </Parents>\r\n    <Children>\r\n      <Person firstname=\"Max\" lastname=\"Gehred\" birthday=\"9/30/2001\" id=\"31111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"Charlotte\" lastname=\"Gehred\" birthday=\"4/21/2005\" id=\"41111111-2222-3333-4444-555555555555\" />\r\n    </Children>\r\n  </Family>\r\n  <Family>\r\n    <FamilyName>WaWa Tosa Gehreds</FamilyName>\r\n    <Parents>\r\n      <Person firstname=\"Ann\" lastname=\"Gehred\" birthday=\"9/26/1962\" id=\"12111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"John\" lastname=\"Gehred\" birthday=\"2/13/1962\" id=\"13111111-2222-3333-4444-555555555555\" />\r\n    </Parents>\r\n    <Children>\r\n      <Person firstname=\"Madeline\" lastname=\"Gehred\" birthday=\"4/15/1988\" id=\"14111111-2222-3333-4444-555555555555\" />\r\n      <Person firstname=\"Cecila\" lastname=\"Gehred\" birthday=\"6/21/1990\" id=\"15111111-2222-3333-4444-555555555555\" />\r\n    </Children>\r\n  </Family>\r\n</FamilyTree>");
      Stream testData = new MemoryStream(serializedFamily);
      XmlSerializer xml = new XmlSerializer(typeof(FamilyTree));
      actual = (FamilyTree)xml.Deserialize(testData);
      Assert.AreEqual(testFamilyTree, actual);
    }

    [TestMethod]
    public void TestFamilyTreeCompareShouldNotBeEqualBecauseNumberOfFamiliesInEachTreeAreDifferent()
    {
      FamilyTree oneFamilyTree = CreateFamilyTree();
      FamilyTree twoFamilyTree = CreateFamilyTree();
      twoFamilyTree.Add(CreateTestGehredFamily());
      bool actual = (oneFamilyTree != twoFamilyTree);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TestFamilyTreeCompareShouldBeEqual()
    {
      FamilyTree oneFamilyTree = CreateFamilyTree();
      FamilyTree twoFamilyTree = CreateFamilyTree();
      bool actual = (oneFamilyTree == twoFamilyTree);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TestFamilyTreeCompareShouldNotBeEqualBecauseFamilyNamesAreDifferent()
    {
      FamilyTree oneFamilyTree = new FamilyTree();
      oneFamilyTree.Add(this.CreateTestGehredFamily());
      FamilyTree twoFamilyTree = new FamilyTree();
      twoFamilyTree.Add(this.CreateTosaGehredFamily());
      bool actual = (oneFamilyTree != twoFamilyTree);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TestFamilyTreeCompareShouldNotBeEqualBecauseFamilyMembersAreDifferent()
    {
      FamilyTree oneFamilyTree = new FamilyTree();
      oneFamilyTree.Add(this.CreateTestGehredFamily());
      FamilyTree twoFamilyTree = new FamilyTree();
      twoFamilyTree.Add(this.CreateTest2GehredFamily());
      bool actual = (oneFamilyTree != twoFamilyTree);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void CreateKidListFromFamilyTree()
    {
      FamilyTree testFamilyTree = CreateFamilyTree();
      DateTime christmas2008 = new DateTime(2008, 12, 25);
      PersonCollection actualKidList = testFamilyTree.CreateChristmasKidList(christmas2008);
      PersonCollection expectedKidList = new PersonCollection();

      Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
      Person CharlotteG = new Person("Charlotte", "Gehred", new DateTime(2005, 4, 21), "41111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");
      expectedKidList.Add(MaxG);
      expectedKidList.Add(CharlotteG);
      expectedKidList.Add(MadelineG);
      expectedKidList.Add(CecilaG);

      Assert.IsTrue((actualKidList == expectedKidList));
    }

    [TestMethod]
    public void CreateAdultListFromFamilyTree()
    {
      FamilyTree testFamilyTree = CreateFamilyTree();
      DateTime christmas2008 = new DateTime(2008, 12, 25);
      PersonCollection actualAdultList = testFamilyTree.CreateChristmasAdultList(christmas2008);
      PersonCollection expectedAdultList = new PersonCollection();

      Person AnnG = new Person("Ann", "Gehred", new DateTime(1962, 9, 26), "12111111-2222-3333-4444-555555555555");
      Person JohnG = new Person("John", "Gehred", new DateTime(1962, 2, 13), "13111111-2222-3333-4444-555555555555");
      Person AngieG = new Person("Angie", "Gehred", new DateTime(1971, 9, 26), "11111111-2222-3333-4444-555555555555");
      Person BobG = new Person("Bob", "Gehred", new DateTime(1972, 7, 27), "21111111-2222-3333-4444-555555555555");

      expectedAdultList.Add(AnnG);
      expectedAdultList.Add(JohnG);
      expectedAdultList.Add(AngieG);
      expectedAdultList.Add(BobG);

      Assert.IsTrue((expectedAdultList == actualAdultList));
    }

    [TestMethod]
    public void AreSiblingsShouldReturnFalseBecausePersonAIsParentPersonBIsChild()
    {
      FamilyTree testObj = CreateFamilyTree();
      Person JohnG = new Person("John", "Gehred", new DateTime(1962, 2, 13), "13111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Assert.IsFalse(testObj.AreSiblings(JohnG, MadelineG));
    }

    [TestMethod]
    public void AreSiblingsShouldReturnFalseBecausePersonBIsParentPersonAIsChild()
    {
      FamilyTree testObj = CreateFamilyTree();
      Person JohnG = new Person("John", "Gehred", new DateTime(1962, 2, 13), "13111111-2222-3333-4444-555555555555");
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Assert.IsFalse(testObj.AreSiblings(MadelineG, JohnG));
    }

    [TestMethod]
    public void AreSiblingsShouldReturnFalseBecausePersonAIsParentPersonBIsParent()
    {
      FamilyTree testObj = CreateFamilyTree();
      Person JohnG = new Person("John", "Gehred", new DateTime(1962, 2, 13), "13111111-2222-3333-4444-555555555555");
      Person AnnG = new Person("Ann", "Gehred", new DateTime(1962, 9, 26), "12111111-2222-3333-4444-555555555555");
      Assert.IsFalse(testObj.AreSiblings(AnnG, JohnG));
    }

    [TestMethod]
    public void AreSiblingsShouldReturnTrueBecausePersonAIsChildPersonBIsChild()
    {
      FamilyTree testObj = CreateFamilyTree();
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");
      Assert.IsTrue(testObj.AreSiblings(MadelineG, CecilaG));
    }

    [TestMethod]
    public void AreParentChildShouldReturnFalseBecausePersonAIsChildAndPersonBIsChild()
    {
      FamilyTree testObj = CreateFamilyTree();
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person CecilaG = new Person("Cecila", "Gehred", new DateTime(1990, 6, 21), "15111111-2222-3333-4444-555555555555");
      Assert.IsFalse(testObj.AreParentChild(MadelineG, CecilaG));
    }

    [TestMethod]
    public void AreParentChildShouldReturnFalseBecausePersonAIsNotInTheSameFamilyAsPersonB()
    {
      FamilyTree testObj = CreateFamilyTree();
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person MaxG = new Person("Max", "Gehred", new DateTime(2001, 9, 30), "31111111-2222-3333-4444-555555555555");
      Assert.IsFalse(testObj.AreParentChild(MadelineG, MaxG));
    }

    [TestMethod]
    public void AreParentChildShouldReturnTrueBecausePersonAIsChildAndPersonBIsParent()
    {
      FamilyTree testObj = CreateFamilyTree();
      Person MadelineG = new Person("Madeline", "Gehred", new DateTime(1988, 4, 15), "14111111-2222-3333-4444-555555555555");
      Person AnnG = new Person("Ann", "Gehred", new DateTime(1962, 9, 26), "12111111-2222-3333-4444-555555555555");
      Assert.IsTrue(testObj.AreParentChild(MadelineG, AnnG));
    }

  }
}
