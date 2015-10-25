using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using System.IO;
using System.Xml.Serialization;

namespace Common.Test
{
  [TestClass]
  public class AgeFixture
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ShouldThrowExceptionNowIsEqualToMinValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      Age actual = Age.CalculateAge(DateTime.MinValue, bday);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ShouldThrowExceptionNowIsEqualToMaxValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      Age actual = Age.CalculateAge(DateTime.MaxValue, bday);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ShouldThrowExceptionBDayIsEqualToMinValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      DateTime now = new DateTime(2008,9,18);
      Age actual = Age.CalculateAge(now, DateTime.MinValue);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ShouldThrowExceptionBDayIsEqualToMaxValue()
    {
      DateTime bday = new DateTime(1972, 7, 27);
      DateTime now = new DateTime(2008, 9, 18);
      Age actual = Age.CalculateAge(now, DateTime.MaxValue);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ShouldThrowExceptionBDayIsMoreRecentThenNow()
    {
      DateTime bday = new DateTime(2008, 9, 18);
      DateTime now = new DateTime(1972, 7, 27);
      Age actual = Age.CalculateAge(now, bday);
    }

    [TestMethod]
    public void BDayAndNowAreEqualYearAndDayShouldEqualZero()
    {
      DateTime bday = new DateTime(2008, 9, 18);
      DateTime now = bday;
      Age actual = Age.CalculateAge(now, bday);
      Assert.AreEqual(0, actual.Year);
      Assert.AreEqual(0, actual.Day);
    }

    [TestMethod]
    public void BDayAndNowAreLessThenOneYear()
    {
      DateTime bday = new DateTime(2008, 9, 18);
      DateTime now = new DateTime(2008, 9, 27); 
      Age actual = Age.CalculateAge(now, bday);
      Assert.AreEqual(0, actual.Year);
      Assert.AreEqual(9, actual.Day);
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(6, 4, 12);
      Assert.IsTrue((currentAge <= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 8, 12);
      Assert.IsTrue((currentAge <= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 4, 18);
      Assert.IsTrue((currentAge <= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsLessThenOrEqualToTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.IsTrue((currentAge <= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(6, 4, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.IsTrue((currentAge >= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 8, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.IsTrue((currentAge >= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 18);
      Age testAge = new Age(5, 4, 12);
      Assert.IsTrue((currentAge >= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsGreaterThenOrEqualToTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.IsTrue((currentAge >= testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsGreaterThenTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(6, 4, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.IsTrue((currentAge > testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsGreaterThenTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 8, 12);
      Age testAge = new Age(5, 4, 12);
      Assert.IsTrue((currentAge > testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsGreaterThenTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 18);
      Age testAge = new Age(5, 4, 12);
      Assert.IsTrue((currentAge > testAge));
    }

    [TestMethod]
    public void ShouldReturnFalseCurrentAgeIsGreaterThenTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.IsFalse((currentAge > testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsLessThenTestAgeBecauseOfYears()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(6, 4, 12);
      Assert.IsTrue((currentAge < testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsLessThenTestAgeBecauseOfMonths()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 8, 12);
      Assert.IsTrue((currentAge < testAge));
    }

    [TestMethod]
    public void ShouldReturnCurrentAgeIsLessThenTestAgeBecauseOfDays()
    {
      Age currentAge = new Age(5, 4, 12);
      Age testAge = new Age(5, 4, 18);
      Assert.IsTrue((currentAge < testAge));
    }

    [TestMethod]
    public void ShouldReturnFalseCurrentAgeIsLessThenTestAgeBecauseTheyAreEqual()
    {
      Age currentAge = new Age(5, 4, 21);
      Age testAge = new Age(5, 4, 21);
      Assert.IsFalse((currentAge < testAge));
    }

  }
}
