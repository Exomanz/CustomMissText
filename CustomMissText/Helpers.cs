using IPA.Utilities;
using System.IO;

namespace CustomMissText
{
    internal static class Helpers
    {
        public static readonly string DIRECTORY = Path.Combine(UnityGame.UserDataPath, "CustomMissText");
        public static readonly string DEFAULT_FILE_PATH = Path.Combine(DIRECTORY, "Default.txt");

        public const string HARMONY_ID = "bs.Exo.CustomMissText";
        public const string DEFAULT_CONFIG = @"# CustomMissText v1.1.0
# by Exomanz and Arti
# 
# Use # for comments!
# Separate entries with empty lines!
HECK

F

OwO

HITN'T

OOF

MS.

115
<size=50%>just kidding</size>

HISS

NOPE

WHOOSH

OOPS

C-C-C-COMBO
BREAKER

I MEANT TO
DO THAT

MOSS

MASS

MESS

MUSS

MYTH

KISS

# The following lines were suggested by @E8 on BSMG
MISSCLICK

HIT OR MISS

LAG

TRACKING

LUL";
    }
}
