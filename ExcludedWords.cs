// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Text;

namespace DGOLibrary
{
  static class ExcludedWords
  {
  private const string OneBigString = ";about;" +
    "adview;along;also;alt;andeconomic;andthe;" +
    "anoth;arent;askthediver;aspx;assis;asthe;" +
    "bangin;bcimedia;bigstory;boardssuperintendent;" +
    "both;breake;bumpin;came;cant;cmyk;comherald;" +
    "compa;corinnef;could;couldn;couldnt;departmentof;" +
    "didn;does;doesn;doesnt;duangoherald;durangherald;" +
    "durango;durangoccl;durangoherald;durangoherlad;" +
    "durangoherld;durnagoherald;each;entr;eps;" +
    "eventsubmit;extracur;exup;fers;filelist;foaf;" +
    "foar;forthe;from;gmail;goes;going;gvexyvg;" +
    "haba;half;have;havent;having;havingsufficient;" +
    "hellip;hermannclarence;homepage;html;http;" +
    "https;iain;ignaciosuperintendent;inamerican;" +
    "incentiveto;into;isbecause;isnt;istockphoto;" +
    "itis;ject;jliff;jmac;jpeg;labdr;ldquo;lsquo;" +
    "made;marqua;mbssllp;mdash;ment;more;morp;"+
    "mtnx;navadvertisewithus;navarchives;" +
    "navaskthedivertrevorrovert;navchechitout;" +
    "navclassifiedads;navclassifieds;navcontact;" +
    "navdayinthelifespringbling;" +
    "navdurangotelegraphcalendarpolicy;" +
    "navfeaturenotyouraverageschlarpen;" +
    "navflashinthepandramaintheolivegroves;" +
    "navhaikubeforeidisappear;" +
    "navlavidalocalblastfromthepast;" +
    "navletters;navlocalnewsthelastlap;" +
    "navnews;navonlineads;navonthetown;" +
    "navopinion;navretooned;navsecondsection;" +
    "navsoapbox;navsubscriptions;navthumbinit;" +
    "navtopshelfabirthdayrantandloggerseason;" +
    "navwordonthestreet;nbsp;ndash;newsearch;" +
    "newsmemory;newsnow;nocache;ntilde;" +
    "officialcoordinating;ongoingdiscussions;" +
    "only;orbust;ouml;pdf;performancegoals;" +
    "phpadsnew;placename;placetype;ppinclude;" +
    "progid;quot;rbmve;rdquo;reservoirdylan;" +
    "retiringsuperintendent;returningfrom;" +
    "rgb;ricekw;rsquo;said;say;says;sboos;" +
    "schemas;schild;schowlater;scommission;" +
    "searchform;skyscraperad;smarttags;" +
    "smarttagtype;soapboxheder;some;" +
    "southwestcolors;stillhave;such;svad;" +
    "swcenter;swscene;syijxs;take;takes;tgivon;" +
    "than;that;thats;thatwould;thedurangoherald;" +
    "their;theirs;them;then;thepole;there;these;" +
    "theyll;theyve;theywill;thing;things;this;" +
    "those;through;throughs;thus;tif;time;" +
    "toincorporate;took;touristsran;tsked;" +
    "uniquequalities;uoregon;used;using;" +
    "valueclosing;vapolitics;very;wasnt;went;" +
    "were;westerncommunities;weve;what;whats;" +
    "when;where;which;while;will;with;wont;would;" +
    "wouldn;wouldnt;wretac;your;youre;yours;youve;";

 

  internal static bool IsExcluded( string Word )
    {
    if( Word == null ) // If it ever got a null.
      return true;

    if( Word.Length < 3 )
      return true;

    if( OneBigString.Contains( ";" + Word + ";" ))
      return true;

    for( int Count = 0; Count < Word.Length; Count++ )
      {
      if( !Utility.IsALetter( Word[Count] ))
        return true;

      }

    return false;
    }




  }
}
