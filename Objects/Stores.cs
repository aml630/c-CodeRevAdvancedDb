using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ShoeStoreNameSpace
{
  public class Store
  {
    private int _id;
    private string _store_name;

    public Store(string Store_name, int Id = 0)
    {
      _id = Id;
      _store_name = Store_name;
    }

    public override bool Equals(System.Object otherStore)
    {
        if (!(otherStore is Store))
        {
          return false;
        }
        else
        {
          Store newStore = (Store) otherStore;
          bool idEquality = this.GetId() == newStore.GetId();
          bool store_nameEquality = this.GetStore_name() == newStore.GetStore_name();
          return (idEquality && store_nameEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetStore_name()
    {
      return _store_name;
    }
    public void SetStore_name(string newStore_name)
    {
      _store_name = newStore_name;
    }

    public void AddBrand(Brand newBrand)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO store_brand (store_id, brand_id) VALUES (@StoreId, @BrandId)", conn);

      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = this.GetId();
      cmd.Parameters.Add(StoreIdParameter);

      SqlParameter brandsIdParameter = new SqlParameter();
      brandsIdParameter.ParameterName = "@BrandId";
      brandsIdParameter.Value = newBrand.GetId();
      cmd.Parameters.Add(brandsIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Store> GetAll()
    {
      List<Store> allStores = new List<Store>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int StoreId = rdr.GetInt32(0);
        string StoreStore_name= rdr.GetString(1);
        Store newStore = new Store(StoreStore_name, StoreId);
        allStores.Add(newStore);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allStores;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stores (store_name) OUTPUT INSERTED.id VALUES (@StoreStore_name);", conn);

      SqlParameter store_nameParameter = new SqlParameter();
      store_nameParameter.ParameterName = "@StoreStore_name";
      store_nameParameter.Value = this.GetStore_name();
      cmd.Parameters.Add(store_nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM stores;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Store Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores WHERE id = @StoreId;", conn);

      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = id.ToString();
      cmd.Parameters.Add(StoreIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStoreId = 0;
      string foundStoreStore_name = null;

      while(rdr.Read())
      {
        foundStoreId = rdr.GetInt32(0);
        foundStoreStore_name = rdr.GetString(1);
      }
      Store foundStore = new Store(foundStoreStore_name, foundStoreId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStore;
    }

    public List<Brand> GetBrands()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      List<Brand> brands = new List<Brand>{};

      SqlCommand cmd = new SqlCommand("SELECT brands.* FROM stores JOIN store_brand on (stores.id = store_brand.store_id) JOIN brands on (brands.id = store_brand.brand_id) WHERE stores.id = @StoreId", conn);

      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = this.GetId();

      cmd.Parameters.Add(StoreIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int brandsId = rdr.GetInt32(0);
        string brandsName = rdr.GetString(1);

        Brand newBrand = new Brand(brandsName, brandsId);
        brands.Add(newBrand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return brands;
    }

    public void Update(string newStore_name)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE stores SET store_name = @NewStore_name OUTPUT INSERTED.store_name WHERE id = @StoreId;", conn);

      SqlParameter newStore_nameParameter = new SqlParameter();
      newStore_nameParameter.ParameterName = "@NewStore_name";
      newStore_nameParameter.Value = newStore_name;
      cmd.Parameters.Add(newStore_nameParameter);


      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = this.GetId();
      cmd.Parameters.Add(StoreIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._store_name = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();


      SqlCommand cmd = new SqlCommand("DELETE brands FROM brands JOIN store_brand on (brands.id = store_brand.brand_id) JOIN stores on (stores.id = store_brand.store_id) WHERE stores.id = @StoreId; DELETE FROM stores WHERE id = @StoreId; DELETE FROM store_brand WHERE store_id = @StoreId", conn);

      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = this.GetId();

      cmd.Parameters.Add(StoreIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
