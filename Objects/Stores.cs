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

    public void AddBook(Book newBook)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books_stores (store_id, book_id) VALUES (@StoreId, @BookId)", conn);

      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = this.GetId();
      cmd.Parameters.Add(StoreIdParameter);

      SqlParameter booksIdParameter = new SqlParameter();
      booksIdParameter.ParameterName = "@BookId";
      booksIdParameter.Value = newBook.GetId();
      cmd.Parameters.Add(booksIdParameter);

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

    public List<Book> GetBooks()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      List<Book> books = new List<Book>{};

      SqlCommand cmd = new SqlCommand("SELECT books.* FROM stores JOIN books_stores on (stores.id = books_stores.store_id) JOIN books on (books.id = books_stores.book_id) WHERE stores.id = @StoreId", conn);

      //select from the books table
      //get your targets from the stores table
      //use the join table to bring these together
      //specifically, on the join table where the real store id is equal to the join table store_id column Value
      //then, come from the other side, the books table.  look in the join table where the real book id = the join table book_id column
      //and do all of this based on teh storeID

      //select books for the store.  first look on teh join table where the store id matches and grab all those books
      // then look on the books table where it matches the store id


      SqlParameter StoreIdParameter = new SqlParameter();
      StoreIdParameter.ParameterName = "@StoreId";
      StoreIdParameter.Value = this.GetId();

      cmd.Parameters.Add(StoreIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> booksIds = new List<int> {};
      while(rdr.Read())
      {
        int booksId = rdr.GetInt32(0);
        string booksName = rdr.GetString(1);
        bool Checked_out = rdr.GetBoolean(2);

        Book newBook = new Book(booksName, Checked_out, booksId);
        books.Add(newBook);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }


      return books;
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


      SqlCommand cmd = new SqlCommand("DELETE books FROM books JOIN books_stores on (books.id = books_stores.book_id) JOIN stores on (stores.id = books_stores.store_id) WHERE stores.id = @StoreId; DELETE FROM stores WHERE id = @StoreId; DELETE FROM books_stores WHERE store_id = @StoreId", conn);

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
