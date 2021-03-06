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

      Post["/store/new"] = _ => {
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

      Patch["/edit/store/{id}"] =parameters=> {
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
        List<Brand> thisBrands = thisStore.GetBrands();
        List<Brand> allBrands = Brand.GetAll();
        newDictionary.Add("brands", thisBrands);
        newDictionary.Add("store", thisStore);
        newDictionary.Add("allBrands", allBrands);
        return View["storeBrands.cshtml", newDictionary];
      };

      Post["/brand/new/{id}"] = parameters => {
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        Store thisStore = Store.Find(parameters.id);
        Brand newBrand = new Brand(Request.Form["newBrand"]);
        newBrand.Save();
        thisStore.AddBrand(newBrand);
        List<Brand> thisBrands = thisStore.GetBrands();
        List<Brand> allBrands = Brand.GetAll();
        newDictionary.Add("allBrands", allBrands);
        newDictionary.Add("brands", thisBrands);
        newDictionary.Add("store", thisStore);
        return View["storeBrands.cshtml", newDictionary];
      };

      Get["/stores/{id}"] = parameters => {
        Dictionary<string, object> testDictionary = new Dictionary<string, object>();
        Brand thisBrand = Brand.Find(parameters.id);
        List<Store>allStores = thisBrand.GetStores();
        testDictionary.Add("stores", allStores);
        testDictionary.Add("brand", thisBrand);
        return View["brandStores.cshtml", testDictionary];
      };

      Post["/brand/existing/{id}"] = parameters => {
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        Store thisStore = Store.Find(parameters.id);
        Brand thisBrand = Brand.FindTitle(Request.Form["oldBrand"]);
        thisStore.AddBrand(thisBrand);
        List<Brand> thisBrands = thisStore.GetBrands();
        List<Brand> allBrands = Brand.GetAll();
        newDictionary.Add("brands", thisBrands);
        newDictionary.Add("store", thisStore);
        newDictionary.Add("allBrands", allBrands);
        return View["storeBrands.cshtml", newDictionary];
      };





    }
  }
}
