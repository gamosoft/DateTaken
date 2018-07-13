# DateTaken
Process your photos to organize in folders automatically using the date taken metadata.

I have an iPhone and copy my photos manually to my local storage, but I like to have them organized in a folder structure such as "c:\photos\year\month\day".

Having to do this manually is a pain, so I've created this small tool to automatically parse the date a photo was taken and nest it in the correct folder.

For those photos that don't have one, they will be ignored and left in the source folder.

The hierarchy will be created under the source folder. If you start under c:\somephotos it will create stuff like c:\somephotos\2018\..., and move or copy them depending on the flag used in the code.

The code is very silly, but if you have any questions please let me know. :-)

Hope you like it!
