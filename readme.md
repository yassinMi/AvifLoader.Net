# AvifLoader.Net

a simple .net wraper for https://github.com/AOMediaCodec/libavif 

the native win-x64 ```libMiAvifJpgWraper.dll```  must be added manually to the project

## usage

```c#
using AvifLoaderNET;

ImageSource imgSrc =  AvifLoader.AvifToImageSource("input.avif"); //uses the rgb pixels data

byte[] _ =  AvifLoader.AvifToJpeg("input.avif"); //converts to jpeg
File.WriteAllBytes("output.jpg", _);

```


