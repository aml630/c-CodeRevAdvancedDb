using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace ShoeStoreNameSpace
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Store> allStores = Store.GetAll();
        List<Brand> allBrands = Brand.GetAll();
        newDictionary.Add("brands", allBrands);
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };

      Post["/createStore"] = _ => {
        Store newStore = new Store(Request.Form["storeName"]);
        newStore.Save();
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Store> allStores = Store.GetAll();
        List<Brand> allBrands = Brand.GetAll();
        newDictionary.Add("brands", allBrands);
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };

      Delete["/delete/{id}"] =parameters=> {
        Store newStore = Store.Find(parameters.id);
        newStore.Delete();
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Store> allStores = Store.GetAll();
        List<Brand> allBrands = Brand.GetAll();
        newDictionary.Add("brands", allBrands);
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };

      Patch["/editStore/{id}"] =parameters=> {
        Store newStore = Store.Find(parameters.id);
        newStore.Update(Request.Form["editStoreName"]);
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Store> allStores = Store.GetAll();
        List<Brand> allBrands = Brand.GetAll();
        newDictionary.Add("brands", allBrands);
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };

      Get["/brands/{id}"] = parameters => {
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        Store thisStore = Store.Find(parameters.id);
        List<Brand> allBrands = thisStore.GetBrands();
        newDictionary.Add("brands", allBrands);
        newDictionary.Add("store", thisStore);
        return View["storeBrands.cshtml", newDictionary];
      };

      Post["/newBrand/{id}"] = parameters => {
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        Store thisStore = Store.Find(parameters.id);
        Brand newBrand = new Brand(Request.Form["newBrand"]);
        newBrand.Save();
        thisStore.AddBrand(newBrand);
        List<Brand> allBrands = thisStore.GetBrands();
        newDictionary.Add("brands", allBrands);
        newDictionary.Add("store", thisStore);
        return View["storeBrands.cshtml", newDictionary];
      };

      // Get["/stores/{id}"] = parameters => {
      //   Dictionary<string, object> newDictionary = new Dictionary<string, object>();
      //   Store thisStore = Store.Find(parameters.id);
      //   List<Brand> allBrands = thisStore.GetBrands();
      //   newDictionary.Add("brands", allBrands);
      //   newDictionary.Add("store", thisStore);
      //   return View["storeBrands.cshtml", newDictionary];
      // };






    }
  }
}
