// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Text;
using System.IO;


namespace DGOLibrary
{
  class URLIndex
  {
  private MainForm MForm;
  private URLsRec[] URLsRecArray;
  private const int URLsRecArrayLength = 0xFFFF + 1;
  private string[] URLStringsArray;
  private int URLStringsArrayLast = 0;
  private string FileName = "";


  // Test this against PageList.


  public struct URLsRec
    {
    public URLIndexRec[] URLIndexArray;
    public int URLIndexArrayLast;
    }


  public struct URLIndexRec
    {
    public string URL;
    public int Index;
    public Page PageForURL;
    }


  private URLIndex()
    {
    }


  internal URLIndex( MainForm UseForm )
    {
    MForm = UseForm;
    URLsRecArray = new URLsRec[URLsRecArrayLength];
    URLStringsArray = new string[1024];
    FileName = MForm.GetDataDirectory() + "URLIndex.txt";
    }


  private int GetRandomishIndex2( string URL )
    {
    string URLKey = URL.ToLower();

    // Check: URLsRecArrayLength for the size of
    // this index.

    // Get a basic randomish index.
    // A quick and easy (for now) hash function.
    int Sum = 0;
    int ExclusiveOR = 0;
    for( int Count = 0; Count < URLKey.Length; Count++ )
      {
      // A sum would be the same as Exclusive OR
      // except that it has a carry bit.
      Sum += (int)URLKey[Count];

      // No carry bit.
      ExclusiveOR = ExclusiveOR ^ (int)URLKey[Count];
      } 

    int Index = Sum & 0xFF;
    Index <<= 8;
    ExclusiveOR = ExclusiveOR | Sum;
    Index |= ExclusiveOR & 0xFF;
    return Index; // A 16 bit number.
    }



  internal Page GetPage( string URL )
    {
    try
    {
    if( URL == null )
      return null;

    if( URL.Length < 5 )
      return null;

    string ExistingURL = GetExistingURL( URL );
    if( ExistingURL.Length == 0 )
      {
      MForm.ShowStatus( "There is no existing URL for GetPage()." );
      MForm.ShowStatus( ExistingURL );
      return null;
      }

    int Index = GetRandomishIndex2( ExistingURL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      {
      MForm.ShowStatus( "The URLIndexArray was null for:" );
      MForm.ShowStatus( ExistingURL );
      return null;
      }

    string URLKey = ExistingURL.ToLower();
    int Last = URLsRecArray[Index].URLIndexArrayLast;
    for( int Count = 0; Count < Last; Count++ )
      {
      if( URLKey == URLsRecArray[Index].URLIndexArray[Count].URL.ToLower() )
        {
        Page Result =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
        if( Result == null )
          {
          Result = new Page( MForm );
          URLsRecArray[Index].URLIndexArray[Count].PageForURL = Result;
          }

        return Result;
        }
      }

    MForm.ShowStatus( "Didn't find a matching URL for:" );
    MForm.ShowStatus( ExistingURL );
    return null;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetExistingPage():" );
      MForm.ShowStatus( Except.Message );
      return null;
      }
    }



  internal string GetExistingURL( string URL )
    {
    try
    {
    if( URL == null )
      return "";

    if( URL.Length < 5 )
      return "";

    if( ExactURLExists( URL ))
      return URL;

    if( ExactURLExists( URL + "/" ))
      return URL + "/";

    string TestURL = URL;
    if( URL.EndsWith( "/" ))
      {
      TestURL = Utility.TruncateString( URL, URL.Length - 1 );
      if( ExactURLExists( TestURL ))
        return TestURL;

      }

    // There are duplicate links because they fall
    // under multiple categories or because they are
    // just messed up and duplicated all over the place.

    // TitlesDictionary

    // Durango track and field team strong in Farmington
    // /20160320/SPORTS03/160329972/0/Sports/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports01/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports02/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports03/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports04/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports05/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/-1/Sports
    // /20160320/SPORTS03/160329972/-1/Sports03/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/Durango-track-and-field-team-strong-in-Farmington

    // http://www.durangoherald.com/article/

    // 4-H garden is gateway to lessons about health
    // /20160316/COLUMNISTS05/160319649/-1/Columnists
    // /20160316/COLUMNISTS05/160319649/-1/Lifestyle
    // /20160316/COLUMNISTS05/160319649/-1/Lifestyle01
    // /20160316/COLUMNISTS05/160319649/-1/Lifestyle04
    // /20160316/COLUMNISTS05/160319649/-1/News06

    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetExistingURL():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  private bool ExactURLExists( string URL )
    {
    try
    {
    if( URL == null )
      return false;

    if( URL.Length < 5 )
      return false;

    int Index = GetRandomishIndex2( URL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      return false;

    string URLKey = URL.ToLower();
    int Last = URLsRecArray[Index].URLIndexArrayLast;
    for( int Count = 0; Count < Last; Count++ )
      {
      if( URLKey == URLsRecArray[Index].URLIndexArray[Count].URL.ToLower() )
        return true;

      }

    return false;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in ExactURLExists():" );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  internal void AddEmptyPage( string Title, string URL, string RelativeURLBase )
    {
    try
    {
    if( "" != GetExistingURL( URL ))
      return;

    if( !LinkIsGood( URL ))
      return;

    Page PageToAdd = new Page( MForm );
    PageToAdd.SetNewTitleAndURL( Title, URL, RelativeURLBase );
    AddURL( URL, PageToAdd );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddEmptyPage():" );
      MForm.ShowStatus( Except.Message );
      }
    }


  internal void AddURL( string URL, Page PageToAdd )
    {
    try
    {
    string Test = GetExistingURL( URL );
    if( Test.Length > 0 )
      {
      // MForm.ShowStatus( "The URL is already there in AddURL()." );
      return;
      }

    URLIndexRec Rec = new URLIndexRec();
    Rec.URL = URL;
    Rec.PageForURL = PageToAdd; // Might be null.

    URLStringsArray[URLStringsArrayLast] = URL;
    Rec.Index = URLStringsArrayLast;
    URLStringsArrayLast++;
    if( URLStringsArrayLast >= URLStringsArray.Length )
      Array.Resize( ref URLStringsArray, URLStringsArray.Length + 1024 );

    int Index = GetRandomishIndex2( Rec.URL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      URLsRecArray[Index].URLIndexArray = new URLIndexRec[64];

    URLsRecArray[Index].URLIndexArray[URLsRecArray[Index].URLIndexArrayLast] = Rec;
    URLsRecArray[Index].URLIndexArrayLast++;
    if( URLsRecArray[Index].URLIndexArrayLast >= URLsRecArray[Index].URLIndexArray.Length )
      Array.Resize( ref URLsRecArray[Index].URLIndexArray, URLsRecArray[Index].URLIndexArray.Length + 64 );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddURL():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  // Keep this method private to this object.
  // LinkTag has something that does this too.
  private bool LinkIsGood( string URL )
    {
    URL = URL.ToLower();

    if( URL.Contains( "/javascript/" ))
      return false;

    if( URL.Contains( "/frontpage/" ))
      return false;

    if( URL.Contains( "/msxml2.xmlhttp/" ))
      return false;

    if( URL.Contains( "/rss/" ))
       return false;

    if( URL.Contains( "/taxonomy/" ))
       return false;

    return true;
    }



  internal int GetIndex( string URL )
    {
    try
    {
    string FixedURL = GetExistingURL( URL );
    if( FixedURL.Length == 0 )
      return -1;

    int Index = GetRandomishIndex2( FixedURL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      return -1;

    int Last = URLsRecArray[Index].URLIndexArrayLast;
    string URLKey = FixedURL.ToLower();
    for( int Count = 0; Count < Last; Count++ )
      {
      if( URLKey == URLsRecArray[Index].URLIndexArray[Count].URL.ToLower() )
        return URLsRecArray[Index].URLIndexArray[Count].Index;

      }

    return -1;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetIndex():" );
      MForm.ShowStatus( Except.Message );
      return -1;
      }
    }



  internal void UpdatePageFromFile( string Title, string URL, string FileName, bool SetTime, string RelativeURLBase, bool ReadFromFile )
    {
    try
    {
    if( URL == null )
      return;

    if( Title == null )
      return;

    if( !LinkIsGood( URL ))
      return;

    if( URL.Length < 10 )
      {
      MForm.ShowStatus( "URL is too short in UpdatePageFromTempFile()." );
      return;
      }

    // "Main Page"
    if( Title.Length < 3 )
      {
      MForm.ShowStatus( "Title is too short in UpdatePageFromTempFile()." );
      MForm.ShowStatus( "Title: " + Title );
      return;
      }

    string ExistingURL = GetExistingURL( URL );
    if( ExistingURL.Length == 0 )
      {
      AddURL( URL, null );
      ExistingURL = URL;
      }

    Page UsePage = GetPage( ExistingURL );
    if( UsePage == null )
      {
      MForm.ShowStatus( "There was an error getting the page for:" );
      MForm.ShowStatus( ExistingURL );
      return;
      }

    // Notice that if the URL aleady exists then
    // the contents of that page are updated but
    // the old contents are not saved.  So for example
    // the main index page at www.durangoherald.com
    // is updated but the old one is not saved.

    // When this page gets parsed it will refer back
    // to this URLIndex to get the indexes for every
    // URL in the links it is adding.  So while this
    // is being called, new URLs are being added.
    UsePage.UpdateFromFile( Title, ExistingURL, FileName, SetTime, RelativeURLBase, ReadFromFile );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.UpdatePageFromFile():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    int Written = 0;
    using( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
      {
      for( int Index = 0; Index < URLsRecArrayLength; Index++ )
        {
        if( URLsRecArray[Index].URLIndexArray ==  null )
          continue;

        int Last = URLsRecArray[Index].URLIndexArrayLast;
        for( int Count = 0; Count < Last; Count++ )
          {
          Page OnePage =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
          if( OnePage == null )
            continue;

          Written++;
          // string Line = Index.ToString() + ") " + OnePage.ObjectToString();
          string Line = OnePage.ObjectToString();
          SWriter.WriteLine( Line );
          }
        }
      }

    MForm.ShowStatus( "URLIndex wrote " + Written.ToString( "N0" ) + " page objects to the file." );
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.WriteToTextFile():" );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  }
}


