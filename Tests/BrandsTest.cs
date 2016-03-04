using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShoeStoreNameSpace
{
  public class BrandTest : IDisposable
  {
    public BrandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_EmptyAtFirst()
    {
      //Arrange, Act
      int result = Brand.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Brand firstBrand = new Brand("Magic Johnson");
      Brand secondBrand = new Brand("Magic Johnson");
      //Assert
      Assert.Equal(firstBrand, secondBrand);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Brand testBrand = new Brand("Magic Johnson");
      testBrand.Save();
      //Act
      List<Brand> result = Brand.GetAll();
      List<Brand> testList = new List<Brand>{testBrand};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Brand testBrand = new Brand("Magic Johnson");
      testBrand.Save();
      //Act
      Brand savedBrand = Brand.GetAll()[0];
      int result = savedBrand.GetId();
      int testId = testBrand.GetId();
      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsBrandInDatabase()
    {
      //Arrange
      Brand testBrand = new Brand("Magic Johnson");
      testBrand.Save();
      //Act
      Brand result = Brand.Find(testBrand.GetId());
      //Assert
      Assert.Equal(testBrand, result);
    }

    [Fact]
    public void Test_GetStores_ReturnsAllBrandStores()
    {
      //Arrange
      Brand testBrand = new Brand("Magic Johnson");
      testBrand.Save();
      Store testStore1 = new Store("Math");
      testStore1.Save();
      Store testStore2 = new Store("Gym");
      testStore2.Save();
      //Act
      testBrand.AddStore(testStore1);
      List<Store> result = testBrand.GetStores();
      List<Store> testList = new List<Store> {testStore1};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Dispose()
    {
      Brand.DeleteAll();
    }
  }
}
