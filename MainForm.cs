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
using System.Diagnostics; // For start program process.
using System.IO;
using System.Threading;



namespace DGOLibrary
{
  public partial class MainForm : Form
  {
  internal const string VersionDate = "3/29/2016";
  internal const int VersionNumber = 09; // 0.9
  internal const string MessageBoxTitle = "Library Project";
  private System.Threading.Mutex SingleInstanceMutex = null;
  private bool IsSingleInstance = false;
  internal GetURLManagerForm GetURLMgrForm;
  internal WebListenerForm WebListenForm;
  internal WebFilesData WebFData;
  private string TempFileDirectory = "";
  private string PagesDirectory = "";
  private string DataDirectory = "";
  private string WebPagesDirectory = "";
  private bool IsClosing = false;
  private bool Cancelled = false;
  internal GlobalProperties GlobalProps;
  internal PageList PageList1;
  internal Words AllWords;
  internal WordsDictionary WordsDictionary1;
  internal ScriptDictionary ScriptDictionary1;
  internal CodeCommentDictionary CodeCommentDictionary1;
  internal NetIPStatus NetStats;




  public MainForm()
    {
    InitializeComponent();

    ///////////////////////
    // Keep this at the top:
    SetupDirectories();
    GlobalProps = new GlobalProperties( this );
    ///////////////////////

    NetStats = new NetIPStatus( this );
    NetStats.ReadFromFile();

    WordsDictionary1 = new WordsDictionary( this );
    ScriptDictionary1 = new ScriptDictionary( this );
    CodeCommentDictionary1 = new CodeCommentDictionary( this );
    PageList1 = new PageList( this );
    AllWords = new Words( this );
    WebFData = new WebFilesData( this );

    if( !CheckSingleInstance())
      return;

    IsSingleInstance = true;

    StartupTimer.Interval = 200;
    StartupTimer.Start();
    }



  internal void ShowStatus( string Status )
    {
    if( IsClosing )
      return;

    // Commented out for testing but it would be
    // needed on a running server.
    // if( MainTextBox.Text.Length > (80 * 5000))
      // MainTextBox.Text = "";

    MainTextBox.AppendText( Status + "\r\n" ); 
    }



  private void testToolStripMenuItem_Click(object sender, EventArgs e)
    {
    // Herald page:
    // string URL = "http://www.durangoherald.com/";
    string FileName = GetPagesDirectory() + "TestFile.txt";
    // PageList1.UpdatePageFromFile( "Main Page", URL, FileName, true, "http://www.durangoherald.com" );

    // Durango Gov:
    // string URL = "http://www.durangogov.org/";
    // PageList1.UpdatePageFromFile( "Durango Gov Main Page", URL, FileName, true, "http://www.durangogov.org" );

    // Colorado Gov:
    string URL = "https://www.colorado.gov/";
    PageList1.UpdatePageFromFile( "Colorado Gov Main Page", URL, FileName, true, "https://www.colorado.gov" );
    }



  private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
    string ShowS = "Programming by Eric Chauvin. Version date: " + VersionDate;
    MessageBox.Show( ShowS, MessageBoxTitle, MessageBoxButtons.OK );
    }



  private void SetupDirectories()
    {
    try
    {
    TempFileDirectory ="c:\\DGOLibProject\\TempFiles\\";
    // PagesDirectory = Application.StartupPath + "\\Pages\\";
    PagesDirectory = "c:\\DGOLibProject\\Pages\\";
    DataDirectory = Application.StartupPath + "\\Data\\";
    WebPagesDirectory = Application.StartupPath + "\\WebPages\\";

    if( !Directory.Exists( TempFileDirectory ))
      Directory.CreateDirectory( TempFileDirectory );

    if( !Directory.Exists( PagesDirectory ))
      Directory.CreateDirectory( PagesDirectory );

    if( !Directory.Exists( WebPagesDirectory ))
      Directory.CreateDirectory( WebPagesDirectory );

    if( !Directory.Exists( DataDirectory ))
      Directory.CreateDirectory( DataDirectory );

    }
    catch( Exception )
      {
      MessageBox.Show( "Error: The directory could not be created.", MessageBoxTitle, MessageBoxButtons.OK );
      return;
      }
    }


  internal string GetTempFileDirectory()
    {
    return TempFileDirectory;
    }


  internal string GetPagesDirectory()
    {
    return PagesDirectory;
    }


  internal string GetWebPagesDirectory()
    {
    return WebPagesDirectory;
    }


  internal string GetDataDirectory()
    {
    return DataDirectory;
    }


  internal bool GetIsClosing()
    {
    return IsClosing;
    }



  internal void SetCancelled()
    {
    Cancelled = true;
    }


  internal bool GetCancelled()
    {
    return Cancelled;
    }


  internal bool CheckEvents()
    {
    if( IsClosing )
      return false;

    Application.DoEvents();
    if( Cancelled )
      return false;

    return true;
    }



  // This has to be added in the Program.cs file.
  internal static void UIThreadException( object sender, ThreadExceptionEventArgs t )
    {
    string ErrorString = t.Exception.Message;

    try
      {
      string ShowString = "There was an unexpected error:\r\n\r\n" +
             "The program will close now.\r\n\r\n" +
             ErrorString;

      MessageBox.Show( ShowString, "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Stop );
      }
    finally
      {
      Application.Exit();
      }
    }



  private bool CheckSingleInstance()
    {
    bool InitialOwner = false; // Owner for single instance check.

    string ShowS = "Another instance of the Durango Library Project Server is already running." +
      " This instance will close.";

    try
    {
    SingleInstanceMutex = new System.Threading.Mutex( true, "Durango Library Project Single Instance", out InitialOwner );
    }
    catch
      {
      MessageBox.Show( ShowS, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      // mutex.Close();
      // mutex = null;

      // Can't do this here:
      // Application.Exit();
      SingleInstanceTimer.Interval = 50;
      SingleInstanceTimer.Start();
      return false;
      }

    if( !InitialOwner )
      {
      MessageBox.Show( ShowS, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      // Application.Exit();
      SingleInstanceTimer.Interval = 50;
      SingleInstanceTimer.Start();
      return false;
      }

    return true;
    }



  private void SingleInstanceTimer_Tick(object sender, EventArgs e)
    {
    SingleInstanceTimer.Stop();
    Application.Exit();
    }



  private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    if( IsSingleInstance )
      {
      if( DialogResult.Yes != MessageBox.Show( "Close the program?", MessageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question ))
        {
        e.Cancel = true;
        return;
        }
      }

    IsClosing = true;
    CheckTimer.Stop();

    NetStats.SaveToFile();

    if( GetURLMgrForm != null )
      {
      if( !GetURLMgrForm.IsDisposed )
        {
        GetURLMgrForm.Hide();

        GetURLMgrForm.FreeEverything();
        GetURLMgrForm.Dispose();
        }

      GetURLMgrForm = null;
      }

    if( WebListenForm != null )
      {
      if( !WebListenForm.IsDisposed )
        {
        WebListenForm.Hide();
        
        // This could take a while since it's closing connections:
        WebListenForm.FreeEverythingAndStopServer();
        WebListenForm.Dispose();
        }

      WebListenForm = null;
      }

    PageList1.WriteToTextFile();
    AllWords.WriteToTextFile();
    // WordsDictionary1.WriteToTextFile();
    ScriptDictionary1.WriteToTextFile();
    CodeCommentDictionary1.WriteToTextFile();
    // After getting what those others show.
    SaveStatusToFile();
    }


  internal void ShowWebListenerFormStatus( string Status )
    {
    if( IsClosing )
      return;

    if( WebListenForm == null )
      return;
      
    if( WebListenForm.IsDisposed )
      return;

    WebListenForm.ShowStatus( Status ); 
    }



  internal void SaveStatusToFile()
    {
    try
    {
    string FileName = GetDataDirectory() + "MainStatus.txt";

    using( StreamWriter SWriter = new StreamWriter( FileName  )) 
      {
      foreach( string Line in MainTextBox.Lines )
        {
        SWriter.WriteLine( Line );
        }
      }

    // MForm.StartProgramOrFile( FileName );

    }
    catch( Exception Except )
      {
      ShowStatus( "Error: Could not write the status to the file." );
      ShowStatus( Except.Message );
      return;
      }
    }



  // Be careful about what you execute from the server.
  internal bool StartProgramOrFile( string FileName )
    {
    if( !File.Exists( FileName ))
      return false;

    Process ProgProcess = new Process();

    try
    {
    ProgProcess.StartInfo.FileName = FileName;
    ProgProcess.StartInfo.Verb = ""; // "Print";
    ProgProcess.StartInfo.CreateNoWindow = false;
    ProgProcess.StartInfo.ErrorDialog = false;
    ProgProcess.Start();
    }
    catch( Exception Except )
      {
      MessageBox.Show( "Could not start the file: \r\n" + FileName + "\r\n\r\nThe error was:\r\n" + Except.Message, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      return false;
      }

    return true;
    }



  private void showGetURLManagerToolStripMenuItem_Click(object sender, EventArgs e)
    {
    if( GetURLMgrForm == null )
      return;

    if( GetURLMgrForm.IsDisposed )
      return;

    GetURLMgrForm.Show();
    GetURLMgrForm.WindowState = FormWindowState.Normal;
    GetURLMgrForm.BringToFront();
    }



  private void StartupTimer_Tick(object sender, EventArgs e)
    {
    StartupTimer.Stop();

    ShowStatus( "Reading data..." );

    // Make sure the PageList is loaded up before
    // GetURLMgrForm is started.
    PageList1.ReadFromTextFile();

    ScriptDictionary1.ReadFromTextFile();
    CodeCommentDictionary1.ReadFromTextFile();
    WordsDictionary1.ReadFromTextFile();
    // Rewrite it so it's sorted and unique.
    WordsDictionary1.WriteToTextFile();

    AllWords.ReadFromTextFile();

    GetURLMgrForm = new GetURLManagerForm( this );

    WebListenForm = new WebListenerForm( this );
    ReadWebFileData();
    ShowStatus( "Finished reading web files." );

    WebListenForm.StartServer();
    // TLSListenForm.StartServer();
    ShowStatus( "After the servers were started." );

    /////////////////
    // Make this last.
    // This calls CheckEvents().
    // PageList1.ReadAllFilesToContent();

    CheckTimer.Interval = 15 * 60 * 1000;
    CheckTimer.Start();
    }


  internal void ReadWebFileData()
    {
    ShowStatus( "Starting to read web file data." );
    WebFData.SearchWebPagesDirectory();
    ShowStatus( "Finished reading web file data." );
    }


  private void CheckTimer_Tick(object sender, EventArgs e)
    {
    NetStats.SaveToFile();
    // ShowStatus( "Saving data files." );
    PageList1.WriteToTextFile();
    // WordsDictionary1.WriteToTextFile();
    AllWords.WriteToTextFile();
    ScriptDictionary1.WriteToTextFile();
    CodeCommentDictionary1.WriteToTextFile();
    // ShowStatus( "Finished saving data files." );
    }



  private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
    if( e.KeyCode == Keys.Escape ) //  && (e.Alt || e.Control || e.Shift))
      {
      ShowStatus( "Cancelled." );
      Cancelled = true;
      // FreeEverything(); // Stop background process.
      }
    }



  private void MakeNonAsciiCharacters()
    {
    // for( int Count = 0xa0; Count < 0xe0; Count++ )
      // ShowStatus( Count.ToString( "X2" ) + ") " + Char.ToString( (char)Count ));

     // &#147;
    int GetHexValFromDecimal = 147;
    ShowStatus( "Character: " + Char.ToString( (char)GetHexValFromDecimal ));

    // ShowStatus( " " );
    // ShowStatus( "&#xad; " + Char.ToString( (char)0xad ));
    }



  private void showUnicodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
    MakeNonAsciiCharacters();
    }



  private void showTitlesToolStripMenuItem_Click(object sender, EventArgs e)
    {
    // PageList1.ShowTitles();
    }



  private void indexAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
    PageList1.IndexAll();
    }



  private void SaveAllMidnightFiles()
    {
    if( WebListenForm != null )
      {
      if( !WebListenForm.IsDisposed )
        {
        WebListenForm.ClearDailyHackCount();
        }
      }

    NetStats.ClearMidnightValues();
    }



  private void showWebServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
    if( WebListenForm == null )
      return;

    if( WebListenForm.IsDisposed )
      return;

    WebListenForm.Show();
    WebListenForm.WindowState = FormWindowState.Normal;
    WebListenForm.BringToFront();
    }



  }
}

