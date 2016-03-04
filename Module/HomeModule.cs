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
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };

      Post["/createStore"] = _ => {
        Store newStore = new Store(Request.Form["storeName"]);
        newStore.Save();
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Store> allStores = Store.GetAll();
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };

      Delete["/delete/{id}"] =parameters=> {
        Store newStore = Store.Find(parameters.id);
        newStore.Delete();
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Store> allStores = Store.GetAll();
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };

      Patch["/editStore/{id}"] =parameters=> {
        Store newStore = Store.Find(parameters.id);
        newStore.Update(Request.Form["editStoreName"]);
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Store> allStores = Store.GetAll();
        newDictionary.Add("stores", allStores);
        return View["index.cshtml", newDictionary];
      };



    }
  }
}
