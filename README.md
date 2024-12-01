# CustomMissText
Reboot of Arti's original CustomMissText mod for Beat Saber.

## Dependencies:
* BSIPA: v4.3.5+
* BeatSaberMarkupLanguage: v1.12.3+

## Current Features:
* Replaces the "MISS" image with a text object that displays a random entry from the config file for each note missed.
* Offers TextMeshPro styling, including colors, size, and much more. See the [TextMeshPro documentation](http://digitalnativestudios.com/textmeshpro/docs/rich-text/ "TextMeshPro Docs") for more information.
* Automatic directory and text file creation with support to be remade during runtime in the event it gets accidentally deleted.
* Font bloom support

## Known Issues:
* If more than one note is missed on the same beat, the same entry will be picked for all notes.

## Config File Format
Config files must be `.txt` files, and be formatted as follows.
```
# Comments are always notated with #'s. If you don't have one, it will be marked as an entry.
# Text entries will be chained as one until an empty line is detected.
THIS IS
ALL ONE
ENTRY

This is also one entry.

# Don't forget that TMP Styling can be used here, too! You can leave your tags unclosed (despite what it
# says in the config) as long as it's either the end of the entry, or you want all of the entry to be that one tag.
<size=+30>Hello world!

Hello world! (but smaller)
# The entry above isn't affected by the size tag that was left unclosed since it's a different entry.
```

## Installation
Grab the latest version from the [releases](https://github.com/Exomanz/CustomMissText/releases/latest "releases") page and install it in your Plugins folder at your Beat Saber directory.
