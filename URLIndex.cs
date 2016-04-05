// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Text;


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
    FileName = MForm.GetDataDirectory() + "PageIndex.txt";
    }


  private int GetRandomishIndex( string URL )
    {
    // Check: URLsRecArrayLength

    // Get a basic randomish index.
    // A quick and easy (for now) hash function.
    int Sum = 0;
    int ExclusiveOR = 0;
    for( int Count = 0; Count < URL.Length; Count++ )
      {
      // A sum would be the same as Exclusive OR
      // except that it has a carry bit.
      Sum += (int)URL[Count];

      // No carry bit.
      ExclusiveOR = ExclusiveOR ^ (int)URL[Count];
      } 

    int Index = Sum & 0xFF;
    Index <<= 8;
    ExclusiveOR = ExclusiveOR | Sum;
    Index |= ExclusiveOR & 0xFF;
    return Index; // A 16 bit number.
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

    string URLKey = URL.ToLower();
    int Index = GetRandomishIndex( URLKey );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      return false;

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



  internal void AddURL( string URL, Page PageToAdd )
    {
    try
    {
    string Test = GetExistingURL( URL );
    if( Test.Length > 0 )
      return;

    URLIndexRec Rec = new URLIndexRec();
    Rec.URL = URL;
    Rec.PageForURL = PageToAdd; // Might be null.

    URLStringsArray[URLStringsArrayLast] = URL;
    Rec.Index = URLStringsArrayLast;
    URLStringsArrayLast++;
    if( URLStringsArrayLast >= URLStringsArray.Length )
      Array.Resize( ref URLStringsArray, URLStringsArray.Length + 1024 );

    int Index = GetRandomishIndex( Rec.URL );

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



   /*
  // Keep this method private to this object.
  // LinkTag has something that does this too.
  private bool IsBadLinkToAdd( string URL )
    {
    URL = URL.ToLower();
    if( URL.Contains( "/frontpage/" ))
      return true;

    return false;
    }
    */


  internal int GetIndex( string URL )
    {
    try
    {
    string FixedURL = GetExistingURL( URL );
    if( FixedURL.Length == 0 )
      return -1;

    string URLKey = FixedURL.ToLower();
    int Index = GetRandomishIndex( URLKey );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      return -1;

    int Last = URLsRecArray[Index].URLIndexArrayLast;
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



  }
}

