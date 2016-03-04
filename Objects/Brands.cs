using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ShoeStoreNameSpace
{
  public class Brand
  {
    private int _id;
    private string _brand_name;


    public Brand(string brandName, int Id = 0)
    {
      _id = Id;
      _brand_name = brandName;
    }
    public override bool Equals(System.Object otherBrand)
    {
        if (!(otherBrand is Brand))
        {
          return false;
        }
        else {
          Brand newBrand = (Brand) otherBrand;
          bool idEquality = this.GetId() == newBrand.GetId();
          bool titleEquality = this.GetTitle() == newBrand.GetTitle();
          return (idEquality && titleEquality);
        }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
      return _brand_name;
    }


    public static List<Brand> GetAll()
    {
      List<Brand> AllBrands = new List<Brand>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int brandId = rdr.GetInt32(0);
        string brandTitle = rdr.GetString(1);
        Brand newBrand = new Brand(brandTitle, brandId);
        AllBrands.Add(newBrand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllBrands;
    }

    public DateTime GetDate()
    {
      DateTime Date = new DateTime(2015, 1, 1);

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT due_date FROM brands_patrons WHERE id = @patronID", conn);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@patronID";
      patronIdParameter.Value = this.GetId();
      cmd.Parameters.Add(patronIdParameter);


      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        Date = rdr.GetDateTime(0);
        // int brandId = rdr.GetInt32(0);
        // string brandTitle = rdr.GetString(1);
        // bool Checked_out = rdr.GetBoolean(2);
        // Brand newBrand = new Brand(brandTitle, Checked_out, brandId);
        // AllBrands.Add(newBrand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      // string stringDate = date.ToString();
      return Date;
    }


    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO brands (brand_name) OUTPUT INSERTED.id VALUES (@BrandTitle)", conn);

      SqlParameter titleParameter = new SqlParameter();
      titleParameter.ParameterName = "@BrandTitle";
      titleParameter.Value = this.GetTitle();


      cmd.Parameters.Add(titleParameter);


      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
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

    // public List<Stores> GetStores()
    // {
    //   SqlConnection conn = DB.Connection();
    //   SqlDataReader rdr = null;
    //   conn.Open();
    //
    //   List<Store> stores = new List<Store>{};
    //
    //   SqlCommand cmd = new SqlCommand("SELECT stores.* from brands join store_brand on (brands.id = store_brand.brand_id) join stores on (store_brand.stores_id = stores.id) where brands.id = @BrandId;", conn);
    //
    //   SqlParameter brandIdParameter = new SqlParameter();
    //   brandIdParameter.ParameterName = "@BrandId";
    //   brandIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(brandIdParameter);
    //
    //   rdr = cmd.ExecuteReader();
    //
    //   while (rdr.Read())
    //   {
    //     int storesId = rdr.GetInt32(0);
    //     string storesTitle = rdr.GetString(1);
    //     Stores newStores = new Stores(storesTitle, storesId);
    //     stores.Add(newStores);
    //   }
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return stores;
    // }

    // public void Update(string newBrand_Title)
    // {
    //   SqlConnection conn = DB.Connection();
    //   SqlDataReader rdr;
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("UPDATE brands SET title = @Title OUTPUT INSERTED.title WHERE id = @BrandId;", conn);
    //
    //   SqlParameter TitleParameter = new SqlParameter();
    //   TitleParameter.ParameterName = "@Title";
    //   TitleParameter.Value = newBrand_Title;
    //
    //   SqlParameter BrandIdParameter = new SqlParameter();
    //   BrandIdParameter.ParameterName = "@BrandId";
    //   BrandIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(TitleParameter);
    //   cmd.Parameters.Add(BrandIdParameter);
    //
    //   rdr = cmd.ExecuteReader();
    //
    //   while(rdr.Read())
    //   {
    //     this._title = rdr.GetString(0);
    //   }
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }



    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM brands;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Brand Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands WHERE id = @BrandId", conn);
      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = id.ToString();

      cmd.Parameters.Add(brandIdParameter);
      rdr = cmd.ExecuteReader();

      int foundBrandId = 0;
      string foundBrandTitle = null;


      while(rdr.Read())
      {
        foundBrandId = rdr.GetInt32(0);
        foundBrandTitle = rdr.GetString(1);
      }
      Brand foundBrand = new Brand(foundBrandTitle, foundBrandId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBrand;
    }

    // public static Brand FindTitle(string title)
    // {
    //   SqlConnection conn = DB.Connection();
    //   SqlDataReader rdr = null;
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM brands WHERE title = @Brandtitle", conn);
    //   SqlParameter brandIdParameter = new SqlParameter();
    //   brandIdParameter.ParameterName = "@Brandtitle";
    //   brandIdParameter.Value = title.ToString();
    //
    //   cmd.Parameters.Add(brandIdParameter);
    //   rdr = cmd.ExecuteReader();
    //
    //   int foundBrandId = 0;
    //   string foundBrandTitle = null;
    //
    //   while(rdr.Read())
    //   {
    //     foundBrandId = rdr.GetInt32(0);
    //     foundBrandTitle = rdr.GetString(1);
    //   }
    //   Brand foundBrand = new Brand(foundBrandTitle, foundBrandId);
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return foundBrand;
    // }

    public void AddStore(Store newStores)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO brand_store (stores_id, brand_id) VALUES (@Stores_id, @Brand_id);", conn);

      SqlParameter storesIdParameter = new SqlParameter();
      storesIdParameter.ParameterName = "@Stores_id";
      storesIdParameter.Value = newStores.GetId();
      cmd.Parameters.Add(storesIdParameter);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@Brand_id";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    // public void Delete()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("DELETE FROM brands WHERE id = @Brand_id; DELETE FROM brands_storess WHERE brand_id = @Brand_id;", conn);
    //
    //   SqlParameter brandIdParameter = new SqlParameter();
    //   brandIdParameter.ParameterName = "@Brand_id";
    //   brandIdParameter.Value = this.GetId();
    //
    //   cmd.Parameters.Add(brandIdParameter);
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
  }
}
