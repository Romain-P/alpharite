### Misc

You can find de-obfuscated libraries in `previous-modules`
Might be used as `SDK` for your projects or as a ready-to-use patched module (e.g for the wallhack one check 1.2)

* Obfuscation type was SmartAssembly
* Internal types have been changed to public
* It is possible to re-compile from dnSpy since 1.1
* Latest de-obfuscation date: 8 May 2020

Latest patched module is at the root of this folder.  
MergedUnity.dll have been patched for loading our customer AlphaRite.dll once the game started.  
For using AlphaRite, move both modules (MergedUnity, AlphaRite) inside `_Data/Managed` directory