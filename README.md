<h1 align="center">GSTools</h1>
<h4 align="center">Gyakuten Saiban data manipulation tools</h4>
<div align="center">
	<a href="https://github.com/davicr/GSTools/blob/master/LICENSE">
		<img src="https://img.shields.io/github/license/davicr/GSTools.svg"/>
	</a>
</div>

## Information
GSTools, including GSEncTools and GSMdtTools can be used to modify Ace Attorney: Trilogy data files, specifically MDT script files, although the GSEncTools may be used to decrypt other game information. GSMdtTools is a work in progress and may generate scripts that aren't 1:1 with the originals.

If you contribute to these tools in any way, please share your changes with me!

## Usage
```
.\GSEncTools.exe sc0_text_u.mdt
.\GSEncTools.exe -e new_script.mdt
```
```
.\GSMdtTools.exe sc0_text_u.mdt.dec
.\GsMdtTools.exe -e new_script.json
```

## GSMdtTools Warning
GSMdtTools has only been tested with USA ``(_u.mdt)`` dialog scripts and might need extra character conversion information to work with scripts from other regions.

## Thanks
* Users that posted information about the file formats in ZenHAX
* Raymonf
