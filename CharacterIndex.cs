// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com



using System;
using System.Text;


namespace DGOLibrary
{
  class CharacterIndex
  {
  private MainForm MForm;
  private char[] CharacterArray;
  private const int CharacterArrayLength = 256;



  private CharacterIndex()
    {
    }


  internal CharacterIndex( MainForm UseForm )
    {
    MForm = UseForm;
    CharacterArray = new char[CharacterArrayLength];
    SetupArrays();
    }



  private void SetupArrays()
    {
    try
    {
    // See Utility.GetCleanUnicodeString().

    // These have the same values they would in Unicode
    // or ASCII unless they take the place of control
    // characters.

    // Basic Multilingual Plane
    // C0 Controls and Basic Latin (0000–007F)
    // C1 Controls and Latin-1 Supplement (0080–00FF)

    // Replacing control characters with these:
    CharacterArray[0] = '✀';  // NoCharacter or null.

    // Markers / Delimiters.
    CharacterArray[1] = '➀';
    CharacterArray[2] = '➁';
    CharacterArray[3] = '➂';
    CharacterArray[4] = '➃';
    CharacterArray[5] = '➄';
    CharacterArray[6] = '➅';
    CharacterArray[7] = '➆';
    CharacterArray[8] = '➇';
    CharacterArray[9] = '➈';
    CharacterArray[10] = '➉';
    CharacterArray[11] = '➊';
    CharacterArray[12] = '➋';

    // Carriage Return.
    CharacterArray[13] = '\r';


    CharacterArray[14] = '➌';
    CharacterArray[15] = '➍';
    CharacterArray[16] = '➎';
    CharacterArray[17] = '➏';
    CharacterArray[18] = '➐';
    CharacterArray[19] = '➑';
    CharacterArray[20] = '➒';
    CharacterArray[21] = '➓';


    // Characters from the data.
    CharacterArray[22] = 'œ'; //   0x0153
    CharacterArray[23] = 'š'; //   0x0161
    CharacterArray[24] = 'ƒ'; //   0x0192
    CharacterArray[25] = '˜'; //   0x02DC
    CharacterArray[26] = '–'; //   0x2013
    CharacterArray[27] = '—'; //   0x2014
    CharacterArray[28] = '•'; //   0x2022
    CharacterArray[29] = '…'; //   0x2026
    CharacterArray[30] = '€'; //   0x20AC
    CharacterArray[31] = '™'; //   0x2122

    // ASCII:
    CharacterArray[32] = ' ';  //  0x20
    CharacterArray[33] = '!';  //  0x21
    CharacterArray[34] = '"';  //  0x22
    CharacterArray[35] = '#';  //  0x23
    CharacterArray[36] = '$';  //  0x24
    CharacterArray[37] = '%';  //  0x25
    CharacterArray[38] = '&';  //  0x26
    CharacterArray[39] = '\'';  //  0x27
    CharacterArray[40] = '(';  //  0x28
    CharacterArray[41] = ')';  //  0x29
    CharacterArray[42] = '*';  //  0x2A
    CharacterArray[43] = '+';  //  0x2B
    CharacterArray[44] = ',';  //  0x2C
    CharacterArray[45] = '-';  //  0x2D
    CharacterArray[46] = '.';  //  0x2E
    CharacterArray[47] = '/';  //  0x2F
    CharacterArray[48] = '0';  //  0x30
    CharacterArray[49] = '1';  //  0x31
    CharacterArray[50] = '2';  //  0x32
    CharacterArray[51] = '3';  //  0x33
    CharacterArray[52] = '4';  //  0x34
    CharacterArray[53] = '5';  //  0x35
    CharacterArray[54] = '6';  //  0x36
    CharacterArray[55] = '7';  //  0x37
    CharacterArray[56] = '8';  //  0x38
    CharacterArray[57] = '9';  //  0x39
    CharacterArray[58] = ':';  //  0x3A
    CharacterArray[59] = ';';  //  0x3B
    CharacterArray[60] = '<';  //  0x3C
    CharacterArray[61] = '=';  //  0x3D
    CharacterArray[62] = '>';  //  0x3E
    CharacterArray[63] = '?';  //  0x3F
    CharacterArray[64] = '@';  //  0x40
    CharacterArray[65] = 'A';  //  0x41
    CharacterArray[66] = 'B';  //  0x42
    CharacterArray[67] = 'C';  //  0x43
    CharacterArray[68] = 'D';  //  0x44
    CharacterArray[69] = 'E';  //  0x45
    CharacterArray[70] = 'F';  //  0x46
    CharacterArray[71] = 'G';  //  0x47
    CharacterArray[72] = 'H';  //  0x48
    CharacterArray[73] = 'I';  //  0x49
    CharacterArray[74] = 'J';  //  0x4A
    CharacterArray[75] = 'K';  //  0x4B
    CharacterArray[76] = 'L';  //  0x4C
    CharacterArray[77] = 'M';  //  0x4D
    CharacterArray[78] = 'N';  //  0x4E
    CharacterArray[79] = 'O';  //  0x4F
    CharacterArray[80] = 'P';  //  0x50
    CharacterArray[81] = 'Q';  //  0x51
    CharacterArray[82] = 'R';  //  0x52
    CharacterArray[83] = 'S';  //  0x53
    CharacterArray[84] = 'T';  //  0x54
    CharacterArray[85] = 'U';  //  0x55
    CharacterArray[86] = 'V';  //  0x56
    CharacterArray[87] = 'W';  //  0x57
    CharacterArray[88] = 'X';  //  0x58
    CharacterArray[89] = 'Y';  //  0x59
    CharacterArray[90] = 'Z';  //  0x5A
    CharacterArray[91] = '[';  //  0x5B
    CharacterArray[92] = '\\';  //  0x5C
    CharacterArray[93] = ']';  //  0x5D
    CharacterArray[94] = '^';  //  0x5E
    CharacterArray[95] = '_';  //  0x5F
    CharacterArray[96] = '`';  //  0x60
    CharacterArray[97] = 'a';  //  0x61
    CharacterArray[98] = 'b';  //  0x62
    CharacterArray[99] = 'c';  //  0x63
    CharacterArray[100] = 'd';  //  0x64
    CharacterArray[101] = 'e';  //  0x65
    CharacterArray[102] = 'f';  //  0x66
    CharacterArray[103] = 'g';  //  0x67
    CharacterArray[104] = 'h';  //  0x68
    CharacterArray[105] = 'i';  //  0x69
    CharacterArray[106] = 'j';  //  0x6A
    CharacterArray[107] = 'k';  //  0x6B
    CharacterArray[108] = 'l';  //  0x6C
    CharacterArray[109] = 'm';  //  0x6D
    CharacterArray[110] = 'n';  //  0x6E
    CharacterArray[111] = 'o';  //  0x6F
    CharacterArray[112] = 'p';  //  0x70
    CharacterArray[113] = 'q';  //  0x71
    CharacterArray[114] = 'r';  //  0x72
    CharacterArray[115] = 's';  //  0x73
    CharacterArray[116] = 't';  //  0x74
    CharacterArray[117] = 'u';  //  0x75
    CharacterArray[118] = 'v';  //  0x76
    CharacterArray[119] = 'w';  //  0x77
    CharacterArray[120] = 'x';  //  0x78
    CharacterArray[121] = 'y';  //  0x79
    CharacterArray[122] = 'z';  //  0x7A
    CharacterArray[123] = '{';  //  0x7B
    CharacterArray[124] = '|';  //  0x7C
    CharacterArray[125] = '}';  //  0x7D
    CharacterArray[126] = '~';  //  0x7E


    // Characters from the data.
    CharacterArray[127] = '‘'; //     0x2018
    CharacterArray[128] = '’'; //     0x2019
    CharacterArray[129] = '‚'; //     0x201A
    CharacterArray[130] = '“'; //    0x201C
    CharacterArray[131] = '”'; //    0x201D
    CharacterArray[132] = '→'; //    0x2192

    for( int Count = 133; Count < 161; Count++ )
      CharacterArray[Count] = MarkersDelimiters.NoCharacter;

    CharacterArray[161] = '¡';  //  0xA1
    CharacterArray[162] = '¢';  //  0xA2
    CharacterArray[163] = '£';  //  0xA3
    CharacterArray[164] = '¤';  //  0xA4
    CharacterArray[165] = '¥';  //  0xA5
    CharacterArray[166] = '¦';  //  0xA6
    CharacterArray[167] = '§';  //  0xA7
    CharacterArray[168] = '¨';  //  0xA8
    CharacterArray[169] = '©';  //  0xA9
    CharacterArray[170] = 'ª';  //  0xAA
    CharacterArray[171] = '«';  //  0xAB
    CharacterArray[172] = '¬';  //  0xAC
    CharacterArray[173] = MarkersDelimiters.NoCharacter;  //  0xAD
    CharacterArray[174] = '®';  //  0xAE
    CharacterArray[175] = '¯';  //  0xAF
    CharacterArray[176] = '°';  //  0xB0
    CharacterArray[177] = '±';  //  0xB1
    CharacterArray[178] = '²';  //  0xB2
    CharacterArray[179] = '³';  //  0xB3
    CharacterArray[180] = '´';  //  0xB4
    CharacterArray[181] = 'µ';  //  0xB5
    CharacterArray[182] = '¶';  //  0xB6
    CharacterArray[183] = '·';  //  0xB7
    CharacterArray[184] = '¸';  //  0xB8
    CharacterArray[185] = '¹';  //  0xB9
    CharacterArray[186] = 'º';  //  0xBA
    CharacterArray[187] = '»';  //  0xBB
    CharacterArray[188] = '¼';  //  0xBC
    CharacterArray[189] = '½';  //  0xBD
    CharacterArray[190] = '¾';  //  0xBE
    CharacterArray[191] = '¿';  //  0xBF
    CharacterArray[192] = 'À';  //  0xC0
    CharacterArray[193] = 'Á';  //  0xC1
    CharacterArray[194] = 'Â';  //  0xC2
    CharacterArray[195] = 'Ã';  //  0xC3
    CharacterArray[196] = 'Ä';  //  0xC4
    CharacterArray[197] = 'Å';  //  0xC5
    CharacterArray[198] = 'Æ';  //  0xC6
    CharacterArray[199] = 'Ç';  //  0xC7
    CharacterArray[200] = 'È';  //  0xC8
    CharacterArray[201] = 'É';  //  0xC9
    CharacterArray[202] = 'Ê';  //  0xCA
    CharacterArray[203] = 'Ë';  //  0xCB
    CharacterArray[204] = 'Ì';  //  0xCC
    CharacterArray[205] = 'Í';  //  0xCD
    CharacterArray[206] = 'Î';  //  0xCE
    CharacterArray[207] = 'Ï';  //  0xCF
    CharacterArray[208] = 'Ð';  //  0xD0
    CharacterArray[209] = 'Ñ';  //  0xD1
    CharacterArray[210] = 'Ò';  //  0xD2
    CharacterArray[211] = 'Ó';  //  0xD3
    CharacterArray[212] = 'Ô';  //  0xD4
    CharacterArray[213] = 'Õ';  //  0xD5
    CharacterArray[214] = 'Ö';  //  0xD6
    CharacterArray[215] = '×';  //  0xD7
    CharacterArray[216] = 'Ø';  //  0xD8
    CharacterArray[217] = 'Ù';  //  0xD9
    CharacterArray[218] = 'Ú';  //  0xDA
    CharacterArray[219] = 'Û';  //  0xDB
    CharacterArray[220] = 'Ü';  //  0xDC
    CharacterArray[221] = 'Ý';  //  0xDD
    CharacterArray[222] = 'Þ';  //  0xDE
    CharacterArray[223] = 'ß';  //  0xDF
    CharacterArray[224] = 'à';  //  0xE0
    CharacterArray[225] = 'á';  //  0xE1
    CharacterArray[226] = 'â';  //  0xE2
    CharacterArray[227] = 'ã';  //  0xE3
    CharacterArray[228] = 'ä';  //  0xE4
    CharacterArray[229] = 'å';  //  0xE5
    CharacterArray[230] = 'æ';  //  0xE6
    CharacterArray[231] = 'ç';  //  0xE7
    CharacterArray[232] = 'è';  //  0xE8
    CharacterArray[233] = 'é';  //  0xE9
    CharacterArray[234] = 'ê';  //  0xEA
    CharacterArray[235] = 'ë';  //  0xEB
    CharacterArray[236] = 'ì';  //  0xEC
    CharacterArray[237] = 'í';  //  0xED
    CharacterArray[238] = 'î';  //  0xEE
    CharacterArray[239] = 'ï';  //  0xEF
    CharacterArray[240] = 'ð';  //  0xF0
    CharacterArray[241] = 'ñ';  //  0xF1
    CharacterArray[242] = 'ò';  //  0xF2
    CharacterArray[243] = 'ó';  //  0xF3
    CharacterArray[244] = 'ô';  //  0xF4
    CharacterArray[245] = 'õ';  //  0xF5
    CharacterArray[246] = 'ö';  //  0xF6
    CharacterArray[247] = '÷';  //  0xF7
    CharacterArray[248] = 'ø';  //  0xF8
    CharacterArray[249] = 'ù';  //  0xF9
    CharacterArray[250] = 'ú';  //  0xFA
    CharacterArray[251] = 'û';  //  0xFB
    CharacterArray[252] = 'ü';  //  0xFC
    CharacterArray[253] = 'ý';  //  0xFD
    CharacterArray[254] = 'þ';  //  0xFE
    CharacterArray[255] = 'ÿ';  //  0xFF

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in CharacterIndex.ReindexFromFile()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal int GetCharacterIndex( char GetFor )
    {
    switch( (int)GetFor )
      {
      case (int)'✀': return 0;  // NoCharacter

      // Markers / Delimiters.
      case (int)'➀': return 1;
      case (int)'➁': return 2;
      case (int)'➂': return 3;
      case (int)'➃': return 4;
      case (int)'➄': return 5;
      case (int)'➅': return 6;
      case (int)'➆': return 7;
      case (int)'➇': return 8;
      case (int)'➈': return 9;
      case (int)'➉': return 10;
      case (int)'➊': return 11;
      case (int)'➋': return 12;

      // Carriage Return.
      case (int)'\r': return 13;


      case (int)'➌': return 14;
      case (int)'➍': return 15;
      case (int)'➎': return 16;
      case (int)'➏': return 17;
      case (int)'➐': return 18;
      case (int)'➑': return 19;
      case (int)'➒': return 20;
      case (int)'➓': return 21;


    // Characters from the data.
      case (int)'œ': return 22;
      case (int)'š': return 23;
      case (int)'ƒ': return 24;
      case (int)'˜': return 25;
      case (int)'–': return 26;
      case (int)'—': return 27;
      case (int)'•': return 28;
      case (int)'…': return 29;
      case (int)'€': return 30;
      case (int)'™': return 31;


      // ASCII:
      case (int)' ': return 32;
      case (int)'!': return 33;
      case (int)'"': return 34;
      case (int)'#': return 35;
      case (int)'$': return 36;
      case (int)'%': return 37;
      case (int)'&': return 38;
      case (int)'\'': return 39;
      case (int)'(': return 40;
      case (int)')': return 41;
      case (int)'*': return 42;
      case (int)'+': return 43;
      case (int)',': return 44;
      case (int)'-': return 45;
      case (int)'.': return 46;
      case (int)'/': return 47;
      case (int)'0': return 48;
      case (int)'1': return 49;
      case (int)'2': return 50;
      case (int)'3': return 51;
      case (int)'4': return 52;
      case (int)'5': return 53;
      case (int)'6': return 54;
      case (int)'7': return 55;
      case (int)'8': return 56;
      case (int)'9': return 57;
      case (int)':': return 58;
      case (int)';': return 59;
      case (int)'<': return 60;
      case (int)'=': return 61;
      case (int)'>': return 62;
      case (int)'?': return 63;
      case (int)'@': return 64;
      case (int)'A': return 65;
      case (int)'B': return 66;
      case (int)'C': return 67;
      case (int)'D': return 68;
      case (int)'E': return 69;
      case (int)'F': return 70;
      case (int)'G': return 71;
      case (int)'H': return 72;
      case (int)'I': return 73;
      case (int)'J': return 74;
      case (int)'K': return 75;
      case (int)'L': return 76;
      case (int)'M': return 77;
      case (int)'N': return 78;
      case (int)'O': return 79;
      case (int)'P': return 80;
      case (int)'Q': return 81;
      case (int)'R': return 82;
      case (int)'S': return 83;
      case (int)'T': return 84;
      case (int)'U': return 85;
      case (int)'V': return 86;
      case (int)'W': return 87;
      case (int)'X': return 88;
      case (int)'Y': return 89;
      case (int)'Z': return 90;
      case (int)'[': return 91;
      case (int)'\\': return 92;
      case (int)']': return 93;
      case (int)'^': return 94;
      case (int)'_': return 95;
      case (int)'`': return 96;
      case (int)'a': return 97;
      case (int)'b': return 98;
      case (int)'c': return 99;
      case (int)'d': return 100;
      case (int)'e': return 101;
      case (int)'f': return 102;
      case (int)'g': return 103;
      case (int)'h': return 104;
      case (int)'i': return 105;
      case (int)'j': return 106;
      case (int)'k': return 107;
      case (int)'l': return 108;
      case (int)'m': return 109;
      case (int)'n': return 110;
      case (int)'o': return 111;
      case (int)'p': return 112;
      case (int)'q': return 113;
      case (int)'r': return 114;
      case (int)'s': return 115;
      case (int)'t': return 116;
      case (int)'u': return 117;
      case (int)'v': return 118;
      case (int)'w': return 119;
      case (int)'x': return 120;
      case (int)'y': return 121;
      case (int)'z': return 122;
      case (int)'{': return 123;
      case (int)'|': return 124;
      case (int)'}': return 125;
      case (int)'~': return 126;


    // Characters from the data.
      case (int)'‘': return 127;
      case (int)'’': return 128;
      case (int)'‚': return 129;
      case (int)'“': return 130;
      case (int)'”': return 131;
      case (int)'→': return 132;


      // Extended Latin:
      case (int)'¡': return 161;
      case (int)'¢': return 162;
      case (int)'£': return 163;
      case (int)'¤': return 164;
      case (int)'¥': return 165;
      case (int)'¦': return 166;
      case (int)'§': return 167;
      case (int)'¨': return 168;
      case (int)'©': return 169;
      case (int)'ª': return 170;
      case (int)'«': return 171;
      case (int)'¬': return 172;
      // return 173;
      case (int)'®': return 174;
      case (int)'¯': return 175;
      case (int)'°': return 176;
      case (int)'±': return 177;
      case (int)'²': return 178;
      case (int)'³': return 179;
      case (int)'´': return 180;
      case (int)'µ': return 181;
      case (int)'¶': return 182;
      case (int)'·': return 183;
      case (int)'¸': return 184;
      case (int)'¹': return 185;
      case (int)'º': return 186;
      case (int)'»': return 187;
      case (int)'¼': return 188;
      case (int)'½': return 189;
      case (int)'¾': return 190;
      case (int)'¿': return 191;
      case (int)'À': return 192;
      case (int)'Á': return 193;
      case (int)'Â': return 194;
      case (int)'Ã': return 195;
      case (int)'Ä': return 196;
      case (int)'Å': return 197;
      case (int)'Æ': return 198;
      case (int)'Ç': return 199;
      case (int)'È': return 200;
      case (int)'É': return 201;
      case (int)'Ê': return 202;
      case (int)'Ë': return 203;
      case (int)'Ì': return 204;
      case (int)'Í': return 205;
      case (int)'Î': return 206;
      case (int)'Ï': return 207;
      case (int)'Ð': return 208;
      case (int)'Ñ': return 209;
      case (int)'Ò': return 210;
      case (int)'Ó': return 211;
      case (int)'Ô': return 212;
      case (int)'Õ': return 213;
      case (int)'Ö': return 214;
      case (int)'×': return 215;
      case (int)'Ø': return 216;
      case (int)'Ù': return 217;
      case (int)'Ú': return 218;
      case (int)'Û': return 219;
      case (int)'Ü': return 220;
      case (int)'Ý': return 221;
      case (int)'Þ': return 222;
      case (int)'ß': return 223;
      case (int)'à': return 224;
      case (int)'á': return 225;
      case (int)'â': return 226;
      case (int)'ã': return 227;
      case (int)'ä': return 228;
      case (int)'å': return 229;
      case (int)'æ': return 230;
      case (int)'ç': return 231;
      case (int)'è': return 232;
      case (int)'é': return 233;
      case (int)'ê': return 234;
      case (int)'ë': return 235;
      case (int)'ì': return 236;
      case (int)'í': return 237;
      case (int)'î': return 238;
      case (int)'ï': return 239;
      case (int)'ð': return 240;
      case (int)'ñ': return 241;
      case (int)'ò': return 242;
      case (int)'ó': return 243;
      case (int)'ô': return 244;
      case (int)'õ': return 245;
      case (int)'ö': return 246;
      case (int)'÷': return 247;
      case (int)'ø': return 248;
      case (int)'ù': return 249;
      case (int)'ú': return 250;
      case (int)'û': return 251;
      case (int)'ü': return 252;
      case (int)'ý': return 253;
      case (int)'þ': return 254;
      case (int)'ÿ': return 255;

      }

    return -1;
    }



  internal char GetCharacter( int Index )
    {
    if( Index < 0 )
      return MarkersDelimiters.NoCharacter;

    if( Index >= CharacterArrayLength )
      return MarkersDelimiters.NoCharacter;

    return CharacterArray[Index];
    }


  internal byte[] StringToBytes( string InString )
    {
    int Where = 0;
    byte[] Result = new byte[InString.Length * 2];
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      int Index = GetCharacterIndex( InString[Count] );
      MForm.HuffmanCd.AddLeaf( Index );
      if( Index >= 0 )
        {
        Result[Where] = (byte)Index;
        Where++;
        }
      else
        {
        Result[Where] = (byte)0;
        Where++;
        uint BigChar = (uint)InString[Count];
        Result[Where] = (byte)((BigChar >> 8) & 0xFF);
        Where++;
        Result[Where] = (byte)(BigChar & 0xFF);
        Where++;
        }
      }

    Array.Resize( ref Result, Where );
    return Result;
    }



  internal string BytesToString( byte[] Buffer )
    {
    if( Buffer == null )
      return "";

    StringBuilder SBuilder = new StringBuilder();

    int Where = 0;
    int BufLength = Buffer.Length;
    for( int Count = 0; Count < BufLength; Count++ )
      {
      if( Buffer[Where] == (byte)0 )
        {
        Where++;
        if( Where >= BufLength )
          break;

        uint BigChar = Buffer[Where];
        BigChar <<= 8;
        Where++;
        if( Where >= BufLength )
          break;

        BigChar |= Buffer[Where];
        SBuilder.Append( (char)BigChar );
        }
      else
        {
        char OneChar = GetCharacter( Buffer[Where] );
        if( OneChar == MarkersDelimiters.NoCharacter )
          return ""; // It's corrupt data.

        SBuilder.Append( OneChar );
        }

      Where++;
      if( Where >= BufLength )
        break;

      }

    return SBuilder.ToString();
    }



  }
}
