using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShoeStoreNameSpace
{
  public class StoreTest : IDisposable
  {
    public StoreTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_AddStore_AddsStoreToStores()
    {
      //Arrange
      Store testStore = new Store("Safeway");
      testStore.Save();
      Brand firstBrand = new Brand("Magic Johnson");
      firstBrand.Save();
      Brand secondBrand = new Brand("Magic James");
      secondBrand.Save();
      //Act
      testStore.AddBrand(firstBrand);
      testStore.AddBrand(secondBrand);
      List<Brand> result = testStore.GetBrands();
      List<Brand> testList = new List<Brand>{firstBrand, secondBrand};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Store.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Store testStore = new Store("Safeway");
      Store secondStore = new Store("Safeway");
      //Assert
      Assert.Equal(testStore, secondStore);
    }

    [Fact]
    public void Test_Save_SavesStoreToDatabase()
    {
      //Arrange
      Store testStore = new Store("Safeway");
      testStore.Save();
      //Act
      List<Store> result = Store.GetAll();
      List<Store> testList = new List<Store>{testStore};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToStoreObject()
    {
      //Arrange
      Store testStore = new Store("Safeway");
      testStore.Save();
      //Act
      Store savedStore = Store.GetAll()[0];
      int result = savedStore.GetId();
      int testId = testStore.GetId();
      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsStoreInDatabase()
    {
      //Arrange
      Store testStore = new Store("Safeway");
      testStore.Save();
      //Act
      Store foundStore = Store.Find(testStore.GetId());
      //Assert
      Assert.Equal(testStore, foundStore);
    }

    [Fact]
    public void Test_Update_UpdatesStoreInDatabase()
    {
      //Arrange
      Store testStore = new Store("Safeway");
      testStore.Save();
      string newName = "Work stuff";
      //Act
      testStore.Update(newName);
      string result = testStore.GetStore_name();
      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_Delete_DeletesStoreFromDatabase()
    {
      //Arrange
      string name1 = "Soccer";
      Store testStore1 = new Store(name1);
      testStore1.Save();
      string name2 = "Dancing";
      Store testStore2 = new Store(name2);
      testStore2.Save();
      //Act
      testStore1.Delete();
      List<Store> resultCategories = Store.GetAll();
      List<Store> testStoreList = new List<Store> {testStore2};
      //Assert
      Assert.Equal(testStoreList, resultCategories);
    }


    [Fact]
    public void Dispose()
    {
      Store.DeleteAll();
    }
  }
}
