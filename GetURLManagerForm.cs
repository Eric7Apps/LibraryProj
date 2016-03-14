// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace DGOLibrary
{
  public partial class GetURLManagerForm : Form
  {
  private MainForm MForm;
  private GetFromURLForm[] GetURLArray;
  private int GetURLArrayLast = 0;
  private ECTime QueueTime;
  private int TempFileNameCount = 0;


  private GetURLManagerForm()
    {
    InitializeComponent();
    }



  internal GetURLManagerForm( MainForm UseForm )
    {
    InitializeComponent();

    MForm = UseForm;
    GetURLArray = new GetFromURLForm[64];
    QueueTime = new ECTime();
    QueueTime.SetToNow();

    AddInitialLinks();

    // The queue timer is being started from the Test
    // menu for now.
    // QueueTimer.Interval = 1000;
    // QueueTimer.Start();

    NewLinksTimer.Interval = 60 * 60 * 1000;
    NewLinksTimer.Start();
    }



  private void AddInitialLinks()
    {
    // This is hard-coded for now, but it could be from a
    // list in a configuration file.

    AddURLForm( "Main Page", "http://www.durangoherald.com" );
    AddURLForm( "Local / Regional", "http://www.durangoherald.com/section/News01/" );
    AddURLForm( "National / World", "http://www.durangoherald.com/section/News03/" );
    AddURLForm( "Business", "http://www.durangoherald.com/section/News04/" );
    AddURLForm( "Columnists", "http://www.durangoherald.com//section/Columnists/" );
    AddURLForm( "Education", "http://www.durangoherald.com/section/News05/" );
    AddURLForm( "Good Earth", "http://www.durangoherald.com/section/News06/" );
    AddURLForm( "Obituaries", "http://obituaries.durangoherald.com/obituaries/durangoherald/" );
    AddURLForm( "Real Estate", "http://www.durangoherald.com/section/realestate/" );
    AddURLForm( "Sports", "http://www.durangoherald.com/section/Sports/" );
    AddURLForm( "Professtional Sports", "http://www.durangoherald.com/section/Sports01/" );
    AddURLForm( "College Sports", "http://www.durangoherald.com/section/Sports02/" );
    AddURLForm( "High School Sports", "http://www.durangoherald.com/section/Sports03/" );
    AddURLForm( "Youth Sports", "http://www.durangoherald.com/section/Sports04/" );
    AddURLForm( "Outdoors Sports", "http://www.durangoherald.com/section/Sports05/" );
    AddURLForm( "Arts and Entertainment", "http://www.durangoherald.com/section/Arts/" );
    AddURLForm( "Performing Arts", "http://www.durangoherald.com/section/Arts01/" );
    AddURLForm( "Visual Arts", "http://www.durangoherald.com/section/Arts02/" );
    AddURLForm( "Music", "http://www.durangoherald.com/section/Arts03/" );
    AddURLForm( "Film and TV", "http://www.durangoherald.com/section/Arts04/" );
    AddURLForm( "Books", "http://www.durangoherald.com/section/Arts05/" );
    AddURLForm( "Lifestyle", "http://www.durangoherald.com/section/Lifestyle/" );
    AddURLForm( "Southwest Life", "http://www.durangoherald.com/section/Lifestyle01/" );
    AddURLForm( "Food", "http://www.durangoherald.com/section/Lifestyle02/" );
    AddURLForm( "Travel", "http://www.durangoherald.com/section/Lifestyle03/" );
    AddURLForm( "Health", "http://www.durangoherald.com/section/Lifestyle04/" );
    AddURLForm( "Family", "http://www.durangoherald.com/section/Lifestyle05/" );
    AddURLForm( "Religion", "http://www.durangoherald.com/section/Lifestyle06/" );
    AddURLForm( "Opinion", "http://www.durangoherald.com/section/Opinion/" );
    AddURLForm( "Editorial", "http://www.durangoherald.com/section/Opinion01/" );
    AddURLForm( "Op Ed", "http://www.durangoherald.com/section/Opinion02/" );
    AddURLForm( "Letters to the Editor", "http://www.durangoherald.com/section/Opinion03/" );
    AddURLForm( "Newsstand", "http://www.durangoherald.com/section/newsstand/" );
    AddURLForm( "Events", "http://swscene.com/" );
    AddURLForm( "Classifieds", "http://marketplace.durangoherald.com/searchresults.aspx?p=8727\" target=\"_blank\">" );
    AddURLForm( "Gold King Mine spill", "http://marketplace.durangoherald.com/section/goldking" );
    AddURLForm( "Staff Listing", "http://www.durangoherald.com/section/contact/" );

    // /section/snowdown
    // /section/video
    // Weather:
    // "http://thecloudscout.com/?referrer=durango-herald">

    // Shop:
    // href="http://shop.fourcornersmarketplace.com/durango-co" target="_blank">

    // Webcams:
    // http://thecloudscout.com

    // Highway web cams:
    // http://i.cotrip.org

    // 4 Corners TV:
    // http://4cornerstv.com

    // http://www.pinerivertimes.com

    // http://www.the-journal.com

    // http://www.dgomag.com

    // /section/contact/">Staff Listing

    // /section/focusonbusiness/
    }



  private string GetNextTempFileName()
    {
    string TempName = MForm.GetTempFileDirectory() +
      "Temp" + TempFileNameCount.ToString() + ".txt";

    TempFileNameCount++;

    // Since this is getting one new page once every
    // three seconds (depending on your timer event
    // setting) it would not come even remotely close
    // to using all 127 unique file names at one time.

    // Bring it back to zero after 127.
    TempFileNameCount = TempFileNameCount & 0x7F;
    return TempName;
    }



  internal bool AddURLForm( string Title, string URL )
    {
    string FileName = GetNextTempFileName();

    GetFromURLForm GetForm = new GetFromURLForm( MForm, URL, FileName, Title );
    GetURLArray[GetURLArrayLast] = GetForm;
    GetURLArrayLast++;

    if( GetURLArrayLast >= GetURLArray.Length )
      {
      try
      {
      Array.Resize( ref GetURLArray, GetURLArray.Length + 64 );
      }
      catch
        {
        return false;
        }
      }

    ShowStatus( "Added to queue: " + URL );
    return true;
    }



  private void ShowStatus( string Status )
    {
    if( IsDisposed )
      return;

    if( MainTextBox.Text.Length > 10000 )
      MainTextBox.Text = "";

    MainTextBox.AppendText( Status + "\r\n" ); 
    }



  private void testToolStripMenuItem_Click(object sender, EventArgs e)
    {
    QueueTimer.Interval = 1000;
    QueueTimer.Start();
    }



  internal void FreeEverything()
    {
    QueueTimer.Stop();

    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( !GetURLArray[Count].IsDisposed )
        {
        GetURLArray[Count].Hide();
        GetURLArray[Count].FreeEverything();
        GetURLArray[Count].Dispose();
        }

      GetURLArray[Count] = null;
      }

    GetURLArrayLast = 0;
    }



  private void FreeOldForms()
    {
    try
    {
    try
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( !GetURLArray[Count].GetHasStarted())
        continue;

      /* ===========
      Leave it showing for testing.

      if( GetURLArray[Count].GetFileIsDone() ||
        GetURLArray[Count].GetHadErrorOrCancel())
        {
        // ShowStatus( "Closing finished form." );
        if( !GetURLArray[Count].IsDisposed )
          {
          GetURLArray[Count].Hide();
          GetURLArray[Count].FreeEverything();
          GetURLArray[Count].Dispose();
          }

        GetURLArray[Count] = null;
        continue;
        }
      */

      int OldTime = 60 * 3;
      if( GetURLArray[Count].GetStartTimeSecondsToNow() > OldTime )
        {
        // It's hung up or something and it shouldn't
        // take this long.
        ShowStatus( "Form timed out." );
        if( !GetURLArray[Count].IsDisposed )
          {
          GetURLArray[Count].Hide();
          GetURLArray[Count].FreeEverything();
          GetURLArray[Count].Dispose();
          }

        // Things like this should only run
        // in the main thread.
        GetURLArray[Count] = null;
        continue;
        }
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in FreeOldForms():" );
      ShowStatus( Except.Message );
      }

    int MoveTo = 0;
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count] != null )
        {
        GetURLArray[MoveTo] = GetURLArray[Count];
        MoveTo++;
        }
      }

    GetURLArrayLast = MoveTo;
    // ShowStatus( "After freeing old ones last is: " + GetURLArrayLast.ToString());

    // if( GetURLArrayLast == 0 )
      // {
      // Check for new links again this way instead of
      // using the new links timer?
      // AddInitialLinks();

      // Or just keep checking for breaking stories
      // only here?
      // AddURLForm( "Main Page", "http://www.durangoherald.com" );
      // }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception FreeOldForms() (second):" );
      ShowStatus( Except.Message );
      }
    }



  private void StartNextInQueue()
    {
    try
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count].GetHasStarted())
        continue;

      // Start up the first one that hasn't already been started.
      // ShowStatus( "Starting next in queue." );
      GetURLArray[Count].StartHttp( true );
      return; // Only start one.
      }

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in StartNextInQueue():" );
      ShowStatus( Except.Message );
      }
    }



  private void QueueTimer_Tick(object sender, EventArgs e)
    {
    try
    {
    if( MForm.GetIsClosing())
      return;

    FreeOldForms();
 
    // Play nice and don't send requests too often.
    if( QueueTime.GetSecondsToNow() > 3.0 )
      {
      // ShowStatus( "Timer for next in queue." );
      StartNextInQueue();
      QueueTime.SetToNow();
      }

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in QueueTimer_Tick():" );
      ShowStatus( Except.Message );
      }
    }



  private void NewLinksTimer_Tick(object sender, EventArgs e)
    {
    // Check for new links again once every so often.
    AddInitialLinks();
    }


  }
}


